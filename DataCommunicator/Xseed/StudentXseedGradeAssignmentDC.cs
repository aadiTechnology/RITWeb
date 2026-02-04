using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;
using XseedReportEntities;

namespace DataCommunicator
{
    public class StudentXseedGradeAssignmentDC
    {
        #region Data Members

        public GradeSubmitStatus GradeSubmitStatus { get; set; }
        public LearningOutcomeConfigMaster LearningOutcomeConfig { get; set; }
        public LearningOutcomesObservation LearningOutcomesObservation { get; set; }
        
        public List<YearwiseStudentMaster> lstYearwiseStudents = new List<YearwiseStudentMaster>();
        public List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationDetail = new List<SubjectSectionConfigurationMaster>();
        public List<GradeMaster> lstGradeDetails = new List<GradeMaster>();
        public AssessmentMaster AssessmentDetails { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string Obsevation { get; set; }
        public LearningOutcomesGrade LearningOutcomesGradeDetails { get; set; }
        private bool mbIsExamPublished = false;
          
        #endregion

        #region Constructor

        public StudentXseedGradeAssignmentDC()
        {
        }
        #endregion

        public bool IsExamPublished
        {
            get { return this.mbIsExamPublished; }
        }

        #region Methods

        public void GetStudentsForStdDiv()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", GradeSubmitStatus.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", GradeSubmitStatus.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", GradeSubmitStatus.StandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", GradeSubmitStatus.AssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", GradeSubmitStatus.SubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].usp_GetStudentsForStdDev"))
                {
                    if (oSqlDataReader != null)
                    {
                        FillYearwiseStudentDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillSubjectSectionDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                            AssessmentDetails = new AssessmentMaster { Name = Convert.ToString(oSqlDataReader["Name"]) };
                        if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                        {
                            ClassName = Convert.ToString(oSqlDataReader["ClassName"]);
                            SubjectName = Convert.ToString(oSqlDataReader["Subject_Name"]);
                        }
                        if (oSqlDataReader.NextResult())
                            FillGradeDetails(oSqlDataReader);
                    }
                }
            }
        }

        private void FillGradeDetails(SqlDataReader aoSqlDataReader)
        {
            GradeMaster oGradeMaster = null;
            while (aoSqlDataReader.Read())
            {
                oGradeMaster = new GradeMaster
                {
                    GradeId = Convert.ToInt32(aoSqlDataReader["GradeId"]),
                    GradeName = Convert.ToString(aoSqlDataReader["GradeName"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    ConsideredAsAbsent = Convert.ToBoolean(aoSqlDataReader["ConsideredAsAbsent"]),
                    ConsideredAsExempted = Convert.ToBoolean(aoSqlDataReader["ConsideredAsExempted"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                };
                lstGradeDetails.Add(oGradeMaster);
            }
        }

        private void FillSubjectSectionDetails(SqlDataReader aoSqlDataReader)
        {
            SubjectSectionConfigurationMaster oSubjectSectionConfigurationMaster = null;
            while (aoSqlDataReader.Read())
            {
                oSubjectSectionConfigurationMaster = new SubjectSectionConfigurationMaster
                {
                    SubjectSectionConfigurationId = Convert.ToInt32(aoSqlDataReader["SubjectSectionConfigurationId"]),
                    SubjectSectionName = Convert.ToString(aoSqlDataReader["SubjectSectionName"])
                };
                lstSubjectSectionConfigurationDetail.Add(oSubjectSectionConfigurationMaster);
            }
        }

        private void FillYearwiseStudentDetails(SqlDataReader aoSqlDataReader)
        {
            YearwiseStudentMaster oYearwiseStudentMaster = null;
            while (aoSqlDataReader.Read())
            {
                oYearwiseStudentMaster = new YearwiseStudentMaster
                {
                    YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearWise_Student_Id"]),
                    StudentName = Convert.ToString(aoSqlDataReader["StudentName"])
                };
                lstYearwiseStudents.Add(oYearwiseStudentMaster);
            }
        }

        public List<LearningOutcomeConfigMaster> GetLearningOutcomesForStdDiv(int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", LearningOutcomesObservation.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", LearningOutcomesObservation.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", LearningOutcomesObservation.AssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectSectionConfigurationId", LearningOutcomesObservation.SubjectSectionConfigurationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", LearningOutcomesObservation.YearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetLearningOutcomesForSubjectSection]"))
                {

                    List<LearningOutcomeConfigMaster> lstLearningOutcomeConfig = new List<LearningOutcomeConfigMaster>();
                    if (oSqlDataReader != null)
                    {
                        LearningOutcomeConfigMaster oLearningOutcomeConfigMaster;
                        while (oSqlDataReader.Read())
                        {
                            oLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster
                            {
                                LearningOutcomeConfigId = Convert.ToInt32(oSqlDataReader["LearningOutcomeConfigId"]),
                                LearningOutCome = Convert.ToString(oSqlDataReader["LearningOutCome"]),
                                GradeId = Convert.ToInt32(oSqlDataReader["GradeId"]),
                                LearningOutcomeGradeId = Convert.ToInt32(oSqlDataReader["LearningOutcomeGradeId"])
                            };
                            lstLearningOutcomeConfig.Add(oLearningOutcomeConfigMaster);
                        }

                        if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                        {
                            LearningOutcomesObservation = new LearningOutcomesObservation
                            {
                                Observation = Convert.ToString(oSqlDataReader["Observation"]),
                                LearningOutcomesObservationId = Convert.ToInt32(oSqlDataReader["LearningOutcomesObservationId"]),
                                SubjectRemark = Convert.ToString(oSqlDataReader["SubjectRemark"]),
                                ShowSubjectRemark = Convert.ToBoolean(oSqlDataReader["ShowSubjectRemark"])
                            };
                        }

                        if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                            mbIsExamPublished = Convert.ToBoolean(oSqlDataReader["IsExamPublished"]);
                    }
                    return lstLearningOutcomeConfig;
                }
            }
        }


        /// <summary>
        /// This method is used to get all student list to assign grades and observations.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiAssessmwntId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public static List<StudentXseedGradeDetails> GetAllStudents(int aiSchoolId, int aiAcademicYearId, int aiStandardDivId, int aiAssessmwntId, int aiSubjectId)
        {
            List<StudentXseedGradeDetails> lstStudentXseedGradeDetails = new List<StudentXseedGradeDetails>();
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AssessmentId", aiAssessmwntId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetStudentsForNonXseedSubjects]"))
                {
                    if (oSqlDataReader != null)
                    {
                        StudentXseedGradeDetails oStudentXseedGradeDetails;
                        while (oSqlDataReader.Read())
                        {
                            oStudentXseedGradeDetails = new StudentXseedGradeDetails
                            {
                                YaerwiseStudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                                StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                                RollNumber = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                                Observations = Convert.ToString(oSqlDataReader["Observation"]),
                                GradeId = Convert.ToInt32(oSqlDataReader["GradeId"]),
                            };
                            lstStudentXseedGradeDetails.Add(oStudentXseedGradeDetails);
                        }
                    }
                    return lstStudentXseedGradeDetails;
                }
            }
        }

        public void Save(int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", LearningOutcomesGradeDetails.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", LearningOutcomesGradeDetails.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", LearningOutcomesGradeDetails.YearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LearningOutcomeXML", LearningOutcomesGradeDetails.LearningOutcomeXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", LearningOutcomesGradeDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", LearningOutcomesObservation.AssessmentId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("LearningOutcomesObservationId", LearningOutcomesObservation.LearningOutcomesObservationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectSectionConfigurationId", LearningOutcomesObservation.SubjectSectionConfigurationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Observation", LearningOutcomesObservation.Observation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SubjectRemark", LearningOutcomesObservation.SubjectRemark, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Xseed].[usp_InsertStudentGrades]");
            }
        }

        /// <summary>
        /// This method is used to save, update and delete assigned grade and observation details.
        /// </summary>
        /// <param name="asXseedGradesXML"></param>
        public void Save(string asXseedGradesXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("XseedGradesXML", asXseedGradesXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", GradeSubmitStatus.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", GradeSubmitStatus.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", GradeSubmitStatus.AssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivId", GradeSubmitStatus.StandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", GradeSubmitStatus.SubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", GradeSubmitStatus.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", GradeSubmitStatus.UpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Xseed].[Usp_SaveNonXseedSubGrades]");
            }
        }

        #endregion
    }
}
