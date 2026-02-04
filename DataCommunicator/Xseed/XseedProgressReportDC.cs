using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;
using SchoolEntities;
using XseedReportEntities;

namespace DataCommunicator
{
    public class XseedProgressReportDC
    {
        public ExamResult oExamResult;
        public List<StandardwiseSubject> lstStandardwiseSubjects = new List<StandardwiseSubject>();
        public List<SubjectSectionConfigurationMaster> lstSubjectSections = new List<SubjectSectionConfigurationMaster>();
        public List<StudentsLearningOutcome> lstStudentsLearningOutcomes = new List<StudentsLearningOutcome>();
        public List<LearningOutcomesObservation> lstLearningOutcomesObservations = new List<LearningOutcomesObservation>();
        public List<NonXseedSubjectGrades> lstNonXseedSubjectGardes = new List<NonXseedSubjectGrades>();
        public SchoolEntity moSchoolEntity = new SchoolEntity();
        public List<YearwiseStudentMaster> lstYearwiseStudentMaster = new List<YearwiseStudentMaster>();
        public List<GradeMaster> lstGradeMaster = new List<GradeMaster>();
        public List<AssessmentMaster> lstAssessmentMaster = new List<AssessmentMaster>();
        public List<ClassTeacherDetails> lstClassTeacherDetails = new List<ClassTeacherDetails>();
        public List<StudentAttendance> lstStudentAttendance = new List<StudentAttendance>();
        public List<XseedRemark> lstRemarks = new List<XseedRemark>();
        public List<SubjectRemark> lstSubjectRemarks = new List<SubjectRemark>();
        public string msLearningOutcomeXML;
        public string msXseedGradesXML;
        public bool mbAssessmentPublishStatus;
        public bool mbStudentWiseAssessmentPublishStatus;
        public string msRemark;
        public string msSubjectRemark;
       

        public XseedProgressReportDC()
        {
        }
        public void GetProgressReportDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", oExamResult.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", oExamResult.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", oExamResult.StandardDivisionId, SqlDbType.Int);
                if (oExamResult.YearwiseStudentId != 0)
                    oSQLServerDbUtility.AddParameter("YearwiseStudentId", oExamResult.YearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", oExamResult.AssessmentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetXseedProgressReportDetails]"))
                {
                    if (oSqlDataReader.Read())
                        FillStandardwiseSubjects(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillSubjectSections(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStudentsLearningOutcomes(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStudentsLearningOutcomeObservations(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillNonXseedSubjectGrades(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillSchoolDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStudentDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillGradeDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStudentAttendance(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        while (oSqlDataReader.Read())
                        {
                            mbAssessmentPublishStatus = Convert.ToString(oSqlDataReader["AssessmentPublishStatus"]) == Constants.S_YES;
                            mbStudentWiseAssessmentPublishStatus = Convert.ToString(oSqlDataReader["StudentWiseAssessmentPublishStatus"]) == Constants.S_YES;
                        }
                    if (oSqlDataReader.NextResult())
                        FillXseedRemarks(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillSubjectRemarks(oSqlDataReader);
                }
            }
        }

        private void FillSubjectRemarks(SqlDataReader aoSqlDataReader)
        {
            SubjectRemark oSubjectRemark;
            while (aoSqlDataReader.Read())
            {
                lstSubjectRemarks.Add
                    (
                        oSubjectRemark = new SubjectRemark
                        {
                            SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]),
                            Remark = Convert.ToString(aoSqlDataReader["Remark"])
                        }
                    );
            }
        }

        private void FillXseedRemarks(SqlDataReader aoSqlDataReader)
        {
            XseedRemark oXseedRemark;
            while (aoSqlDataReader.Read())
            {
                lstRemarks.Add
                    (
                        oXseedRemark = new XseedRemark
                        {
                            YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearwiseStudentId"]),
                            Remark = Convert.ToString(aoSqlDataReader["Remark"])
                        }
                    );
            }
        }

        private void FillStudentAttendance(SqlDataReader aoSqlDataReader)
        {
            StudentAttendance oStudentAttendance = null;
            while (aoSqlDataReader.Read())
            {
                oStudentAttendance = new StudentAttendance
                {
                    YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearwiseStudentId"]),
                    IsPresent = Convert.ToBoolean(aoSqlDataReader["IsPresent"])
                };
                lstStudentAttendance.Add(oStudentAttendance);
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
                    Description = Convert.ToString(aoSqlDataReader["Description"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    ConsideredAsAbsent = Convert.ToBoolean(aoSqlDataReader["ConsideredAsAbsent"]),
                    ConsideredAsExempted = Convert.ToBoolean(aoSqlDataReader["ConsideredAsExempted"])
                };
                lstGradeMaster.Add(oGradeMaster);
            }
        }

        private void FillStudentDetails(SqlDataReader aoSqlDataReader)
        {
            YearwiseStudentMaster oYearwiseStudentMaster = null;
            while (aoSqlDataReader.Read())
            {
                oYearwiseStudentMaster = new YearwiseStudentMaster
                {
                    YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearwiseStudentId"]),
                    RollNo = Convert.ToInt32(aoSqlDataReader["RollNo"]),
                    StudentName = Convert.ToString(aoSqlDataReader["StudentName"]),
                    Assessment = Convert.ToString(aoSqlDataReader["Assessment"]),
                    Class = Convert.ToString(aoSqlDataReader["Class"]),
                    AcademicYear = Convert.ToString(aoSqlDataReader["AcademicYear"])
                };
                lstYearwiseStudentMaster.Add(oYearwiseStudentMaster);
            }
        }

        private void FillSchoolDetails(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                moSchoolEntity = new SchoolEntity
                {
                    OrganizationName = Convert.ToString(aoSqlDataReader["OrganizationName"]),
                    SchoolName = Convert.ToString(aoSqlDataReader["School_Name"])
                };
            }
        }

        private void FillNonXseedSubjectGrades(SqlDataReader aoSqlDataReader)
        {
            NonXseedSubjectGrades oNonXseedSubjectGardes = null;
            while (aoSqlDataReader.Read())
            {
                oNonXseedSubjectGardes = new NonXseedSubjectGrades
                {
                    YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearwiseStudentId"]),
                    AssessmentId = Convert.ToInt32(aoSqlDataReader["AssessmentId"]),
                    GradeId = Convert.ToInt32(aoSqlDataReader["GradeId"]),
                    SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    SubjectName = Convert.ToString(aoSqlDataReader["SubjectName"]),
                    Observation = Convert.ToString(aoSqlDataReader["Observation"]),
                    IsCoCurricularActivity = Convert.ToBoolean(aoSqlDataReader["Is_CoCurricularActivity"])
                };
                lstNonXseedSubjectGardes.Add(oNonXseedSubjectGardes);
            }
        }

        private void FillStudentsLearningOutcomeObservations(SqlDataReader aoSqlDataReader)
        {
            LearningOutcomesObservation oLearningOutcomesObservation = null;
            while (aoSqlDataReader.Read())
            {
                oLearningOutcomesObservation = new LearningOutcomesObservation
                {
                    YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearwiseStudentId"]),
                    LearningOutcomesObservationId = Convert.ToInt32(aoSqlDataReader["LearningOutcomesObservationId"]),
                    SubjectSectionConfigurationId = Convert.ToInt32(aoSqlDataReader["SubjectSectionConfigurationId"]),
                    Observation = Convert.ToString(aoSqlDataReader["Observation"]),
                };
                lstLearningOutcomesObservations.Add(oLearningOutcomesObservation);
            }
        }

        private void FillStudentsLearningOutcomes(SqlDataReader aoSqlDataReader)
        {
            StudentsLearningOutcome oStudentsLearningOutcome = null;
            while (aoSqlDataReader.Read())
            {
                oStudentsLearningOutcome = new StudentsLearningOutcome
                {
                    YearwiseStudentId = Convert.ToInt32(aoSqlDataReader["YearwiseStudentId"]),
                    LearningOutcomeConfigId = Convert.ToInt32(aoSqlDataReader["LearningOutcomeConfigId"]),
                    SubjectSectionConfigId = Convert.ToInt32(aoSqlDataReader["SubjectSectionConfigId"]),
                    LearningOutcome = Convert.ToString(aoSqlDataReader["LearningOutcome"]),
                    GradeId = Convert.ToInt32(aoSqlDataReader["GradeId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    SubjectSectionSortOrder = Convert.ToInt32(aoSqlDataReader["SubjectSectionSortOrder"]),
                    LearningOutcomeSortOrder = Convert.ToInt32(aoSqlDataReader["LearningOutcomeSortOrder"]),
                    LearningOutcomeGradeId = Convert.ToInt32(aoSqlDataReader["LearningOutcomeGradeId"]),
                };
                lstStudentsLearningOutcomes.Add(oStudentsLearningOutcome);
            }
        }

        private void FillSubjectSections(SqlDataReader aoSqlDataReader)
        {
            SubjectSectionConfigurationMaster oSubjectSectionConfigurationMaster = null;
            while (aoSqlDataReader.Read())
            {
                oSubjectSectionConfigurationMaster = new SubjectSectionConfigurationMaster
                {
                    SubjectSectionConfigurationId = Convert.ToInt32(aoSqlDataReader["SubjectSectionConfigurationId"]),
                    StandardwiseSubjectId = Convert.ToInt32(aoSqlDataReader["StandardwiseSubjectId"]),
                    SubjectSectionName = Convert.ToString(aoSqlDataReader["SubjectSectionName"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]),
                    ShowSubjectRemarks = Convert.ToBoolean(aoSqlDataReader["ShowSubjectRemarks"])
                };
                lstSubjectSections.Add(oSubjectSectionConfigurationMaster);
            }
        }

        private void FillStandardwiseSubjects(SqlDataReader aoSqlDataReader)
        {
            StandardwiseSubject oStandardwiseSubject = null;
            while (aoSqlDataReader.Read())
            {
                oStandardwiseSubject = new StandardwiseSubject
                {
                    StandardDivisionId = Convert.ToInt32(aoSqlDataReader["StandardDivisionId"]),
                    StandardSubjectId = Convert.ToInt32(aoSqlDataReader["StandardSubjectId"]),
                    SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]),
                    SubjectName = Convert.ToString(aoSqlDataReader["SubjectName"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"])                    
                };
                lstStandardwiseSubjects.Add(oStandardwiseSubject);
            }
        }

        public void GetClassTeachers(int aiSchoolId, int aiAcademicYEarId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYEarId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetClassTeachers]"))
                {
                    if (oSqlDataReader != null)
                    {
                        if (oSqlDataReader != null)
                            FillClassTeacherDetails(oSqlDataReader);
                    }
                }
            }
        }

        public List<AssessmentMaster> GetPublishedAssesments(int aiSchoolId, int aiAcademicYearId, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<AssessmentMaster> lstAssessments = new List<AssessmentMaster>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[GetPublishedAssessments]"))
                {
                    if (oSqlDataReader != null)
                    {
                        AssessmentMaster oAssessmentMaster = null;
                        while (oSqlDataReader.Read())
                        {
                            oAssessmentMaster = new AssessmentMaster
                            {
                                AssessmentId = Convert.ToInt32(oSqlDataReader["AssessmentId"]),
                                Name = Convert.ToString(oSqlDataReader["Name"]),
                            };
                            lstAssessments.Add(oAssessmentMaster);
                        }
                    }
                    return lstAssessments;
                }
            }
        }

        private void FillClassTeacherDetails(SqlDataReader aoSqlDataReader)
        {
            ClassTeacherDetails oClassTeacherDetails;
            while (aoSqlDataReader.Read())
            {
                oClassTeacherDetails = new ClassTeacherDetails
                {
                    StandardDivisionId = Convert.ToInt32(aoSqlDataReader["SchoolWise_Standard_Division_Id"]),
                    TeacherName = Convert.ToString(aoSqlDataReader["TeacherName"]),
                    TeacherId = Convert.ToInt32(aoSqlDataReader["Teacher_Id"])
                };
                lstClassTeacherDetails.Add(oClassTeacherDetails);
            }
        }

        private void FillAssessmentDetails(SqlDataReader aoSqlDataReader)
        {
            AssessmentMaster oAssessmentMaster = null;
            while (aoSqlDataReader.Read())
            {
                oAssessmentMaster = new AssessmentMaster
                {
                    AssessmentId = Convert.ToInt32(aoSqlDataReader["AssessmentId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                };
                lstAssessmentMaster.Add(oAssessmentMaster);
            }
        }

        public List<YearwiseStudentMaster> GetStudents(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetStudentDetails]"))
                {
                    List<YearwiseStudentMaster> lstYearwiseStudentMaster = new List<YearwiseStudentMaster>();
                    if (oSqlDataReader != null)
                    {
                        YearwiseStudentMaster oYearwiseStudentMaster = null;
                        while (oSqlDataReader.Read())
                        {
                            oYearwiseStudentMaster = new YearwiseStudentMaster
                            {
                                YearwiseStudentId = Convert.ToInt32(oSqlDataReader["YearwiseStudentId"]),
                                StudentName = Convert.ToString(oSqlDataReader["StudentName"])
                            };
                            lstYearwiseStudentMaster.Add(oYearwiseStudentMaster);
                        }
                    }

                    if (oSqlDataReader.NextResult())
                        FillAssessmentDetails(oSqlDataReader);

                    return lstYearwiseStudentMaster;
                }
            }
        }

        public bool IsXseedApplicable(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiTeachersStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeachersStandardDivisionId", aiTeachersStandardDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_IsXseedApplicable]"))
                {
                    if (oSqlDataReader != null && oSqlDataReader.Read())
                        return Convert.ToBoolean(oSqlDataReader["IsXseedApplicable"]);
                    return false;
                }
            }
        }

        public void ManageStudentWiseAssessmentGrades(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiStandardDivisionId, int aiAssessmentId, int aiUserId, string asMode)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LearningOutcomeXML", msLearningOutcomeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("XseedGradesXML", msXseedGradesXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Mode", asMode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Remark", msRemark, SqlDbType.NVarChar);
                if (msSubjectRemark != string.Empty)
                    oSQLServerDbUtility.AddParameter("SubjectRemarkXML", msSubjectRemark, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Xseed].[usp_ManageStudentWiseAssessmentGrades]");
            }
       }

        
        /// <summary>
        /// THis method is used to save Publish Xseed Result.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAssessmentId"></param>
        /// <param name="asMode"></param>
        public void PublishXseedResult(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiAssessmentId, string asMode, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Mode", asMode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishXseedResult");
            }
        }
        /// <summary>
        /// This method is used to Get PublishStatus.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAssessmentId"></param>
        /// <returns></returns>
        public PublishStatus GetPublishStatus(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiAssessmentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPublishStatus"))
                {
                    PublishStatus oPublishStatus = new PublishStatus();
                    if (oSqlDataReader.Read())
                    {
                        oPublishStatus.AllowPublish = oSqlDataReader["AllowPublish"].ToBool();
                        oPublishStatus.AllowUnpublish = oSqlDataReader["AllowUnpublish"].ToBool();
                    }
                    return oPublishStatus;
                }               
            }
        }

        public bool IsXseedResultPublished(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiAssessmentId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAssessmentPublishStatus"))
                {
                    if (oSqlDataReader.Read())
                        return oSqlDataReader["PublishStatus"].ToBool();
                    else
                        return false;
                }
            }
        }
    }
}
