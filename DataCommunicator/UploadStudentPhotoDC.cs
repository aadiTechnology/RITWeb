// File Name - UploasStudentPhotoDC.cs
// Create By - Vishakha
// Created Date - 01 dec 2022

using System;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database for insert/delete/submit student photo.
    /// </summary>
    public class UploadStudentPhotoDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miInsertedById;
        private int miAcademicYearId;

        #endregion

        #region Constants & Structure

        public string sPhotoFilePath;
        public bool bIsSubmitted;
        public Byte[] sPhotoFilePathInBinary;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UploadStudentPhotoDC()
        {
        }

        /// <summary>
        /// Initialise member variable.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiInsertedById"></param>
        public UploadStudentPhotoDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miInsertedById = aiInsertedById;
            this.miAcademicYearId = aiAcademicYearId;
        } 

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to save student photo.
        /// </summary>
        /// <param name="aoSavePhotoFile"></param>
        public void Save(SavePhotoFile aoSavePhotoFile)
        {
            int iStudentId = 0;
            using (SQLServerDbUtility oSQLServerDBUtility = new SQLServerDbUtility())
            {
                oSQLServerDBUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDBUtility.AddParameter("StudentId", aoSavePhotoFile.StudentId, SqlDbType.Int);
                oSQLServerDBUtility.AddParameter("InsertedById", miInsertedById, SqlDbType.Int);

               DataTable dt = oSQLServerDBUtility.ExecuteStoredProcedureAndGetDataTable("usp_SaveStudentPhoto");
               if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                   iStudentId = dt.Rows[0][0].ToInt();
            }
            string sSQL = "UPDATE StudentPhotoSubmitStatus SET PhotoImage = @Image" +
                  " WHERE SchoolId = " + miSchoolId +
                  " AND StudentId = " + iStudentId +
                  " AND IsDeleted = 0";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(aoSavePhotoFile.PhotoFilePathInBinary, sSQL);
        }

        /// <summary>
        /// This method is used to delete student photo.
        /// </summary>
        /// <param name="aiStudentId"></param>
        public void Delete(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteStudentPhoto");
            }
        }

        /// <summary>
        /// This method is used to submit student photo.
        /// </summary>
        /// <param name="aiStudentId"></param>
        public void Submit(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentPhoto");
            }
        }

        /// <summary>
        /// This method is used to get student name for label.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <returns></returns>
        public StudentPhotoUploadDetails GetStudentPhotoUploadDetails(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentPhotoUploadDetails oGetStudentNameForLabel = new StudentPhotoUploadDetails();
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentNameToPhotoUpload"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oGetStudentNameForLabel.StudentName = oSqlDataReader["StudentName"].ToString();
                        oGetStudentNameForLabel.IsSaved = oSqlDataReader["IsSaved"].ToBool();
                        oGetStudentNameForLabel.IsSubmitted = oSqlDataReader["IsSubmitted"].ToBool();
                        oGetStudentNameForLabel.PhotoImage = oSqlDataReader["PhotoImage"] as byte[];
                        oGetStudentNameForLabel.IsOldPhotoExist = oSqlDataReader["IsOldPhotoExist"].ToBool();
                        oGetStudentNameForLabel.SchoolwiseStudentId = oSqlDataReader["SchoolwiseStudentId"].ToInt();
                    }
                }

                return oGetStudentNameForLabel;
            }
        }

       #endregion
    }
}
