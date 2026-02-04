using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LessonPlanEntities;
using StaffPerformanceEntity;
using Utility;

namespace DataCommunicator
{
    public class LessionPlanDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private List<LessonPlanParameters> mlstParameters = new List<LessonPlanParameters>();
        private List<LessonPlanConfig> mlstLessonPlanConfigs = new List<LessonPlanConfig>();
        private List<LessonPlanReportingConfig> mlstLessonPlanReportingConfig = new List<LessonPlanReportingConfig>();
        private List<LessonPlanDetails> mlstLessonPlanDetails = new List<LessonPlanDetails>();
        private LessonPlanBasicDetails moLessonPlanBasicDetails = new LessonPlanBasicDetails();
        private ButtonState moButtonState = new ButtonState();
        private LessonPlanStandardDivIds moLessonPlanStdDivIds = new LessonPlanStandardDivIds();
        private List<ApproverComment> mlstApproverComments = new List<ApproverComment>();
        private List<LessonPlanPhrase> mlstLessonPlanPhrases = new List<LessonPlanPhrase>();
        
        #endregion

        #region Constructor(s)

        public LessionPlanDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region Property(s)

        public List<LessonPlanParameters> Parameters
        {
            get
            {
                return this.mlstParameters;
            }
        }

        public List<LessonPlanConfig> PlanConfigs
        {
            get
            {
                return this.mlstLessonPlanConfigs;
            }
        }

        public List<LessonPlanReportingConfig> LessonPlanReportingUsers
        {
            get
            {
                return this.mlstLessonPlanReportingConfig;
            }
        }

        public List<LessonPlanDetails> LessonPlanDetails
        {
            get
            {
                return this.mlstLessonPlanDetails;
            }
        }

        public LessonPlanBasicDetails BasicDetails
        {
            get
            {
                return this.moLessonPlanBasicDetails;
            }
        }

        public ButtonState ButtonState
        {
            get
            {
                return this.moButtonState;
            }
        }

        public LessonPlanStandardDivIds LessonPlanStandard
        {
            get
            {
                return this.moLessonPlanStdDivIds;
            }
        }

        public List<ApproverComment> ApproverComments
        {
            get { return this.mlstApproverComments; }
        }

        public List<LessonPlanPhrase> LessonPlanPhrases
        {
            get { return this.mlstLessonPlanPhrases; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return lesson plan details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public static List<LessonPlanConfig> GetAllConfigs(int aiSchoolId, int aiAcademicYearId, int aiReportingUserId, int aiUserId, int aiStartIndex, int aiEndIndex, string StartDate = null, string EndDate = null)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsRecordCount", 0, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StartDate", StartDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EndDate", EndDate, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLessonPlanConfigDetails"))
                {
                    List<LessonPlanConfig> lstConfigs = new List<LessonPlanConfig>();
                    while (oSqlDataReader.Read())
                    {
                        lstConfigs.Add
                            (
                                new LessonPlanConfig
                                {
                                    StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
                                    EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]),
                                    IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]),
                                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                    Remarks = Convert.ToString(oSqlDataReader["Remarks"]),
                                    IsSuggestionAdded = Convert.ToBoolean(oSqlDataReader["IsSuggisionAdded"]),
                                    IsSuggestionRead = Convert.ToBoolean(oSqlDataReader["IsSuggisitionRead"]),
                                    IsSubmitedByReportingUser = Convert.ToBoolean(oSqlDataReader["SubmitedByReportingUser"]),
                                    //ParentParameterId=Convert.ToInt32(oSqlDataReader["ParentParameterId"])
                                }
                            );
                    }

                    return lstConfigs;
                }
            }
        }

        /// <summary>
        ///  This method is used to return lesson plan count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static int GetAllConfigsCount(int aiSchoolId, int aiAcademicYearId, int aiReportingUserId, int aiUserId, string StartDate = null, string EndDate = null)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);               
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsRecordCount", 1, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StartDate", StartDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EndDate", EndDate, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("RecordCount", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetLessonPlanConfigDetails");

                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to save lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="asLessonPlanXml"></param>
        /// <param name="adtOldStartDate"></param>
        /// <param name="adtOldEndDate"></param>
        public void Save(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, string asLessonPlanXml, DateTime adtOldStartDate, DateTime adtOldEndDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("LessonPlanXml", asLessonPlanXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                
                if (adtOldStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("OldStartDate", adtOldStartDate, SqlDbType.DateTime);
                
                if (adtOldEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("OldEndDate", adtOldEndDate, SqlDbType.DateTime);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveLessonPlanDetails");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        public void Submit(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate)
        {   
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitLessonPlanDetails");
            }
        }

        /// <summary>
        /// This method is used to return lesson plan details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiLessonPlanConfigId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSubjectId"></param>
        public void GetAll(int aiUserId, int aiReportingUserId, int aiLessonPlanConfigId, int aiStdDivId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LessonPlanConfigId", aiLessonPlanConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLassonPlanDetails"))
                {
                    this.FillParameters(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillConfigDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillBasicDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillReportingUsers(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillComments(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillButtonState(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to return lesson plan details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="abIsNewMode"></param>
        public void GetAll(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, bool abIsNewMode)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);

                if (adtStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);

                if (adtStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);

                oSQLServerDbUtility.AddParameter("IsNewMode", abIsNewMode, SqlDbType.Bit);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLessonPlanDetails"))
                {
                    this.FillParameters(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillConfigDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillBasicDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillReportingUsers(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillComments(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillApproverComment(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillButtonState(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillStandardDivIds(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillLessionPlanPhareses(oSqlDataReader);
                }
            }
        }

        private void FillLessionPlanPhareses(SqlDataReader aoSqlDataReader)
        {
            this.mlstLessonPlanPhrases.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstLessonPlanPhrases.Add
                    (
                        new LessonPlanPhrase
                        {
                            Title = Convert.ToString(aoSqlDataReader["Title"]),
                            IsPhrase = Convert.ToBoolean(aoSqlDataReader["IsPhrase"])                            
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to return all teachers.
        /// </summary>
        /// <returns></returns>
        public List<TeacherDetails> GetAllTeachers(string asFullAccess)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsFullAccess", asFullAccess, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllTeachersOfLessonPlan"))
                {
                    List<TeacherDetails> lstTeachers = new List<TeacherDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstTeachers.Add
                            (
                                new TeacherDetails
                                {
                                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                    Name = Convert.ToString(oSqlDataReader["UserName"])
                                }

                            );
                    }

                    return lstTeachers;
                }
            }
        }

        /// <summary>
        /// This method is used to return class subjects.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <returns></returns>
        public List<ClassSubjectDetails> GetAllClassSubjects(int aiUserId, int aiReportingUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClassSubjectOfLessonPlan"))
                {
                    List<ClassSubjectDetails> lstClasses = new List<ClassSubjectDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstClasses.Add
                            (
                                new ClassSubjectDetails
                                {
                                    StdDivId = Convert.ToInt32(oSqlDataReader["Standard_Division_Id"]),
                                    ClassName = Convert.ToString(oSqlDataReader["ClassName"]),
                                    SubjectId = Convert.ToInt32(oSqlDataReader["Subject_Id"]),
                                    SubjectName = Convert.ToString(oSqlDataReader["Subject_Name"])
                                }

                            );
                    }

                    return lstClasses;
                }
            }
        }

        /// <summary>
        /// This method is used to delete configuration.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        public void DeleteConfiguration(int aiUserId, DateTime adtStartDate, DateTime adtEndDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteLessonPlanConfig");
            }
        }

        /// <summary>
        /// This method is used to update the read suggesition status.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        public void UpdateReadSuggestion(int aiUserId, DateTime adtStartDate, DateTime adtEndDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateLessonPlanReadSuggestion");
            }
        }

        /// <summary>
        /// This method is used to approve lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiConfigId"></param>
        public void Approve(int aiUserId, int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ApproveLessonPlan");
            }
        }

        /// <summary>
        /// This method is used to reject lesson plan.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiConfigId"></param>
        /// <param name="asReason"></param>
        public void Reject(int aiUserId, int aiConfigId, string asReason)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Reason", asReason, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_RejectLessonPlan");
            }
        }

        /// <summary>
        /// This method is used to return last day of week.
        /// </summary>
        /// <returns></returns>
        public int GetLastDayOfWeek()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = "SELECT TOP 1 Original_WeekDays_Id" +
                                          " FROM " +
                                          "WeekDays_Master" +
                                          " WHERE " +
                                          " School_Id = " + this.miSchoolId +
                                          " AND Is_Deleted = 'N'" +
                                          " AND Academic_year_id = " + this.miAcademicYearId +
                                          " ORDER BY Original_WeekDays_Id DESC";
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            }
        }

        /// <summary>
        /// This method is used to return reporting user configuration.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<LessonPlanReportingConfig> GetAllReportingConfigs(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLessonPlanReportingConfigs"))
                    return FillReportingConfig(oSqlDataReader);                   
            }
        }

        /// <summary>
        /// This method is used to save comments saved by approver.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="asApproverComment"></param>
        public void SaveComment(int aiUserId, int aiReportingUserId, DateTime adtStartDate, DateTime adtEndDate, string asApproverComment, DateTime adtOldStartDate, DateTime adtOldEndDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ApproverComment", asApproverComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OldStartDate", adtOldStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("OldEndDate", adtOldEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveApproverComment");
            }
        }

        /// <summary>
        /// This method is used to Update Date updated by Full Access User.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="adtOldEndDate"></param>
        /// <param name="adtOldStartDate"></param>
        public void UpdateDate(int aiUserId, int aiReportingUserId, DateTime adStartDate, DateTime adEndDate, DateTime adOldStartDate, DateTime adOldEndDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDate", adEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OldStartDate", adOldStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("OldEndDate", adOldEndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateLessonPlanDate");
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill comment details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillComments(SqlDataReader aoSqlDataReader)
        {
            this.mlstLessonPlanDetails.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstLessonPlanDetails.Add
                    (
                        new LessonPlanDetails
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            ReportingUserId = Convert.ToInt32(aoSqlDataReader["ReportingUserId"]),
                            ParameterId = Convert.ToInt32(aoSqlDataReader["ParameterId"]),
                            Comment = Convert.ToString(aoSqlDataReader["Comment"]),
                            StdDivId = Convert.ToInt32(aoSqlDataReader["StdDivId"]),
                            SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]),
                            SubjectStartDate = (aoSqlDataReader["SubjectStartDate"] != DBNull.Value? Convert.ToDateTime(aoSqlDataReader["SubjectStartDate"]).ToString(Constants.S_DATE_FORMAT):string.Empty),
                            SubjectEndDate = (aoSqlDataReader["SubjectEndDate"]!= DBNull.Value? Convert.ToDateTime(aoSqlDataReader["SubjectEndDate"]).ToString(Constants.S_DATE_FORMAT):string.Empty)
                        }

                    );
            }
        }

        /// <summary>
        ///  This method is used to fill reporting users.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillReportingUsers(SqlDataReader aoSqlDataReader)
        {
            this.mlstLessonPlanReportingConfig.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstLessonPlanReportingConfig.Add
                    (
                        new LessonPlanReportingConfig
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                            UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                            ReportingUserId = Convert.ToInt32(aoSqlDataReader["ReportingUserId"]),
                            ReportingUserName = Convert.ToString(aoSqlDataReader["ReportingUserName"]),
                            IsFinalApprover = Convert.ToBoolean(aoSqlDataReader["IsFinalApprover"]),
                            ApprovalSortOrder = Convert.ToInt32(aoSqlDataReader["ApprovalSortOrder"])                          
                        }

                    );
            }
        }

        /// <summary>
        ///  This method is used to fill up configuration details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillConfigDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstLessonPlanConfigs.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstLessonPlanConfigs.Add
                    (
                        new LessonPlanConfig
                        {  
                            StdDivId = Convert.ToInt32(aoSqlDataReader["StdDivId"]),
                            SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]),
                            ClassName = Convert.ToString(aoSqlDataReader["ClassName"]),
                            SubjectName = Convert.ToString(aoSqlDataReader["Subject_Name"]),
                            LessonPlanCategoryId = Convert.ToInt32(aoSqlDataReader["LessonPlanCategoryId"]),
                            StandardId = Convert.ToInt32(aoSqlDataReader["Standard_Id"]),
                            SubjectCategoryId = Convert.ToInt32(aoSqlDataReader["SubjectCategoryId"])
                        }

                    );
            }
        }

        /// <summary>
        ///  This method is used to fill basic details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillBasicDetails(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moLessonPlanBasicDetails = new LessonPlanBasicDetails
                  {
                      ClassName = Convert.ToString(aoSqlDataReader["ClassName"]),
                      TeacherName = Convert.ToString(aoSqlDataReader["TeacherName"]),
                      SubjectName = Convert.ToString(aoSqlDataReader["SubjectName"])
                  };
            }
        }

        /// <summary>
        ///  This method is used to fill up parameters.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillParameters(SqlDataReader aoSqlDataReader)
        {
            this.mlstParameters.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstParameters.Add
                    (
                        new LessonPlanParameters
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Title = Convert.ToString(aoSqlDataReader["Title"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                            LessonPlanCategoryId = Convert.ToInt32(aoSqlDataReader["LessonPlanCategoryId"]),
                            SubjectCategoryId = Convert.ToInt32(aoSqlDataReader["SubjectCategoryId"]),
                            ParentParameterId = Convert.ToInt32(aoSqlDataReader["ParentParameterId"]),
                            ParentParameter = Convert.ToString(aoSqlDataReader["ParentParameter"]),
                          }
                    );
            }
        }

        /// <summary>
        ///  This method is used to fill approver comments.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillApproverComment(SqlDataReader aoSqlDataReader)
        {
            this.mlstApproverComments.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstApproverComments.Add
                    (
                        new ApproverComment
                        {
                            ReportingUserId = Convert.ToInt32(aoSqlDataReader["ReportingUserId"]),
                            Comment = Convert.ToString(aoSqlDataReader["Comment"]),
                            UpdateDate = Convert.ToDateTime(aoSqlDataReader["UpdateDate"]),
                            IsPublished = Convert.ToBoolean(aoSqlDataReader["IsPublished"]),
                            LessonPlanXMLId = Convert.ToInt32(aoSqlDataReader["LessonPlanXML"]),
                            IsReportingUser = Convert.ToBoolean(aoSqlDataReader["IsReportingUser"])
                        }

                    );
            }
        }

        /// <summary>
        ///  This method is used to set button state details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillButtonState(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moButtonState = new ButtonState
                {
                    EnableSaveButton = Convert.ToBoolean(aoSqlDataReader["EnableSaveButton"]),
                    EnableSubmitButton = Convert.ToBoolean(aoSqlDataReader["EnableSubmitButton"]),
                    EnableRejectButton = Convert.ToBoolean(aoSqlDataReader["EnableRejectButton"])
                };
            }
        }

        /// <summary>
        /// This method is used to fill StandardDiv Ids.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillStandardDivIds(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moLessonPlanStdDivIds = new LessonPlanStandardDivIds
                {
                    StandardDivisionIds = Convert.ToString(aoSqlDataReader["StandardDivisionIds"])
                };
            }
        }

        /// <summary>
        ///  This method is used to Fill Reporting User Details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private List<LessonPlanReportingConfig> FillReportingConfig(SqlDataReader aoSqlDataReader)
        {
            List<LessonPlanReportingConfig> olstLessonPlanConfig = new List<LessonPlanReportingConfig>();
            while (aoSqlDataReader.Read())
            {
                LessonPlanReportingConfig oLessonPlanReportingConfig = new LessonPlanReportingConfig();
                oLessonPlanReportingConfig.IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]);
                oLessonPlanReportingConfig.ReportingUserId = Convert.ToInt32(aoSqlDataReader["ReportingUserId"]);
                oLessonPlanReportingConfig.ReportingUserName = Convert.ToString(aoSqlDataReader["ReportingUserName"]);
                oLessonPlanReportingConfig.ApprovalSortOrder = Convert.ToInt32(aoSqlDataReader["ApprovalSortOrder"]);
                oLessonPlanReportingConfig.StartDate = Convert.ToDateTime(aoSqlDataReader["StartDate"]);
                oLessonPlanReportingConfig.EndDate = Convert.ToDateTime(aoSqlDataReader["EndDate"]);
                if (aoSqlDataReader["MinDate"] != DBNull.Value)
                    oLessonPlanReportingConfig.MinDate = Convert.ToDateTime(aoSqlDataReader["MinDate"]);
                else
                    oLessonPlanReportingConfig.MinDate = DateTime.MinValue;
                if (aoSqlDataReader["MaxDate"] != DBNull.Value)
                    oLessonPlanReportingConfig.MaxDate = Convert.ToDateTime(aoSqlDataReader["MaxDate"]);
                else
                    oLessonPlanReportingConfig.MaxDate = DateTime.MaxValue;
                olstLessonPlanConfig.Add(oLessonPlanReportingConfig);
            }
            return olstLessonPlanConfig;
        }
        #endregion
    }
}
