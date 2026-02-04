using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportEntities;
using Utility;
using SchoolEntities.Transport;

namespace DataCommunicator.TransportDC
{
    public class BulkDocumentDetailsDC
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
        public BulkDocumentDetailsDC()
        {
        }

        /// <summary>
        /// Initialise member variable.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiInsertedById"></param>
        public BulkDocumentDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        }

        #endregion

        #region Method(s)

        public void Save(int aiDocumentId, string sXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentXmlDetails", sXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveBulkDocumentDetails]");
            }
        }

        public List<GetBulkDocumentDetails> GetDocumentsDetails(int aId, string asFilter, bool abShowAll)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ShowAll", abShowAll, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetBulkDocumentDetails]"))
                {
                    List<GetBulkDocumentDetails> lstDocumentDetails = new List<GetBulkDocumentDetails>();

                    while (oSqlDataReader.Read())
                    {
                        lstDocumentDetails.Add(new GetBulkDocumentDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            VehicleId = Convert.ToInt32(oSqlDataReader["VehicleId"]),
                            VehicleNumber = Convert.ToString(oSqlDataReader["VehicleNumber"]),
                            Title = Convert.ToString(oSqlDataReader["Title"]),
                            StartDate = (oSqlDataReader["StartDate"] == DBNull.Value? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["StartDate"])),
                            EndDate = (oSqlDataReader["EndDate"] == DBNull.Value? DateTime.MinValue: Convert.ToDateTime(oSqlDataReader["EndDate"])),
                            Amount = Convert.ToInt32(oSqlDataReader["Amount"]),
                            PolicyNo = Convert.ToString(oSqlDataReader["PolicyNo"]),
                            Description = Convert.ToString(oSqlDataReader["Description"]),
                            FileName = Convert.ToString(oSqlDataReader["FileName"])
                        });
                    }
                    return lstDocumentDetails;
                }
                
            }
        }

        public void DeleteBulkDocument(int Id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", Id, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_DeleteBulkDocuments]");
            }
        }

        public string Validate(int aiDocumentId, string asDatesXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DatesXml", asDatesXml, SqlDbType.Xml);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Message", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output,200);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[ValidateVehicleDocumentDetails]");
                return oSqlParameter.Value.ToString();
            }
        }

        #endregion
    }
} 