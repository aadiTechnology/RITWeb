/*
 *  File Name - StaffPerformanceEvaluationDC.cs
 *  Created By - Sachin
 *  Created Date - 30 Sept 2013
 *  Description - This class is used to communicate with database for managing performance evaluation details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using StaffPerformanceEntity;
using Utility;

namespace DataCommunicator
{
    public class StaffPerformanceEvaluationDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcadeicYearId;
        private int miUpdatedById;
        private int miSelectedUserId;
        private SchoolEntity moSchoolEntity = new SchoolEntity();
        private List<ReportingStaff> mlstReportingStaffs = new List<ReportingStaff>();
        private List<PerformanceGrade> mlstPerformanceGrades = new List<PerformanceGrade>();
        private List<PerformanceParameter> mlstPerformanceParameters = new List<PerformanceParameter>();
        private List<PerformanceSkill> mlstPerformanceSkills = new List<PerformanceSkill>();
        private List<StaffPerformanceStatus> mlstStaffPerformanceStatus = new List<StaffPerformanceStatus>();
        private ReportingStaff moReportingStaff = new ReportingStaff();
        private ButtonState moButtonState = new ButtonState();
        #endregion

        #region Constructor(s)
        
        /// <summary>
        /// Parameterized cosntructor(s)
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public StaffPerformanceEvaluationDC(int aiSchoolId, int aiUpdatedById, int aiSelectedUserId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miSelectedUserId = aiSelectedUserId;
            this.miAcadeicYearId = aiAcademicYearId;
        } 

        #endregion

        #region Property(s)

        public SchoolEntity SchoolEntity
        {
            get { return this.moSchoolEntity; }
        }

        public List<ReportingStaff> ReportingStaffs
        {
            get { return this.mlstReportingStaffs; }
        }

        public List<PerformanceGrade> PerformanceGrades
        {
            get { return this.mlstPerformanceGrades; }
        }

        public List<PerformanceParameter> PerformanceParameters
        {
            get { return this.mlstPerformanceParameters; }
        }

        public List<PerformanceSkill> PerformanceSkills
        {
            get { return this.mlstPerformanceSkills; }
        }

        public List<StaffPerformanceStatus> StaffPerformanceStatus
        {
            get { return this.mlstStaffPerformanceStatus; }
        }

        public ReportingStaff UserDetails
        {
            get { return this.moReportingStaff; }
        }

        public ButtonState ButtonState
        {
            get { return this.moButtonState; }
        }

        #endregion

        #region Public Method(s)
        
        /// <summary>
        /// This method is used to return performance observations for given staff.
        /// </summary>
        /// <param name="aiUserid"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public List<StaffPerformanceObservation> GetAll(int aiReportingUserId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miSelectedUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcadeicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStaffPerformanceEvaluationDetails"))
                {

                    SchoolDC oSchoolDC = new SchoolDC();
                    this.moSchoolEntity = oSchoolDC.GetSchoolDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    GetStaffStaffDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    PerformanceGradeDC oPerformanceGradeDC = new PerformanceGradeDC();
                    this.mlstPerformanceGrades = oPerformanceGradeDC.SetGradetDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    PerformanceParameterDC oPerformanceParameterDC = new PerformanceParameterDC(this.miSchoolId, this.miUpdatedById);
                    this.mlstPerformanceParameters = oPerformanceParameterDC.FillPerformanceParameters(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    PerformanceSkillDC oPerformanceSkillDC = new PerformanceSkillDC(this.miSchoolId, this.miUpdatedById);
                    this.mlstPerformanceSkills = oPerformanceSkillDC.FillSkillDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    List<StaffPerformanceObservation> lstStaffPerformanceEvaluations = FillPerformanceEvaluationDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    GetStaffPerformanceStatus(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillReportingStaffDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    SetButtonState(oSqlDataReader);

                    return lstStaffPerformanceEvaluations;
                }
            }
        }

        private void SetButtonState(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moButtonState.EnablePublishButton = Convert.ToBoolean(aoSqlDataReader["EnablePublishButton"]);
                this.moButtonState.EnableRejectButton = Convert.ToBoolean(aoSqlDataReader["EnableRejectButton"]);
                this.moButtonState.EnableSaveButton = Convert.ToBoolean(aoSqlDataReader["EnableSaveButton"]);
                this.moButtonState.EnableSubmitButton = Convert.ToBoolean(aoSqlDataReader["EnableSubmitButton"]);
                this.moButtonState.IsPublished = Convert.ToBoolean(aoSqlDataReader["IsPublished"]);
                this.moButtonState.CanUserAddComments = Convert.ToBoolean(aoSqlDataReader["CanUserAddComments"]);
            }
        }

        /// <summary>
        /// This method is used to save performance observations of given staff.
        /// </summary>
        /// <param name="aiUserid"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="asXml"></param>
        public void Save(int aiReportingUserId, int aiYear, string asXml,string asclass, string assubject)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miSelectedUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PerformanceXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Classes", asclass, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Subjects", assubject, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStaffPerformanceEvalDetails");
            }
        }
        /// <summary>
        /// This method is used to publish performance observations of given staff.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="abIsPublish"></param>
        public void Publish(int aiYear, bool abIsPublish, string asEffectiveDate, string asLastIncrementDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", this.miSelectedUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublish", abIsPublish, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcadeicYearId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("EffectiveDate", asEffectiveDate, SqlDbType.DateTime);

                if (!string.IsNullOrEmpty(asLastIncrementDate))
                    oSQLServerDbUtility.AddParameter("LastIncrementDate", asLastIncrementDate, SqlDbType.DateTime);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishStaffPerformanceDetails");
            }
        }

        /// <summary>
        /// This method is used to submit performance observations of given staff.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        public void Submit(int aiYear, bool abIsSubmitAction)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", this.miSelectedUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitAction", abIsSubmitAction, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStaffPerformanceDetails");
            }
        } 

        /// <summary>
        /// This method is used to get File Count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiDocumentTypeId"></param>
        /// <returns></returns>
        public int GetAttachmentCount(int aiAcademicYearId, int aiDocumentTypeId)
        {
            string sQuery = "select count(*) from StaffPerformanceAttachment where SchoolId=" + miSchoolId + " AND AcademicYearId=" + aiAcademicYearId + " AND UserId=" + miSelectedUserId + " AND DocumentTypeId=" + aiDocumentTypeId + " and IsDeleted=0";
            using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            return oSQLServerDbUtility.ExecuteTransaction(sQuery);
        }
        #endregion
      
        #region Private Method(s)

        /// <summary>
        /// This method is used to set taff details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void GetStaffStaffDetails(SqlDataReader aoSqlDataReader)
        {
            this.moReportingStaff = null;
            if (aoSqlDataReader.Read())
            {
                this.moReportingStaff = new ReportingStaff
                {
                    Name = aoSqlDataReader["UserName"].ToString(),
                    Designation = aoSqlDataReader["Designation"].ToString(),
                    JobStatus = aoSqlDataReader["JobStatus"].ToString(),
                    EmployeeNo = aoSqlDataReader["EmployeeNo"].ToString(),
                    JoiningDate = (aoSqlDataReader["JoiningDate"] == DBNull.Value ? "-" : Convert.ToDateTime(aoSqlDataReader["JoiningDate"]).ToString(Constants.S_DATE_FORMAT)),
                    ServiceLength = aoSqlDataReader["ServiceLength"].ToString(),
                    FormFor = aoSqlDataReader["FormFor"].ToString(),
                    Standards = aoSqlDataReader["Standards"].ToString(),
                    Subjects = aoSqlDataReader["Subjects"].ToString(),
                    UserRoleId = aoSqlDataReader["UserRoleId"].ToInt(),
                    AcademicYear = aoSqlDataReader["AcademicYear"].ToString(),
                    LastIncrementDate = (aoSqlDataReader["LastIncrementDate"] == DBNull.Value?string.Empty:Convert.ToDateTime(aoSqlDataReader["LastIncrementDate"]).ToString(Constants.S_DATE_FORMAT)),
                    EffectiveFromDate = (aoSqlDataReader["EffectiveFromDate"] == DBNull.Value ? DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT) : Convert.ToDateTime(aoSqlDataReader["EffectiveFromDate"]).ToString(Constants.S_DATE_FORMAT)),
                    Address=(aoSqlDataReader["Address"]).ToString(),  //new add
                    HighestEducation = (aoSqlDataReader["Year_Of_Passing"]).ToString(),///new add                    
                };
            }
        }

        /// <summary>
        /// This method is used to set reporting staff details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillReportingStaffDetails(SqlDataReader aoSqlDataReader)
        {
            this.mlstReportingStaffs.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstReportingStaffs.Add(new ReportingStaff
                {
                    Name = aoSqlDataReader["UserName"].ToString(),
                    Designation = aoSqlDataReader["Designation"].ToString(),
                    IsFinalApprover = Convert.ToBoolean(aoSqlDataReader["IsFinalApprover"]),
                    IsSupervisor = Convert.ToBoolean(aoSqlDataReader["IsSupervisor"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                    ReportingUserId = Convert.ToInt32(aoSqlDataReader["ReportingUserId"]),
                    ApprovalSortOrder = Convert.ToInt32(aoSqlDataReader["ApprovalSortOrder"]),
                    AttachmentCount = aoSqlDataReader["AttachmentCount"].ToString()
                });
            }
        }

        /// <summary>
        /// This method is used to set pwerformance status.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void GetStaffPerformanceStatus(SqlDataReader aoSqlDataReader)
        {
            this.mlstStaffPerformanceStatus.Clear();
            while (aoSqlDataReader.Read())
            {
                this.mlstStaffPerformanceStatus.Add(new StaffPerformanceStatus
                {
                    IsPublished = Convert.ToBoolean(aoSqlDataReader["IsPublished"]),
                    StaffPerformanceEvalDetailsId = Convert.ToInt32(aoSqlDataReader["Id"]),
                    ReportingUserId = Convert.ToInt32(aoSqlDataReader["ReportingUserId"])
                });
            }
        }

        /// <summary>
        /// This method is used to set performance evaluation details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StaffPerformanceObservation> FillPerformanceEvaluationDetails(SqlDataReader aoSqlDataReader)
        {
            List<StaffPerformanceObservation> lstObservations = new List<StaffPerformanceObservation>();
            while (aoSqlDataReader.Read())
            {
                lstObservations.Add(new StaffPerformanceObservation
                {
                    Id = aoSqlDataReader["Id"].ToInt(),
                    ParameterId = aoSqlDataReader["ParameterId"].ToInt(),
                    GradeId = aoSqlDataReader["GradeId"].ToInt(),
                    Observation = aoSqlDataReader["Observation"].ToString(),
                    ReportingUserId = aoSqlDataReader["ReportingUserId"].ToInt(),
                });
            }

            return lstObservations;
        } 

        #endregion

        public void RejectSubmittion(int aiUserId, string asReason, int aiReportingUserId, int aiAcademicYearId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Reason", asReason, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EvalYear", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_RejectAppraisalSubmittion");
            }
        }
    }
}
