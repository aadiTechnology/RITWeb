/*File Name - ReportingConfigurationDC.cs
 * Created By - Pravin Shinde
 * Created Date - 25 Sept 2013
 * Description - This class is used to manage performance reporting configuration.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaffPerformanceEntity;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{   
    public class ReportingConfigurationDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region Constructor(s)

        public ReportingConfigurationDC(int aiSchoolId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;            
            this.miUpdatedById = aiUpdatedById;
        }

        public ReportingConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
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
        public List<PerformanceReportingConfig> GetAll(int aiUserId,int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader =  oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllReportingConfigs"))
                    return this.FillReportingDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get all the reporting configuration for selected reporting user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public PerformanceReportingConfig Get(int aiUserId, int aiYear,int aiReportingConfigId)
        {
            PerformanceReportingConfig oPerformanceReportingConfig = new PerformanceReportingConfig();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfigId", aiReportingConfigId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllReportingConfigs"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oPerformanceReportingConfig.RoleId = oSqlDataReader["RoleId"].ToInt();
                        oPerformanceReportingConfig.ReportingUserId = oSqlDataReader["ReportingUserId"].ToInt();
                        oPerformanceReportingConfig.IsFinalApprover = oSqlDataReader["IsFinalApprover"].ToBool();
                        oPerformanceReportingConfig.IsSupervisor = oSqlDataReader["IsSupervisor"].ToBool();
                        oPerformanceReportingConfig.ApprovalSortOrder = oSqlDataReader["ApprovalSortOrder"].ToInt();
                    }
                    return oPerformanceReportingConfig;
                }
            }
        }

        /// <summary>
        /// This method is used to save the reporting configuration for selected user.
        /// </summary>
        /// <param name="aoPerformanceReportingConfig"></param>
        public void Save(PerformanceReportingConfig aoPerformanceReportingConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aoPerformanceReportingConfig.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aoPerformanceReportingConfig.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aoPerformanceReportingConfig.ReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfigId", aoPerformanceReportingConfig.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFinalApprover", aoPerformanceReportingConfig.IsFinalApprover, SqlDbType.Bit);
                //oSQLServerDbUtility.AddParameter("IsSupervisor", aoPerformanceReportingConfig.IsSupervisor, SqlDbType.Bit);       \
                oSQLServerDbUtility.AddParameter("ApprovalSortOrder", aoPerformanceReportingConfig.ApprovalSortOrder, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveReportingConfig");
            }
        }

        /// <summary>
        /// This method is used to delete the reporting configuation for selected user.
        /// </summary>
        /// <param name="aiYear"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingConfigId"></param>
        public void Delete(int aiYear, int aiUserId, int aiReportingConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfigId", aiReportingConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingConfiguratioScreenId", Constants.SchoolConfigurations.ReportingConfiguration.ToInt(), SqlDbType.Int);                                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteReportingConfig");
            }
        }

        /// <summary>
        /// This method is used to submit all the configuration to database.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="abIsSubmit"></param>
        public void SubmitUnsubmitConfig(int aiUserId, int aiYear, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitReportingConfig");
            }
        }

        /// <summary>
        /// This method is used to retrieve staff reporting config for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public List<PerformanceReportingConfig> GetAllReportingConfigs(int aiUserID, int aiYear, bool abShowPending)
        {
            List<PerformanceReportingConfig> lstStaffReportingConfig = new List<PerformanceReportingConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserID", aiUserID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowPending", abShowPending, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUsersReportingToGivenUser"))
                {
                    while (oSqlDataReader.Read())
                    {
                        PerformanceReportingConfig oReportingConfig = new PerformanceReportingConfig
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            IsSupervisor = Convert.ToBoolean(oSqlDataReader["IsSupervisor"])
                        };
                        lstStaffReportingConfig.Add(oReportingConfig);
                    }
                }
            }
            return lstStaffReportingConfig;
        }
        
        /// <summary>
        /// This method is used to retrieve invitee of for current user.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public  List<PerformanceReportingConfig> GetAllInviteeOfGivenUser(int aiUserID, int aiYear)
        {
            List<PerformanceReportingConfig> lstInvitee = new List<PerformanceReportingConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserID", aiUserID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllInviteeOfGivenUser"))
                {
                    while (oSqlDataReader.Read())
                    {
                        PerformanceReportingConfig oReportingConfig = new PerformanceReportingConfig
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]),
                        };
                        lstInvitee.Add(oReportingConfig);
                    }
                }
            }
            return lstInvitee;
        }

        /// <summary>
        /// This method is used to send request to invitee.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SendRequestToInvitee(string asInviteeUserIds, int aiReportingUserId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("InviteeUserIds", asInviteeUserIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserID", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SendRequestToInvitee");
            }
        }
    
        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill the reporting configuration details into a list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<PerformanceReportingConfig> FillReportingDetails(SqlDataReader aoSqlDataReader)
        {
            List<PerformanceReportingConfig> lstPerformanceReportingConfig = new List<PerformanceReportingConfig>();
            while (aoSqlDataReader.Read())
            {
                lstPerformanceReportingConfig.Add(new PerformanceReportingConfig
                {
                    Id = aoSqlDataReader["ReportingConfigId"].ToInt(),
                    ReportingUserId = aoSqlDataReader["ReportingUserId"].ToInt(),
                    UserName = aoSqlDataReader["Name"].ToString(),
                    IsSubmitted = aoSqlDataReader["IsSubmitted"].ToBool(),
                    IsFinalApprover = aoSqlDataReader["IsFinalApprover"].ToBool(),
                    IsSupervisor = aoSqlDataReader["IsSupervisor"].ToBool(),
					IsPublished = aoSqlDataReader["IsPublished"].ToBool(),
                    ApprovalSortOrder = aoSqlDataReader["ApprovalSortOrder"].ToInt()
                });
            }

            return lstPerformanceReportingConfig;
        }    
        
        #endregion

        public List<ReportingStaff> GetAllUsers(int aiUserId, int aiUserRoleId, int aiYear, Constants.ReportingUserScreen aoReportingUserScreen)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserScreenId", aoReportingUserScreen.ToInt(), SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUsersToCopyConfig"))
                {
                    List<ReportingStaff> lstUsers = new List<ReportingStaff>();
                    while (oSqlDataReader.Read())
                        lstUsers.Add(new ReportingStaff { ReportingUserId = Convert.ToInt32(oSqlDataReader["UserId"]), Name = Convert.ToString(oSqlDataReader["UserName"]) });
                    return lstUsers;
                }
            }
        }

        public void Copy(int aiUserId, int aiUserRoleId, int aiYear, string asSelectedUserIds, Constants.ReportingUserScreen aoReportingUserScreen)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SelectedUserIds", asSelectedUserIds, SqlDbType.NVarChar);

                if (aoReportingUserScreen == Constants.ReportingUserScreen.StaffPerformanceEval)
                {
                    oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyReportingStaffConfiguration");
                }
                else if (aoReportingUserScreen == Constants.ReportingUserScreen.LessonPlan)
                {
                    oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);                    
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyLessonPlanReportingConfig");
                }
                else if (aoReportingUserScreen == Constants.ReportingUserScreen.UserLeaveApprovalConfiguration)
                {
                    oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyLeaveApprovalConfiguration");
                }
            }
        }
    }
}
