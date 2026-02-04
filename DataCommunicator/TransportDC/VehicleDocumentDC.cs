// File Name - VehcleDocument.cs
// Create By - Vishakha
// Created Date - 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportEntities;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database for insert/delete/update/ display of vehicle document.
    /// </summary>
    public class VehicleDocumentDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miInsertedById;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VehicleDocumentDC()
        {
        }

        /// <summary>
        /// Initialise member variable.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiInsertedById"></param>
        public VehicleDocumentDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        } 

        #endregion

        #region Public Method(s)
        /// <summary>
        ///  This method is used to save document.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="asFileName"></param>
        /// <param name="aiUserId"></param>
        public void SaveDocument(VehicleDocumentDetails oVehicleDocumentDetails)
        {
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", oVehicleDocumentDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", oVehicleDocumentDetails.VehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", oVehicleDocumentDetails.DocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Title", oVehicleDocumentDetails.Title, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Description", oVehicleDocumentDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartDate", oVehicleDocumentDetails.StartDate, SqlDbType.DateTime);
                if (oVehicleDocumentDetails.EndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("EndDate", oVehicleDocumentDetails.EndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("PolicyNo", oVehicleDocumentDetails.PolicyNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Amount", oVehicleDocumentDetails.Amount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FileName", oVehicleDocumentDetails.FileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveVehicleDocuments]");
            }
        }

        /// <summary>
        /// This method is used to delete document.
        /// </summary>
        /// <param name="iId"></param>
        public void DeleteDocument(int Id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", Id, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_DeleteVehicleDocuments]");
            }
        }

        /// <summary>
        /// This method is used to get list of documents.
        /// </summary>
        /// <param name="aiDocumentId"></param>
        /// <param name="asDocumentName"></param>
        /// <returns></returns>
        public List<Documents> GetDocumentList()
        {
            List<Documents> lstDocuments = new List<Documents>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetVehicleDocumentsList]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        Documents oDocuments = new Documents();
                        oDocuments.DocumentName = oSqlDataReader["DocumentName"].ToString();
                        oDocuments.DocumentId = oSqlDataReader["DocumentId"].ToInt();

                        lstDocuments.Add(oDocuments);
                    }
                }
                return lstDocuments;
            }
        }

        /// <summary>
        /// This method is used to get vehicle document details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiVehicleId"></param>
        /// <param name="aiDocumentId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<GetVehicleDocumentDetails> GetAll(int aiSchoolId, int aiAcademicYearId, int aiVehicleId, int aiDocumentId, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetVehicleDocumentDetails]"))
                {
                    List<GetVehicleDocumentDetails> lstVehicleDocumentDetails = new List<GetVehicleDocumentDetails>();

                    while (oSqlDataReader.Read())
                    {
                        lstVehicleDocumentDetails.Add(new GetVehicleDocumentDetails
                      {
                          Id = Convert.ToInt32(oSqlDataReader["Id"]),
                          DocumentName = Convert.ToString(oSqlDataReader["DocumentName"]),
                          Title = Convert.ToString(oSqlDataReader["Title"]),
                          StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
                          EndDate = (oSqlDataReader["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["EndDate"])),
                          FileName = Convert.ToString(oSqlDataReader["FileName"]),
                          TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"])
                      });
                    }
                    return lstVehicleDocumentDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to read vehicle document details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public GetVehicleDocumentDetails Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                GetVehicleDocumentDetails oGetVehicleDocumentDetails = new GetVehicleDocumentDetails();
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetDataToReadVehicleDocumentDetails]"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oGetVehicleDocumentDetails.Id = oSqlDataReader["Id"].ToInt();
                        oGetVehicleDocumentDetails.DocumentId = oSqlDataReader["DocumentId"].ToInt();
                        oGetVehicleDocumentDetails.VehicleId = oSqlDataReader["VehicleId"].ToInt();
                        oGetVehicleDocumentDetails.StartDate = oSqlDataReader["StartDate"].ToDateTime();
                        oGetVehicleDocumentDetails.EndDate = (oSqlDataReader["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["EndDate"]));
                        oGetVehicleDocumentDetails.FileName = oSqlDataReader["FileName"].ToString();
                        oGetVehicleDocumentDetails.PolicyNo = oSqlDataReader["PolicyNo"].ToString();
                        oGetVehicleDocumentDetails.Amount = oSqlDataReader["Amount"].ToInt();
                        oGetVehicleDocumentDetails.Title = oSqlDataReader["Title"].ToString();
                        oGetVehicleDocumentDetails.Description = oSqlDataReader["Description"].ToString();
                    }
                }
                return oGetVehicleDocumentDetails;
            }
        }
                
        #endregion

        public bool Validate(int aiDocumentId, int aiVehicleId, string asStartDate, string asEndDate, int aiId, string asTitle, string asPolicyNo, int aiCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("Title", asTitle, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PolicyNo", asPolicyNo, SqlDbType.NVarChar);
                
                if(asStartDate != string.Empty)
                    oSQLServerDbUtility.AddParameter("StartDate", asStartDate, SqlDbType.Date);

                if (asEndDate != string.Empty)
                    oSQLServerDbUtility.AddParameter("EndDate", asEndDate, SqlDbType.Date);

                SqlParameter oSqlParameter= oSQLServerDbUtility.AddParameter("IsValid", true, SqlDbType.Bit, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_ValidateVehicleDocument]");

                return oSqlParameter.Value.ToBool();
            }
        }
    }
}
