using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LessonPlanEntities;
using System.Data.SqlClient;
using StaffPerformanceEntity;
using Utility;
using System.Data;

namespace DataCommunicator
{
    public class LessonPlanReportingConfigDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public LessonPlanReportingConfigDC()
        {
        }

        public LessonPlanReportingConfigDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get all the reporting configuration for selected user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<LessonPlanReportingConfig> GetAll(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLessonPlanReportingConfigs"))
                    return this.FillReportingDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get all the reporting configuration for selected reporting user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public LessonPlanReportingConfig Get(int aiUserId, int aiReportingConfigId)
        {
            LessonPlanReportingConfig moLessonPlanReportingConfig = new LessonPlanReportingConfig();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfigId", aiReportingConfigId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLessonPlanReportingConfigs"))
                {
                    if (oSqlDataReader.Read())
                    {
                        moLessonPlanReportingConfig.ReportingUserId = oSqlDataReader["ReportingUserId"].ToInt();
                        moLessonPlanReportingConfig.IsFinalApprover = oSqlDataReader["IsFinalApprover"].ToBool();
                        moLessonPlanReportingConfig.ApprovalSortOrder = oSqlDataReader["ApprovalSortOrder"].ToInt();
                        moLessonPlanReportingConfig.IsSubmitted = oSqlDataReader["IsSubmitted"].ToBool();
                    }
                    return moLessonPlanReportingConfig;
                }
            }
        }

        /// <summary>
        /// This method is used to save the reporting configuration for selected user.
        /// </summary>
        /// <param name="aoLessonPlanReportingConfig"></param>
        public void Save(LessonPlanReportingConfig aoLessonPlanReportingConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aoLessonPlanReportingConfig.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aoLessonPlanReportingConfig.ReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfigId", aoLessonPlanReportingConfig.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFinalApprover", aoLessonPlanReportingConfig.IsFinalApprover, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ApprovalSortOrder", aoLessonPlanReportingConfig.ApprovalSortOrder, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveLessonPlanReportingConfig");
            }
        }

        /// <summary>
        /// This method is used to delete the reporting configuation for selected user.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingConfigId"></param>
        public void Delete(int aiUserId, int aiReportingConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYEarId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfigId", aiReportingConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfiguratioScreenId", Constants.SchoolConfigurations.LessonPlanReportingUserConfig.ToInt(), SqlDbType.Int);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteLessonPlanReportingConfig");
            }
        }

        /// <summary>
        /// This method is used to submit all the configuration to database.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="abIsSubmit"></param>
        public void SubmitUnsubmitConfig(int aiUserId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitLessonPlanReportingConfig");
            }
        }

        ///// <summary>
        ///// This method is used to retrieve staff reporting config for current user.
        ///// </summary>
        ///// <param name="aiSchoolId"></param>
        //public List<PerformanceReportingConfig> GetAllReportingConfigs(int aiUserID, int aiYear)
        //{
        //    List<PerformanceReportingConfig> lstStaffReportingConfig = new List<PerformanceReportingConfig>();
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("UserID", aiUserID, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
        //        using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUsersReportingToGivenUser"))
        //        {
        //            while (oSqlDataReader.Read())
        //            {
        //                PerformanceReportingConfig oReportingConfig = new PerformanceReportingConfig
        //                {
        //                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
        //                    UserName = Convert.ToString(oSqlDataReader["UserName"]),
        //                    IsSupervisor = Convert.ToBoolean(oSqlDataReader["IsSupervisor"]),
        //                };
        //                lstStaffReportingConfig.Add(oReportingConfig);
        //            }
        //        }
        //    }
        //    return lstStaffReportingConfig;
        //}

        /// <summary>
        /// This method is used to retrieve invitee of for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        //public List<PerformanceReportingConfig> GetAllInviteeOfGivenUser(int aiUserID, int aiYear)
        //{
        //    List<PerformanceReportingConfig> lstInvitee = new List<PerformanceReportingConfig>();
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("UserID", aiUserID, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
        //        using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllInviteeOfGivenUser"))
        //        {
        //            while (oSqlDataReader.Read())
        //            {
        //                PerformanceReportingConfig oReportingConfig = new PerformanceReportingConfig
        //                {
        //                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
        //                    UserName = Convert.ToString(oSqlDataReader["UserName"]),
        //                    IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]),
        //                };
        //                lstInvitee.Add(oReportingConfig);
        //            }
        //        }
        //    }
        //    return lstInvitee;
        //}

        ///// <summary>
        ///// This method is used to send request to invitee.
        ///// </summary>
        ///// <param name="aoIncomeTaxSlab"></param>
        //public void SendRequestToInvitee(string asInviteeUserIds, int aiReportingUserId, int aiYear)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("InviteeUserIds", asInviteeUserIds, SqlDbType.NVarChar);
        //        oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("UserID", aiReportingUserId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
        //        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SendRequestToInvitee");
        //    }
        //}

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill the reporting configuration details into a list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<LessonPlanReportingConfig> FillReportingDetails(SqlDataReader aoSqlDataReader)
        {
            List<LessonPlanReportingConfig> lstLessonPlanReportingConfig = new List<LessonPlanReportingConfig>();
            while (aoSqlDataReader.Read())
            {
                lstLessonPlanReportingConfig.Add(new LessonPlanReportingConfig
                {
                    Id = aoSqlDataReader["ReportingConfigId"].ToInt(),
                    ReportingUserId = aoSqlDataReader["ReportingUserId"].ToInt(),
                    UserName = aoSqlDataReader["Name"].ToString(),
                    IsSubmitted = aoSqlDataReader["IsSubmitted"].ToBool(),
                    IsFinalApprover = aoSqlDataReader["IsFinalApprover"].ToBool(),
                    IsPublished = aoSqlDataReader["IsPublished"].ToBool(),
                    ApprovalSortOrder = aoSqlDataReader["ApprovalSortOrder"].ToInt()
                });
            }

            return lstLessonPlanReportingConfig;
        }

        #endregion

        //public List<ReportingStaff> GetAllUsers(int aiUserId, int aiUserRoleId)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Year", this.miAcademicYearId, SqlDbType.Int);
        //        SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUsersToCopyConfig");
        //        List<ReportingStaff> lstUsers = new List<ReportingStaff>();
        //        while (oSqlDataReader.Read())
        //            lstUsers.Add(new ReportingStaff { ReportingUserId = Convert.ToInt32(oSqlDataReader["UserId"]), Name = Convert.ToString(oSqlDataReader["UserName"]) });
        //        return lstUsers;
        //    }
        //}

        public void Copy(int aiUserId, int aiUserRoleId, int aiYear, string asSelectedUserIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SelectedUserIds", asSelectedUserIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyReportingStaffConfiguration");
            }
        }
    }
}
