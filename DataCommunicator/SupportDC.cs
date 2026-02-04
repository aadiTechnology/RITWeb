// -----------------------------------------------------------------------
// File Name : SupportDC.cs
// Creator : Ashish Soanwane
// Created Date : 21-Oct-2013
// -----------------------------------------------------------------------

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using SchoolEntities;

    /// <summary>
    /// TODO: This class use to communicate with database to save & get support details .
    /// </summary>
    public class SupportDC
    {
        #region -- MEMBER(s) --
        
        private int miSchoolId;
        private int miAcademicYearId;

        #endregion -- MEMBER(s) --

        #region -- CONSTRUCTOR(s) --
        
        public SupportDC(int aiSchoolId, int aiAcademicYearId)
        { 
            this.miSchoolId=aiSchoolId;
            this.miAcademicYearId=aiAcademicYearId;
        }

        #endregion -- CONSTRUCTOR(s) --

        #region -- PUBLIC METHOD(s) --
        
        
        /// <summary>
        /// This method return list that contain support details as object
        /// </summary>
        /// <returns> List<SupportDetails> </returns>
        public List<SupportDetails> GetAll()
        {
            List<SupportDetails> lstSupportDetails = new List<SupportDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSupportDetails"))
                { 
                    while (oSqlDataReader.Read())
                        lstSupportDetails.Add(ReadObjectFromReader(oSqlDataReader));
                }
             }
            return lstSupportDetails;
        }
        
        /// <summary>
        /// This method use to save Support Details information
        /// </summary>
        /// <param name="aoSupportDetails"></param>
        public void Save(SupportDetails aoSupportDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EmailAddress", aoSupportDetails.EmailAddress, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MobileNo", aoSupportDetails.MobileNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Subject", aoSupportDetails.Subject, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Description", aoSupportDetails.Description, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserId", aoSupportDetails.UserId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FileName", aoSupportDetails.FileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSupportDetails");
             }
        
        }

        /// <summary>
        /// This method return SupportDetails class object for selected support id
        /// </summary>
        /// <param name="aiSupportId"></param>
        /// <returns> SupportDetails </returns>
        public SupportDetails Get(int aiSupportId)
        {
            SupportDetails oSupportDetails = new SupportDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SupportId", aiSupportId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSupportDetails"))
                {
                    if (oSqlDataReader.Read())
                        oSupportDetails=ReadObjectFromReader(oSqlDataReader);	
                }
            }
            return oSupportDetails;
        }

        /// <summary>
        /// This method return Name with Enrolment Number or Designation of User.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns> sName </returns>
        public string GetStudentDetails(int aiUserId, int aiUserRoleId)
        {
            string sName = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {                
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetNameForSupport"))
                {
                    if (oSqlDataReader.Read())
                    {
                        sName =oSqlDataReader["Name"].ToString();
                    }
                }                
            }
            return sName;
        }
 
        #endregion -- PUBLIC METHOD(s) --

        #region -- PRIVATE METHOD(s) --
     
        /// <summary>
        /// This method is used to populate object of support details config.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private SupportDetails ReadObjectFromReader(SqlDataReader aoSqlDataReader)
        {
            SupportDetails oRetirementNoticeConfig = new SupportDetails
            {
                Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                Subject = Convert.ToString(aoSqlDataReader["Subject"]),
                MobileNo = aoSqlDataReader["MobileNo"].ToString(),
                FileName = Convert.ToString(aoSqlDataReader["FileName"]),
                EmailAddress = Convert.ToString(aoSqlDataReader["EmailAddress"]),
                Description = Convert.ToString(aoSqlDataReader["Description"]),
                UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                UserRole = Convert.ToString(aoSqlDataReader["UserRole"])

            };
            return oRetirementNoticeConfig;
        }

        #endregion -- PRIVATE METHOD(s) --
    }
}
