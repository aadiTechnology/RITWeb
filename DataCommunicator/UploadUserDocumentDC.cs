using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoUploadEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;
namespace DataCommunicator
{
    public class UploadUserDocumentDC
    {
        #region "Data Members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        public List<UserRolewiseDocumentDetails> molstUserRolewiseDocumentDetails = new List<UserRolewiseDocumentDetails>();

        #endregion

        #region "Constructors"

        public UploadUserDocumentDC()
        { }

        public UploadUserDocumentDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// This method is used to get user details for documents upload details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiDocumentTypeId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="aiTotalRows"></param>
        /// <returns></returns>
        public List<UserRolewiseDocumentDetails> GetUserDetailsForDocumentUpload(int aiSchoolId, int aiAcademicYearId, int aiDocumentTypeId, int aiUserRoleId, string asUserName, int iEndIndex,
            int startRowIndex, out int aiTotalRows, int aiUser, bool asLeftStudent, int aiStandardDivisionId) //
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentTypeId", aiDocumentTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", Utility.StringUtility.ReplaceSingleQuoteInString(asUserName, true), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUser, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeftStudent", asLeftStudent, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForDocumentUpload"))
                    return SetUsersDetails(oSqlDataReader, out aiTotalRows);
            }
        }

        /// <summary>
        /// This method is used to save the user document details.
        /// </summary>
        /// <param name="aiDocumentTypeId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserDocumentDetails"></param>
        /// <param name="aiInsertedById"></param>
        public void Save(int aiDocumentTypeId, int aiUserRoleId, string asUserDocumentDetails, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentTypeId", aiDocumentTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserDocumentDetails", asUserDocumentDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserDocumentDetails");
            }
        }


       

        /// <summary>
        /// This method is used to delete user document details.
        /// </summary>
        /// <param name="aiDocumentId"></param>
        /// <param name="aiDocumentTypeId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUpdatedById"></param>
        public void Delete(int aiDocumentId, int aiDocumentTypeId, int aiUserId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentTypeId", aiDocumentTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteUserDocumentDetails");
            }
        }

        

        /// <summary>
        /// This method is used to get user document details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <returns></returns>
        public List<UserRolewiseDocumentDetails> GetUserDocumentDetails(int aiUserId, int aiFinancialYearId)
        {
            List<UserRolewiseDocumentDetails> lstUserRolewiseDocumentDetails = new List<UserRolewiseDocumentDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDocumentDetails"))
                {
                    UserRolewiseDocumentDetails oUserRolewiseDocumentDetails = null;
                    while (oSqlDataReader.Read())
                    {
                        oUserRolewiseDocumentDetails = new UserRolewiseDocumentDetails()
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            DocumentFilePath = Convert.ToString(oSqlDataReader["FileName"]),
                            DocumentTypeId = Convert.ToInt32(oSqlDataReader["DocumentTypeId"]),
                            DocumentTypeName = Convert.ToString(oSqlDataReader["DocumentType"]),
                            Year = Convert.ToString(oSqlDataReader["DocumentYear"])
                            
                        };
                        lstUserRolewiseDocumentDetails.Add(oUserRolewiseDocumentDetails);
                    }
                }
            }

            return lstUserRolewiseDocumentDetails;
        }
       
       
        #endregion

        #region "Private Methods"

        /// <summary>
        /// This methos is used to fill the user detail list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<UserRolewiseDocumentDetails> SetUsersDetails(SqlDataReader aoSqlDataReader, out int aiTotalRows)
        {
            UserRolewiseDocumentDetails oUserRolewiseDocumentDetails = null;
            aiTotalRows = 0;
            if (aoSqlDataReader.Read())
            {
                aiTotalRows = Convert.ToInt32(aoSqlDataReader["TotalRows"]);
                aoSqlDataReader.NextResult();
                while (aoSqlDataReader.Read())
                {
                    oUserRolewiseDocumentDetails = new UserRolewiseDocumentDetails()
                    {
                        RowNo = Convert.ToInt32(aoSqlDataReader["RowNo"]),
                        UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                        UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                        DocumentFilePath = Convert.ToString(aoSqlDataReader["DocumentFileName"]),
                        DocumentId = Convert.ToInt32(aoSqlDataReader["Id"]),
                        DocumentTypeId = Convert.ToInt32(aoSqlDataReader["DocumentTypeId"])
                         , DocumentTypeName = Convert.ToString(aoSqlDataReader["DocumentTypeName"])
                       , PanNo = Convert.ToString(aoSqlDataReader["PanNo"])                       
                        ,EmployeeNo = Convert.ToString(aoSqlDataReader["EmployeeNo"])
                    };
                    molstUserRolewiseDocumentDetails.Add(oUserRolewiseDocumentDetails);
                }
            }
            return molstUserRolewiseDocumentDetails;
        }
       

        /// <summary>
        /// This method is used to get users .
        /// </summary>
        /// <returns></returns>
        public DataTable GetUsers(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiStdDivId)  ////
        {
            string sQueryStmt = "";
            if (aiUserRoleId != 3)
            {
                sQueryStmt = " SELECT APU.UserId , " +
                                   " APU.UserName " +
                                   " FROM Vw_AllPayrollUsers APU INNER JOIN UsersStaffGroupsAssociation USGA"+
                                   " ON APU.UserId = USGA.UserId" +
                                   " WHERE APU.Is_Deleted = N'" + Constants.C_NO + "'" +
                                     " AND APU.SchoolId = " + aiSchoolId +
                                        " AND APU.UserRoleId = " + aiUserRoleId +
                                        " AND USGA.UserId IS NOT NULL"+
								        " AND USGA.Is_Locked = 0"+
								        " AND USGA.Is_Deleted = 'N'"+	    
                "order by UserName";
            }
            else
            {
                sQueryStmt = "select user_id as UserId, cast(roll_no as nvarchar(10))+' - ' + studentname as UserName" +
                            " from vw_GetAllStudentsForStandardDivision WHERE SchoolWise_Standard_Division_Id=" + aiStdDivId+" Order by Roll_No ASC";
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQueryStmt);
        }
        /// <summary>
        /// This method is used to get document types .
        /// </summary>
        /// <returns></returns>
        public DataTable GetDocumentTypes(int aiSchoolId)  //
        {
            string sQueryStmt = " SELECT Id , " +
                              " Name " +
                              " FROM DocumentType " +
                              " WHERE IsDeleted = N'" + Constants.I_ZERO + "'" +
                                " AND SchoolId = " + aiSchoolId;



            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQueryStmt);
        }

        public DataTable GetUserWisePanNo(int aiSchoolId,int aiAcademicYearId,string aiuserId)  //
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiuserId, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserDetailsPanNo");
            }

        }

        public DataTable GetStudentLCFileName(int aischoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aischoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentLCDocumnet");
            }
        }

        #endregion
    }
}
