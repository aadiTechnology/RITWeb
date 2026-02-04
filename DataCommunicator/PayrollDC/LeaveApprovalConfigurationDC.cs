using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Payroll;
using Utility;
namespace DataCommunicator.PayrollDC
{
  public  class LeaveApprovalConfigurationDC
    {      
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        
        #endregion
      
        #region Constructor(s)

        public LeaveApprovalConfigurationDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
        }

        public LeaveApprovalConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }
      
        public LeaveApprovalConfigurationDC()
        {          
        }
 
        #endregion

        #region Public Method(s)

        public void Save(LeaveApprovalConfiguration oLeaveApprovalConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", oLeaveApprovalConfig.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", oLeaveApprovalConfig.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFinalApprover", oLeaveApprovalConfig.IsFinalApprover, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ApprovalSortOrder", oLeaveApprovalConfig.ApproverSortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", oLeaveApprovalConfig.ReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Usp_SaveLeaveApprovalConfig");
            }
        }

        public List<LeaveApprovalConfiguration> GetAll(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLeaveApprovalConfiguration"))
                    return this.FillReportingDetails(oSqlDataReader);
            }
        }

        public void Submit(int aiUserId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitLeaveApprovalConfig");
            }
        }

        public DataTable GetStatus(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("Usp_GetLeaveSubmitStatus");
            }
        }

        public LeaveApprovalConfiguration Get(int iParameterId)
        {
            LeaveApprovalConfiguration oPerformanceReportingConfig = new LeaveApprovalConfiguration();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserDetailsId", iParameterId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeaveDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oPerformanceReportingConfig.ReportingUserId = oSqlDataReader["ReportingUserId"].ToInt();
                        oPerformanceReportingConfig.IsFinalApprover = oSqlDataReader["IsFinalApprover"].ToBool();
                        oPerformanceReportingConfig.UserName = oSqlDataReader["UserName"].ToString();
                        oPerformanceReportingConfig.ApproverSortOrder = oSqlDataReader["ApproverSortOrder"].ToInt();
                        oPerformanceReportingConfig.UserRoleId = oSqlDataReader["UserRoleId"].ToInt();
                    }
                    return oPerformanceReportingConfig;
                }
            }
        }

        public void Delete(int aiConfigId)
        {
            LeaveApprovalConfiguration oPerformanceReportingConfig = new LeaveApprovalConfiguration();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteLeaveApproval");
            }
        }

        #endregion
      
        #region Private Method(s)

        /// <summary>
        /// This method is used to fill the reporting configuration details into a list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<LeaveApprovalConfiguration> FillReportingDetails(SqlDataReader aoSqlDataReader)
        {
            List<LeaveApprovalConfiguration> lstPerformanceReportingConfig = new List<LeaveApprovalConfiguration>();
            while (aoSqlDataReader.Read())
            {
                lstPerformanceReportingConfig.Add(new LeaveApprovalConfiguration
                {
                    UserName = aoSqlDataReader["UserName"].ToString(),
                    ReportingUserId = aoSqlDataReader["ReportingUserId"].ToInt(),
                    IsSubmitted = aoSqlDataReader["IsSubmitted"].ToBool(),
                    IsFinalApprover = aoSqlDataReader["IsFinalApprover"].ToBool(),
                    Id = aoSqlDataReader["Id"].ToInt(),
                    UserRoleId = aoSqlDataReader["UserRoleId"].ToInt(),
                    ApproverSortOrder = aoSqlDataReader["ApproverSortOrder"].ToInt()
                });
            }

            return lstPerformanceReportingConfig;
        }
      
        #endregion
    }
}
