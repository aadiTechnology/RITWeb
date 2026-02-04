// Class Name       :- UserRolewisePhotoUploadDC
// Purpose          :- This class is used to manage UserRolewisePhotoUpload details.
// Date Of creation :- 5/11/2011
// Author Name      :- Vinod

// Modification Purpose :- To Remove uploaded photos in bulk.
// Date Of modification :- 05/09/2014
// Modified by          :- Yogesh

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using Utility;
using PhotoUploadEntities;

namespace DataCommunicator
{
    public class UserRolewisePhotoUploadDC
    {
        #region "Data Members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private UserRolewisePhotoDetails moUserRolewisePhotoDetails = null;
        public List<UserRolewisePhotoDetails> molstUserRolewisePhotoDetails = new List<UserRolewisePhotoDetails>();

        #endregion "Data Members"

        #region "Constructors"

        public UserRolewisePhotoUploadDC()
        {
            moUserRolewisePhotoDetails = new UserRolewisePhotoDetails();
        }

        public UserRolewisePhotoUploadDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            moUserRolewisePhotoDetails = new UserRolewisePhotoDetails();
        }

        #endregion "Constructors"

        #region "Properties"

        public UserRolewisePhotoDetails oUserRolewisePhotoDetails
        {
            get { return moUserRolewisePhotoDetails; }
            set { moUserRolewisePhotoDetails = value; }
        }

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This methos is used to get the User Rolewise Photo Upload details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asUserName"></param>
        /// <param name="abChkUserWithPhotoFlag"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public List<UserRolewisePhotoDetails> GetUserDetailsForPhotoUplaod(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, string asUserName, bool abChkUserWithPhotoFlag, int iEndIndex, int startRowIndex, int aiStandardId, int aiDivisionId, out int aiTotalRows)
        {
            string sStudentFilter = CreateFilter(aiStandardId, aiDivisionId, asUserName, aiUserRoleId);
            aiTotalRows = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", Utility.StringUtility.ReplaceSingleQuoteInString(asUserName, true), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FilterWithPhoto", abChkUserWithPhotoFlag, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentFilter", sStudentFilter, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUsersPhotoDetails"))
                    return SetUsersDetails(oSqlDataReader, out aiTotalRows);
            }
            //return moUserRolewisePhotoUploadDC.GetUserDetailsForPhotoUplaod(aiSchoolId, aiAcademicYearId,aiUserRoleId, asUserName, abChkUserWithPhotoFlag, iEndIndex, startRowIndex);
        }

        /// <summary>
        /// This methos is used to fill the user detail list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<UserRolewisePhotoDetails> SetUsersDetails(SqlDataReader aoSqlDataReader, out int aiTotalRows)
        {
            UserRolewisePhotoDetails oUserRolewisePhotoDetails = null;
            aiTotalRows = 0;
            if (aoSqlDataReader.Read())
            {
                aiTotalRows = Convert.ToInt32(aoSqlDataReader["TotalRows"]);
                aoSqlDataReader.NextResult();
                while (aoSqlDataReader.Read())
                {
                    oUserRolewisePhotoDetails = new UserRolewisePhotoDetails()
                    {
                        RowNo = Convert.ToInt32(aoSqlDataReader["RowNo"]),
                        UserRoleId = Convert.ToInt32(aoSqlDataReader["UserRoleId"]),
                        UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                        UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                        PhotoFilePath = Convert.ToString(aoSqlDataReader["PhotoFilePath"]),
                        BinaryPhotoImage = aoSqlDataReader["BinaryPhotoImage"] as Byte[],
                        ClassName = Convert.ToString(aoSqlDataReader["Class"]),
                        UserRoleName = Convert.ToString(aoSqlDataReader["UserRole"]),
                    };
                    molstUserRolewisePhotoDetails.Add(oUserRolewisePhotoDetails);
                }
            }
            return molstUserRolewisePhotoDetails;
        }

        /// <summary>
        /// This methos is used to count the User Rolewise Photo Upload details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public int CountUserForPhotoUplaod(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, string asUserName, bool abChkUserWithPhotoFlag, int aiStandardId, int aiDivisionId)
        {
            string sStudentFilter = CreateFilter(aiStandardId, aiDivisionId, asUserName, aiUserRoleId);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FilterWithPhoto", abChkUserWithPhotoFlag, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Filter", Utility.StringUtility.ReplaceSingleQuoteInString(asUserName, true), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentFilter", sStudentFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountUsersPhotoDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get student's Standard and Division filter.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asUserName"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asUserName, int aiUserRoleId)
        {
            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";
            }
            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            }
            if (aiUserRoleId == 3)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";
            return sFilter;
        }

        /// <summary>
        /// This method is used to get user role details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserRoleDetail()
        {
            string sQueryStmt = " SELECT User_Role_Id as UserRoleId, " +
                                " User_Role_Name as UserRoleName " +
                                " FROM User_Role_Master " +
                                " WHERE Is_Deleted = N'" + Constants.C_NO + "'" +
                                " AND User_Role_Id <> " + Convert.ToInt32(Constants.UserRoles.Parent);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQueryStmt);
        }

        /// <summary>
        /// This method is used to upload photo details as per the user role.
        /// </summary>
        /// <param name="aoArrayListUpdateStatements"></param>
        public void UploadTeacherPhoto(ArrayList aoArrayListUpdateStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListUpdateStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to get update sql statement.
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatementForPhotoUpload()
        {
            string sAdminQry;
            string sTeacherQry;
            string sStudentQry;
            string sAdminStaffQry;
            string sOtherStaffQry;
            string sTransportStaffQry;
            string sFilter = string.Empty;
            string sPhotoFilePath = moUserRolewisePhotoDetails.PhotoFilePath;
            if (moUserRolewisePhotoDetails.RemovePhoto == true)
            {
                sPhotoFilePath = string.Empty;
                sFilter = ", BinaryPhotoImage = NULL";
                if (moUserRolewisePhotoDetails.UserRoleId == 3)
                    sFilter = ", Photo_file_Path_Image = NULL";
            }
            sTeacherQry = " UPDATE SchoolWise_Teacher_Master SET " +
                   " Photo_file_Path = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(sPhotoFilePath, false) + "', ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                   sFilter +
                   " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                   " AND School_Id = " + miSchoolId +
                   " AND academic_Year_Id = " + miAcademicYearId;

            sAdminQry = " UPDATE User_Master " +
                   " SET PhotoFilePath = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(sPhotoFilePath, false) + "', ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                   sFilter +
                   " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                   " AND School_Id = " + miSchoolId;

            sStudentQry = " UPDATE SchoolWise_Student_Master " +
                    " SET Photo_file_Path = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(sPhotoFilePath, false) + "', ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                    sFilter +
                    " WHERE School_Id = " + miSchoolId +
                    " AND SchoolWise_Student_Id = " +
                                        " (SELECT Student_Id " +
                                        " FROM YearWise_Student_Details  " +
                                        " WHERE YearWise_Student_Id = " + moUserRolewisePhotoDetails.UserId +
                                        " AND Academic_Year_ID= " + miAcademicYearId + " AND School_Id=" + miSchoolId + " )";

            sAdminStaffQry = " UPDATE SchoolWise_Supervisor_Master " +
                    " SET PhotoFilePath = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(sPhotoFilePath, false) + "', ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                    sFilter +
                    " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                    " AND School_Id = " + miSchoolId;
            //" AND Academic_Year_Id = " + miAcademicYearId;

            sOtherStaffQry = " UPDATE OtherStaff " +
                    " SET PhotoFilePath = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(sPhotoFilePath, false) + "', ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                    sFilter +
                    " WHERE UserId = " + moUserRolewisePhotoDetails.UserId +
                    " AND SchoolId = " + miSchoolId;


            sTransportStaffQry = " UPDATE Transport.TransportStaffMaster " +
                   " SET PhotoFilePath = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(sPhotoFilePath, false) + "'" +
                   sFilter +
                   " WHERE UserId = " + moUserRolewisePhotoDetails.UserId +
                   " AND SchoolId = " + miSchoolId +
                   " AND Academic_Year_Id = " + miAcademicYearId;

            if (moUserRolewisePhotoDetails.RemovePhoto == false)
                UpdatePhotoBinaryFormat();

            if (moUserRolewisePhotoDetails.UserRoleId == 1)
                return sAdminQry;
            if (moUserRolewisePhotoDetails.UserRoleId == 2)
                return sTeacherQry;
            if (moUserRolewisePhotoDetails.UserRoleId == 3)
                return sStudentQry;
            if (moUserRolewisePhotoDetails.UserRoleId == 6)
                return sAdminStaffQry;
            if (moUserRolewisePhotoDetails.UserRoleId == 7)
                return sOtherStaffQry;
            if (moUserRolewisePhotoDetails.UserRoleId == 8)
                return sTransportStaffQry;
            else
                return (sAdminQry + " " + sTeacherQry + " " + sAdminStaffQry + " " + sStudentQry + " " + sOtherStaffQry + " " + sTransportStaffQry);
        }

        /// <summary>
        /// This method is used to update binary format of image in the database.
        /// </summary>
        public void UpdatePhotoBinaryFormat()
        {
            string sAdminQry;
            string sTeacherQry;
            string sStudentQry;
            string sAdminStaffQry;
            string sOtherStaffQry;
            string sTransportStaffQry;
            if (moUserRolewisePhotoDetails.UserRoleId == 2)
            {
                sTeacherQry = " UPDATE SchoolWise_Teacher_Master SET " +
                      " BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                      " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                      " AND School_Id = " + miSchoolId +
                      " AND academic_Year_Id = " + miAcademicYearId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sTeacherQry);

            }

            if (moUserRolewisePhotoDetails.UserRoleId == 1)
            {
                sAdminQry = " UPDATE User_Master " +
                       " SET BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                       " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                       " AND School_Id = " + miSchoolId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sAdminQry);

            }

            if (moUserRolewisePhotoDetails.UserRoleId == 3)
            {
                sStudentQry = " UPDATE SchoolWise_Student_Master " +
                      " SET Photo_file_Path_Image = @image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                      " WHERE School_Id = " + miSchoolId +
                      " AND SchoolWise_Student_Id = " +
                                            " (SELECT Student_Id " +
                                            " FROM YearWise_Student_Details  " +
                                            " WHERE YearWise_Student_Id = " + moUserRolewisePhotoDetails.UserId +
                                            " AND Academic_Year_ID= " + miAcademicYearId + " AND School_Id=" + miSchoolId + " )";
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sStudentQry);

            }

            if (moUserRolewisePhotoDetails.UserRoleId == 6)
            {
                sAdminStaffQry = " UPDATE SchoolWise_Supervisor_Master " +
                    " SET BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                    " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                    " AND School_Id = " + miSchoolId;
                //" AND Academic_Year_Id = " + miAcademicYearId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sAdminStaffQry);

            }

            if (moUserRolewisePhotoDetails.UserRoleId == 7)
            {
                sOtherStaffQry = " UPDATE OtherStaff " +
                   " SET BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                   " WHERE UserId = " + moUserRolewisePhotoDetails.UserId +
                   " AND SchoolId = " + miSchoolId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sOtherStaffQry);

            }

            if (moUserRolewisePhotoDetails.UserRoleId == 8)
            {
                sTransportStaffQry = " UPDATE Transport.TransportStaffMaster " +
                   " SET BinaryPhotoImage = @Image " +
                   " WHERE UserId = " + moUserRolewisePhotoDetails.UserId +
                   " AND SchoolId = " + miSchoolId +
                   " AND Academic_Year_Id = " + miAcademicYearId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sTransportStaffQry);

            }
        }

        public void Save()                    
        {  
            string sQuery;

            if (moUserRolewisePhotoDetails.UserRoleId == 2) // teacher 
            {
                sQuery = " UPDATE SchoolWise_Teacher_Master SET " +
                      " BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                      " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                      " AND School_Id = " + miSchoolId +
                      " AND academic_Year_Id = " + miAcademicYearId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sQuery);
            }
            else if (moUserRolewisePhotoDetails.UserRoleId == 6) // Supervisor
            {
                sQuery = " UPDATE SchoolWise_Supervisor_Master " +
                    " SET BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                    " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                    " AND School_Id = " + miSchoolId;                
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sQuery);
            }
            else if (moUserRolewisePhotoDetails.UserRoleId == 1)
            {
                sQuery = " UPDATE User_Master " +
                       " SET BinaryPhotoImage = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                       " WHERE User_Id = " + moUserRolewisePhotoDetails.UserId +
                       " AND School_Id = " + miSchoolId;
                using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                    oSQLServerUility.ExecuteTransaction(moUserRolewisePhotoDetails.BinaryPhotoImage, sQuery);
            }

            using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
            {
                oSQLServerUility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerUility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerUility.AddParameter("UserId", moUserRolewisePhotoDetails.UserId, SqlDbType.Int);
                oSQLServerUility.ExecuteStoredProcedureOnServer("Usp_SubmitTeacherPhoto");
            }
        }

        public DataTable GetSubmitStatus(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("Usp_GetSubmitStatus");
            }
        }

        #endregion "Public Methods
    }
}