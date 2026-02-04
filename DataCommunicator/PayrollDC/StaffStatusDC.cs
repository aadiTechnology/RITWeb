
/* 
   Created By       :- Vinod  
   Created Date     :- 12-Sept-2011
   Class Description:- This class is used to manage staff status details.
*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using Utility;
using PayrollEntities;

namespace DataCommunicator
{
    public class StaffStatusDC
    {
        #region Members

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;

        public List<StaffStatusDetails> molstStaffStatusDetails = new List<StaffStatusDetails>();

        #endregion

        #region Constructurs

        public StaffStatusDC()
        {
        }

        public StaffStatusDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Properties

        public List<StaffStatusDetails> lstStaffStatusDetails
        {
            get { return molstStaffStatusDetails; }
            set { molstStaffStatusDetails = value; }
        }

        #endregion               

        #region Methods

        /// <summary>
        /// This method used to get Staff Statys type details.
        /// </summary>
        /// <returns></returns>
        public List<StaffStatusDetails> GetStaffStatusTypes()
        {
           
            string sStamtement = "SELECT StaffStatusId as StatusId,StatusName FROM StaffStatusMaster WHERE Is_Deleted = N'N' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStamtement))
                return SetStaffStatusType(oSqlDataReader);
            }
           
        }

        private List<StaffStatusDetails> SetStaffStatusType(SqlDataReader aoSqlDataReader)
        {
            StaffStatusDetails oStaffStatusDetails = null;
            while (aoSqlDataReader.Read())
            {
                oStaffStatusDetails = new StaffStatusDetails()
                {
                    StatusId = Convert.ToInt32(aoSqlDataReader["StatusId"]),
                    StatusName = Convert.ToString(aoSqlDataReader["StatusName"]),
                };
                molstStaffStatusDetails.Add(oStaffStatusDetails);
            }
            return molstStaffStatusDetails;
        }

        /// <summary>
        /// This method used to get Staff status details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="asStatusType"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        /// 
        public List<StaffStatusDetails> GetStaffStatusDetails(int aiSchoolId, int aiAcademicYearId, String sortExpression, int iEndIndex, int iStartIndex, int aiUserRoleId, string asStatusType, string asFilter, bool asLocked) //asLocked filter added
        {
            string sFilter = asFilter == null ? "" : asFilter.Trim();
            string sSortExpression = (sortExpression != string.Empty && sortExpression != null) ? " ORDER BY " + sortExpression : " ORDER BY DesignationId";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StatusType", asStatusType, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sortExpression", sSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", iStartIndex, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ShowAll", asLocked, SqlDbType.Bit);   
               using( SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStaffStatusDetails"))
                return SetStaffstatusDetails(oSqlDataReader);
            }            
        }


        /// <summary>
        /// This method is used to get count of total staff status records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static int CountTotalStaffStatusDetails(int aiSchoolId, int aiAcademicYearId, int maximumRows, int iStartRowIndex, int aiUserRoleId, string asStatusType, string asFilter, bool asLocked)
        {
            string sFilter = asFilter == null ? "" : asFilter.Trim();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StatusType", asStatusType, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("maximumRows", maximumRows, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartRowIndex", iStartRowIndex, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.AddParameter("ShowAll", asLocked, SqlDbType.Bit);    //
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountTotalStaffStatusDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }  

        /// <summary>
        /// This method used to fill Staff status detail list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StaffStatusDetails> SetStaffstatusDetails(SqlDataReader aoSqlDataReader)
        {
            StaffStatusDetails oStaffStatusDetails = null;
            while (aoSqlDataReader.Read())
            {
                oStaffStatusDetails = new StaffStatusDetails()
                {
                    StaffStatusDetailsId = Convert.ToInt32(aoSqlDataReader["StaffStatusDetailsId"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),    
                    StatusId = Convert.ToInt32(aoSqlDataReader["StatusId"]),
                    DesignationId = Convert.ToInt32(aoSqlDataReader["DesignationId"]),
                    UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                    DesignationName = Convert.ToString(aoSqlDataReader["DesignationName"]),
                    IsDeleted = Convert.ToString(aoSqlDataReader["IsDeleted"]),
                    IsLocked = Convert.ToString(aoSqlDataReader["IsLocked"]),
                };
                molstStaffStatusDetails.Add(oStaffStatusDetails);
            }
            return molstStaffStatusDetails;
        }

        /// <summary>
        /// This method used to save and update Staff status details.
        /// </summary>
        /// <returns></returns>
        public void SaveStaffStatusDetails(string asStaffStatusDetailsXML, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffStatusDetailsXML", asStaffStatusDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStaffStatusDetails");
            }
        }

        /// <summary>
        /// This method used to get Staff working status details.
        /// </summary>
        /// <returns></returns>
        public List<StaffWorkingStatus> GetStaffWorkingStatus()
        {
            List<StaffWorkingStatus> olstStaffWorkingStatus = new List<StaffWorkingStatus>();
            string sStamtement = "SELECT StatusId,WorkingStatus FROM TeacherWorkingStatus WHERE IsDeleted = 0 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStamtement))
                    while (oSqlDataReader.Read())
                    {
                        StaffWorkingStatus oStaffWorkingStatus = new StaffWorkingStatus();
                        oStaffWorkingStatus.StatusId = Convert.ToInt32(oSqlDataReader["StatusId"]);
                        oStaffWorkingStatus.WorkingStatus = Convert.ToString(oSqlDataReader["WorkingStatus"]);
                        olstStaffWorkingStatus.Add(oStaffWorkingStatus);
                    }
                return olstStaffWorkingStatus;   
            }

        }

        #endregion
    }
}
