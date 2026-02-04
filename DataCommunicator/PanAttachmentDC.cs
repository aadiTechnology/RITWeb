// -----------------------------------------------------------------------
// <copyright file="PanAttachmentDC.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

// Class Name       :- PanAttachmentDC
// Purpose          :- This class is used to manage PAN Attachment details.
// Date Of creation :- 1/9/2014
// Author Name      :- Yogesh

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using SchoolEntities.Admin;

    public  class PanAttachmentDC
    {
        #region PUBLIC METHOD(S)

        /// <summary>
        /// This method is used to return PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <param name="aiShowAllDetails"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asSortExpression"></param>
        /// <returns></returns>
        public static List<PANAttachmentDetails> GetAllPanAttachmentDetails(int aiUserRoleId, int aiSchoolId, int aiAcademicYearId, string asNameFilter, int aiShowAllDetails, string asSortDirection, int aiStartRowIndex, int aiEndRowIndex, int aiCategoryId, int aiStdDivId, string asSortExpression, bool asIncludeLeftStudents)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NameFilter", asNameFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ShowAllDetails", aiShowAllDetails, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SortDirection ", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndRowIndex", aiEndRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IncludeLeftStudents", asIncludeLeftStudents, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPanAttachmentDetails"))
                    return LoadPANDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to return record count.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asNameFilter"></param>
        /// <param name="aiShowAllDetails"></param>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiStdDivId"></param>
        /// <returns></returns>
        public static int GetCountAllPanAttachmentDetails(int aiUserRoleId, int aiSchoolId, int aiAcademicYearId, string asNameFilter, int aiShowAllDetails, int aiCategoryId, int aiStdDivId, bool asIncludeLeftStudents)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NameFilter", asNameFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ShowAllDetails", aiShowAllDetails, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeLeftStudents", asIncludeLeftStudents, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetCountAllPanAttachmentDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to return PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiDocumentId"></param>
        /// <returns></returns>
        public PANAttachmentDetails Get(int aiUserId, int aiDocumentId)
        {
            PANAttachmentDetails oPANAttachmentDetails = null;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserPANDeails"))
                {
                    if (oSqlDataReader.HasRows && oSqlDataReader.Read())
                    {
                        oPANAttachmentDetails = new PANAttachmentDetails
                        {
                            Name = oSqlDataReader["Name"].ToString(),
                            PanNo = oSqlDataReader["PanNo"].ToString(),
                            NameonAadharCard = oSqlDataReader["NameonAadharCard"].ToString(),
                            PanAttachment = oSqlDataReader["PanAttachment"].ToString()
                        };
                    }
                }
            }
            return oPANAttachmentDetails;
        }

        /// <summary>
        /// This method is used to save PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiDocumentId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asPANNo"></param>
        /// <param name="asFileName"></param>
        /// <param name="aiUpdatedById"></param>
        public void Save(int aiDocumentId, int aiUserId, string asPANNo, string asNameonAadharCard, string asFileName, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PANNo", asPANNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("NameonAadharCard", asNameonAadharCard, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FileName", asFileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SavePANDetails");
            }
        }

        /// <summary>
        /// This method is used to delete PAN / Aadhar card details.
        /// </summary>
        /// <param name="aiDocumentId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUpdatedById"></param>
        public void Delete(int aiDocumentId, int aiUserId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeletePANDetails");
            }
        }

        #endregion

        #region PRIVATE METHOD(S)

        /// <summary>
        /// This method is used to load PAn / Aadhar card details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private static List<PANAttachmentDetails> LoadPANDetails(SqlDataReader aoSqlDataReader)
        {
            List<PANAttachmentDetails> lstPANAttachmentDetails = new List<PANAttachmentDetails>();
            PANAttachmentDetails oPANAttachmentDetails;
            while (aoSqlDataReader.Read())
            {
                oPANAttachmentDetails = new PANAttachmentDetails()
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    PanNo = Convert.ToString(aoSqlDataReader["PanNo"]),
                    PanAttachment = Convert.ToString(aoSqlDataReader["PanAttachment"]),
                    RollNo = Convert.ToInt32(aoSqlDataReader["Roll_No"]),
                    ClassName = Convert.ToString(aoSqlDataReader["ClassName"])
                };
                lstPANAttachmentDetails.Add(oPANAttachmentDetails);
            }

            return lstPANAttachmentDetails;
        }
        #endregion

        
    }
}
