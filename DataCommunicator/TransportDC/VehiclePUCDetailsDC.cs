using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SchoolEntities.Transport;
using System.Data.SqlClient;

namespace DataCommunicator.TransportDC
{
    public class VehiclePUCDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miInsertedById;

        #endregion

        #region Construstor(s)

        public VehiclePUCDetailsDC()
        {         
        }

        public VehiclePUCDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method i
        /// </summary>
        /// <returns></returns>
        public DataTable GetVehicalDetailsForComboBox()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[Transport].[usp_GetVehicalDetailsForFillCombo]");
            }
        }

        /// <summary>
        /// This method is used to save the PUC details in table.
        /// </summary>
        /// <param name="aoTransportPUCDetails"></param>
        public void Save(TransportPUCDetails aoTransportPUCDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PUCId", aoTransportPUCDetails.VehiclePUCId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aoTransportPUCDetails.VehicalId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SerialNo", aoTransportPUCDetails.SerialNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TestDate", aoTransportPUCDetails.TestDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ExpiryDate", aoTransportPUCDetails.ExpiryDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("NotificationDays", aoTransportPUCDetails.NoticicationDays, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentPhotos", aoTransportPUCDetails.DocumnetPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Note", aoTransportPUCDetails.PUCNote, SqlDbType.NVarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveVehiclePUCDetails]");
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
        public List<TransportPUCDetails> GetAll(int aiSchoolId, bool abShowOldRecord, string asSortExpression, int aiStartIndex, int aiEndIndex, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowOldRecord", abShowOldRecord, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SortExpr", "ORDER BY " +  asSortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetAllVehiclePUCDetails]"))
                    return FillVehicleDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to delete PUC details.
        /// </summary>
        /// <param name="aiVehiclePUCId"></param>
        public List<string> Delete(int aiVehiclePUCId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<string> lstFileNames = new List<string>();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehiclePUCId", aiVehiclePUCId, SqlDbType.Int);

                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_DeleteVehiclePUCDetails]"))
                {
                    while (oSqlDataReader.Read())
                        lstFileNames.Add(oSqlDataReader["FileName"].ToString());
                }
                return lstFileNames;
            }
        }

        /// <summary>
        /// This method is used to get Transport Option Image details for Fill list view.
        /// </summary>
        /// <param name="aiTypeId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<TransportOptionImages> GetTransportOptionImages(int aiTypeId, int aiDetailsId, int aiVehicleId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DetailsId", aiDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetTransportOptionImages]"))
                    return FillTransportOptionImages(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to delete transport Option images.
        /// </summary>
        /// <param name="aiDetailsId"></param>
        /// <param name="aiTypeId"></param>
        public string DeleteTransportOptionImage(int aiDetailsId, int aiTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("DetailId", aiDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);

                string sFileName = string.Empty;

                using (SqlDataReader oSqlDataReader =  oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_DeleteTransportOptionImage]"))
                {
                    if(oSqlDataReader.Read())
                        sFileName = oSqlDataReader["FileName"].ToString();
                }
                return sFileName;
            }
        }

        /// <summary>
        /// This method is used to get vehicle PUC details for Edit.
        /// </summary>
        /// <param name="aiPUCId"></param>
        /// <returns></returns>
        public TransportPUCDetails Get(int aiPUCId)
        {            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                TransportPUCDetails oTransportPUCDetails = new TransportPUCDetails();

                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PUCId", aiPUCId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetVehiclePUCDetails]"))
                    if (oSqlDataReader.Read())
                    {
                        oTransportPUCDetails.VehiclePUCId = Convert.ToInt32(oSqlDataReader["Id"]);
                        oTransportPUCDetails.VehicalId = Convert.ToInt32(oSqlDataReader["VehicleId"]);
                        oTransportPUCDetails.SerialNumber = Convert.ToString(oSqlDataReader["SerialNumber"]);
                        oTransportPUCDetails.TestDate = Convert.ToDateTime(oSqlDataReader["TestDate"]);
                        oTransportPUCDetails.ExpiryDate = Convert.ToDateTime(oSqlDataReader["ExpiryDate"]);
                        oTransportPUCDetails.NoticicationDays = Convert.ToInt32(oSqlDataReader["NotificationDays"]);

                        if (oSqlDataReader["Note"] != DBNull.Value)
                            oTransportPUCDetails.PUCNote = Convert.ToString(oSqlDataReader["Note"]);
                    }
                return oTransportPUCDetails;
            }
        }

        #endregion

        #region Private method(s)

        /// <summary>
        /// This method is used to fill the Vehicle details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private List<TransportPUCDetails> FillVehicleDetails(SqlDataReader oSqlDataReader)
        {
            List<TransportPUCDetails> lstTransportPUCDetails = new List<TransportPUCDetails>();
            while (oSqlDataReader.Read())
            {
                TransportPUCDetails oTransportPUCDetails = new TransportPUCDetails();
                oTransportPUCDetails.VehicalId = Convert.ToInt32(oSqlDataReader["VehicleId"]);
                oTransportPUCDetails.VehiclePUCId = Convert.ToInt32(oSqlDataReader["VehiclePUCId"]);
                oTransportPUCDetails.VehicalNumber = Convert.ToString(oSqlDataReader["VehicleNumber"]);
                oTransportPUCDetails.SerialNumber = Convert.ToString(oSqlDataReader["SerialNumber"]);
                oTransportPUCDetails.TestDate = Convert.ToDateTime(oSqlDataReader["TestDate"]);
                oTransportPUCDetails.ExpiryDate = Convert.ToDateTime(oSqlDataReader["ExpiryDate"]);
                oTransportPUCDetails.NoticicationDays = Convert.ToInt32(oSqlDataReader["NotificationDays"]);
                oTransportPUCDetails.TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]);
                oTransportPUCDetails.IsFileExists = Convert.ToBoolean(oSqlDataReader["IsFileExists"]);
                oTransportPUCDetails.IsLocked = Convert.ToBoolean(oSqlDataReader["IsLocked"]);
                oTransportPUCDetails.IsOldRecord = Convert.ToBoolean(oSqlDataReader["IsOldRecord"]);

                lstTransportPUCDetails.Add(oTransportPUCDetails);
            }
            return lstTransportPUCDetails;
        }

        /// <summary>
        /// This method is used to fill Transport Option Images.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private List<TransportOptionImages> FillTransportOptionImages(SqlDataReader oSqlDataReader)
        {
            List<TransportOptionImages> lstTransportOptionImages = new List<TransportOptionImages>();
            {
                while (oSqlDataReader.Read())
                {
                    TransportOptionImages oTransportOptionImages = new TransportOptionImages();
                    oTransportOptionImages.TypeId = Convert.ToInt32(oSqlDataReader["TypeId"]);
                    oTransportOptionImages.Type = Convert.ToString(oSqlDataReader["Type"]);                    
                    oTransportOptionImages.Vehicle = Convert.ToString(oSqlDataReader["VehicleNumber"]);
                    oTransportOptionImages.Images = Convert.ToString(oSqlDataReader["FileName"]);
                    oTransportOptionImages.DetailId = Convert.ToInt32(oSqlDataReader["DetailId"]);

                    lstTransportOptionImages.Add(oTransportOptionImages);
                }
                return lstTransportOptionImages;
            }
        }

        #endregion
    }
}
