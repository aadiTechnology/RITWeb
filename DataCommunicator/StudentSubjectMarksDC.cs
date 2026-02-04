using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProgressReportEntities;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class StudentSubjectMarksDC
    {
        #region Constructor

        public StudentSubjectMarksDC()
        {
        }

        public StudentSubjectMarksDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Structure

        public struct StudentSubjectMarksStruct
        {
            public int miInsertedByid;
            public int miTestWiseSubjectMarksId;
            public string msStudentDetails;
            public string msStudentMarkDetails;
            public string msStudentTestSubmitStatus;
            public string msRemarkXml;
            public bool mbHasRemarks;
            public int miTestId;
            public int miSubjectId;
        }

        #endregion

        #region Data members

        private StudentSubjectMarksStruct moStudentSubjectMarksStruct;
        public string msTestIds;
        public int miStudentMarksTransferCount;
        private int miSchoolId;
        private int miAcademicYearId;
        
        #endregion

        #region Properties

        public StudentSubjectMarksStruct StudentSubjectMarksStructDetails
        {
            get { return moStudentSubjectMarksStruct; }
            set { moStudentSubjectMarksStruct = value; }
        }

        #endregion

        #region Public Methods

        public void ManageStudentTestMarks(string asRemoveProgress, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestWise_Subject_Marks_Id", moStudentSubjectMarksStruct.miTestWiseSubjectMarksId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moStudentSubjectMarksStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Test_Type_Marks", moStudentSubjectMarksStruct.msStudentDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Student_Test_Type_Marks_Details", moStudentSubjectMarksStruct.msStudentMarkDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("RemoveProgress", asRemoveProgress, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RemarkXml", moStudentSubjectMarksStruct.msRemarkXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("HasRemark", moStudentSubjectMarksStruct.mbHasRemarks, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("TestId", moStudentSubjectMarksStruct.miTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", moStudentSubjectMarksStruct.miSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId , SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ManageStudentTestMarks",true);
            }
        }

        public void ManageTestWiseStudentMarks(string asRemoveProgress, string asMode, int aiStandardDivisionId, string asRoundMarksAtSubjectLevel)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moStudentSubjectMarksStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Test_Type_Marks", moStudentSubjectMarksStruct.msStudentDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Student_Test_Type_Marks_Details", moStudentSubjectMarksStruct.msStudentMarkDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("StudentWise_Test_Submit_Status", moStudentSubjectMarksStruct.msStudentTestSubmitStatus, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("RemoveProgress", asRemoveProgress, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Mode", asMode, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RoundMarksAtSubjectLevel", asRoundMarksAtSubjectLevel, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ManageTestWiseStudentMarks",true);
            }
        }

        public static void UpdateStudentTestMarks(int aiUpdated_By_id, string asStudentMarkDetails, char acUseAvarageFinalResult)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Student_Test_Type_Marks_Details", asStudentMarkDetails, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Updated_By_id", aiUpdated_By_id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("cUseAvarageFinalResult", acUseAvarageFinalResult, SqlDbType.NVarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_UpdateStudentTestMarks",true);
            }
        }

        public static void UpdatePrePrimaryTestMarks(string asStudentMarkDetails, string sTestComment, int aiStudentId, int aiTestId, int aiUpdated_By_id)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Student_Marks_Details", asStudentMarkDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("sTestComment", sTestComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Updated_By_id", aiUpdated_By_id, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_UpdatePrePrimaryProgressTestMarks",true);
            }
        }

        public DataSet GetAllRelatedInformation(int aiSchoolId, int aiAcademicYrId, int aiSubjectId, int aiTestId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Subject_Id", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYrId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetInfoForStudentMarkAssignment");
            }
        }

        /// <summary>
        /// This method is used to get student progress report details which are stored xml format.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public StudentProgressReport GetStudentProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId, int iUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iUserId", iUserId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_StudentProgressReport"))
                {
                    return new StudentProgressReport()
                    {
                        StudentDetails = GetStudentDetails(oSqlDataReader),
                        SubjectDetails = GetSubjectDetails(oSqlDataReader),
                        ExamDetails = GetExamDetails(oSqlDataReader),
                        MarkAssignmentDetails = GetMarkAssignmentDetails(oSqlDataReader),
                        ExamWisePercentageDetails = GetExamWisePercentageDetails(oSqlDataReader),
                        SubjectTestGroupTotalDetails = GetSubjectTestGroupTotalDetails(oSqlDataReader),
                        SubjectTestTypeGroupTotalDetails = GetSubjectTestTypeGroupTotalDetails(oSqlDataReader),
                        SubjectTestTypeDetails = GetSubjectTestTypeDetails(oSqlDataReader),
                        TestTypeDetails = GetTestTypeDetails(oSqlDataReader),
                        GradeDetails = GetGradeDetails(oSqlDataReader),
                        ExamStatusDetails = GetExamStatusDetails(oSqlDataReader),
                        DependentExamDetails = GetDependentExamDetails(oSqlDataReader),
                    };
                }
            }
        }

        /// <summary>
        /// This method is used to get stdudent's progress report details dynamically.
        /// iTestId:- It is used to get progress report details of a particular test.
        /// msTestIds:- To get progress report details of test ids specidied in this field.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="iTestId"></param>
        /// <returns></returns>
        public StudentProgressReport GetStudentTestProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId, int iTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iTestId", iTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iMakePersist", 0, SqlDbType.Int);
                if (!msTestIds.IsNullOrEmpty())
                    oSQLServerDbUtility.AddParameter("TestIds", msTestIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GenerateStudentProgressSheet"))
                {

                    StudentProgressReport oStudentProgressReport = new StudentProgressReport();

                    // Get Student Details.
                    oStudentProgressReport.StudentDetails = GetStudentDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                    {
                        oStudentProgressReport.SubjectDetails = new List<Subject>();
                        while (oSqlDataReader.Read())
                        {
                            ProgressReportSubujectDetails oSubject = new ProgressReportSubujectDetails();
                            if (oSqlDataReader["ID_Num"] != DBNull.Value)
                                oSubject.Id = oSqlDataReader["ID_Num"].ToInt();
                            if (oSqlDataReader["Subject_Name"] != DBNull.Value)
                                oSubject.SubjectName = oSqlDataReader["Subject_Name"].ToString();
                            if (oSqlDataReader["Subject_Id"] != DBNull.Value)
                                oSubject.SubjectId = oSqlDataReader["Subject_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                                oSubject.ParentSubjectId = oSqlDataReader["Parent_Subject_Id"].ToInt();
                            if (oSqlDataReader["Total_Consideration"] != DBNull.Value)
                                oSubject.TotalConsideration = oSqlDataReader["Total_Consideration"].ToString();
                            if (oSqlDataReader["Sort_Order"] != DBNull.Value)
                                oSubject.SortOrder = oSqlDataReader["Sort_Order"].ToInt();
                            oStudentProgressReport.SubjectDetails.Add(oSubject);
                        }
                    }

                    oStudentProgressReport.ExamDetails = GetExamDetails(oSqlDataReader);
                    oStudentProgressReport.MarkAssignmentDetails = GetMarkAssignmentDetails(oSqlDataReader);

                    // Get exam wise percentage details.
                    if (oSqlDataReader.NextResult())
                    {
                        oStudentProgressReport.ExamWisePercentageDetails = new List<ExamWisePercentage>();
                        while (oSqlDataReader.Read())
                        {
                            StudentWiseProgressReportExamWisePercentage oExamWisePercentage = new StudentWiseProgressReportExamWisePercentage();
                            if (oSqlDataReader["SchoolWise_Test_Id"] != DBNull.Value)
                                oExamWisePercentage.SchoolWiseTestId = oSqlDataReader["SchoolWise_Test_Id"].ToInt();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oExamWisePercentage.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subjects_Total_Marks"] != DBNull.Value)
                                oExamWisePercentage.SubjectTotalMarks = oSqlDataReader["Subjects_Total_Marks"].ToInt();
                            if (oSqlDataReader["Percentage"] != DBNull.Value)
                                oExamWisePercentage.Percentage = oSqlDataReader["Percentage"].ToDecimal();
                            if (oSqlDataReader["Grade_Name"] != DBNull.Value)
                                oExamWisePercentage.Grade = oSqlDataReader["Grade_Name"].ToString();
                            if (oSqlDataReader["Grade_id"] != DBNull.Value)
                                oExamWisePercentage.GradeId = oSqlDataReader["Grade_id"].ToInt();
                            if (oSqlDataReader["Result"] != DBNull.Value)
                                oExamWisePercentage.Result = oSqlDataReader["Result"].ToString();
                            if (oSqlDataReader["rank"] != DBNull.Value)
                                oExamWisePercentage.Rank = oSqlDataReader["rank"].ToInt();
                            oStudentProgressReport.ExamWisePercentageDetails.Add(oExamWisePercentage);
                        }
                    }


                    // Get subject test group total details.
                    if (oSqlDataReader.NextResult())
                    {
                        oStudentProgressReport.SubjectTestGroupTotalDetails = new List<SubjectTestGroupTotal>();
                        while (oSqlDataReader.Read())
                        {
                            StudentWiseProgressReportSubjectTestGroupTotal oSubjectTestGroupTotal = new StudentWiseProgressReportSubjectTestGroupTotal();
                            if (oSqlDataReader["Test_Id"] != DBNull.Value)
                                oSubjectTestGroupTotal.SchoolWiseTestId = oSqlDataReader["Test_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                                oSubjectTestGroupTotal.ParentSubjectId = oSqlDataReader["Parent_Subject_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Name"] != DBNull.Value)
                                oSubjectTestGroupTotal.ParentSubjectName = oSqlDataReader["Parent_Subject_Name"].ToString();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oSubjectTestGroupTotal.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["ChildSubject_Marks_Total"] != DBNull.Value)
                                oSubjectTestGroupTotal.ChildSubjectMarksTotal = oSqlDataReader["ChildSubject_Marks_Total"].ToDecimal();
                            if (msTestIds.IsNullOrEmpty() && oSqlDataReader["Grade"] != DBNull.Value)
                                oSubjectTestGroupTotal.Grade = oSqlDataReader["Grade"].ToString();
                            if (oSqlDataReader["AverageMarks"] != DBNull.Value)
                                oSubjectTestGroupTotal.AverageMarks = oSqlDataReader["AverageMarks"].ToDecimal();
                            if (oSqlDataReader["OutOfMarks"] != DBNull.Value)
                                oSubjectTestGroupTotal.OutOfMarks = oSqlDataReader["OutOfMarks"].ToDecimal();
                            oStudentProgressReport.SubjectTestGroupTotalDetails.Add(oSubjectTestGroupTotal);
                        }
                    }

                    oStudentProgressReport.SubjectTestTypeGroupTotalDetails = GetSubjectTestTypeGroupTotalDetails(oSqlDataReader);
                    oStudentProgressReport.SubjectTestTypeDetails = GetSubjectTestTypeDetails(oSqlDataReader);
                    oStudentProgressReport.TestTypeDetails = GetTestTypeDetails(oSqlDataReader);
                    oStudentProgressReport.GradeDetails = GetGradeDetails(oSqlDataReader);
                    oStudentProgressReport.ExamStatusDetails = GetExamStatusDetails(oSqlDataReader);
                    oStudentProgressReport.DependentExamDetails = GetDependentExamDetails(oSqlDataReader);

                    return oStudentProgressReport;
                }
            }
        }

        /// <summary>
        /// This method is used to fill student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private StudentDetails GetStudentDetails(SqlDataReader aoSqlDataReader)
        {
            StudentDetails oStudentDetails = new StudentDetails();
            while (aoSqlDataReader.Read())
            {
                oStudentDetails = new StudentDetails()
                {
                    YearWiseStudentId = aoSqlDataReader["YearWise_Student_Id"].ToInt(),
                    StudentName = aoSqlDataReader["Student_Name"].ToString(),
                    StandardDivisionDetails = new MasterEntities.StandardDivisionMaster()
                    {
                        StandardId = aoSqlDataReader["Standard_Id"].ToInt(),
                        StandardName = aoSqlDataReader["Standard_Name"].ToString(),
                        DivisionName = aoSqlDataReader["Division_Name"].ToString(),
                        StandardDivisionId = aoSqlDataReader["Standard_Division_Id"].ToInt(),
                        IsPreprimaryStandard = aoSqlDataReader["IsPreprimaryStandard"].ToBool()
                    },
                    AcademicYear = aoSqlDataReader["Academic_Year"].ToString(),
                    RollNo = aoSqlDataReader["Roll_No"].ToInt(),
                    EnrolmentNumber = aoSqlDataReader["Enrolment_Number"].ToString(),
                    SchoolName = aoSqlDataReader["School_Name"].ToString(),
                    OrganizationName = aoSqlDataReader["School_Orgn_Name"].ToString(),
                    ShowOnlyGrades = aoSqlDataReader["ShowOnlyGrades"].ToString().Trim().ToBool(),
                    IsFailCriteriaNotApplicable = aoSqlDataReader["IsFailCriteriaNotApplicable"].ToString(),
                };
            }

            return oStudentDetails;
        }
        
        /// <summary>
        /// This method is used to fill subject details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<Subject> GetSubjectDetails(SqlDataReader aoSqlDataReader)
        {
            List<Subject> lstSubjctDetails = new List<Subject>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    ProgressReportSubujectDetails oSubject = new ProgressReportSubujectDetails();
                    if (aoSqlDataReader["ID_Num"] != DBNull.Value)
                        oSubject.Id = aoSqlDataReader["ID_Num"].ToInt();
                    if (aoSqlDataReader["Subject_Name"] != DBNull.Value)
                        oSubject.SubjectName = aoSqlDataReader["Subject_Name"].ToString();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oSubject.SubjectId = aoSqlDataReader["Subject_Id"].ToInt();
                    if (aoSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                        oSubject.ParentSubjectId = aoSqlDataReader["Parent_Subject_Id"].ToInt();
                    if (aoSqlDataReader["Total_Consideration"] != DBNull.Value)
                        oSubject.TotalConsideration = aoSqlDataReader["Total_Consideration"].ToString();
                    lstSubjctDetails.Add(oSubject);
                }
            }

            return lstSubjctDetails;
        }

        /// <summary>
        /// This method is used to fill exam details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<Exam> GetExamDetails(SqlDataReader aoSqlDataReader)
        {
            List<Exam> lstExamDetails = new List<Exam>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    Exam oExam = new Exam();
                    if (aoSqlDataReader["Test_Id"] != DBNull.Value)
                        oExam.SchoolWiseTestId = aoSqlDataReader["Test_Id"].ToInt();
                    if (aoSqlDataReader["Test_Name"] != DBNull.Value)
                        oExam.SchoolWiseTestName = aoSqlDataReader["Test_Name"].ToString();
                    if (aoSqlDataReader["Original_SchoolWise_Test_Id"] != DBNull.Value)
                        oExam.OriginalShcoolWiseTestId = aoSqlDataReader["Original_SchoolWise_Test_Id"].ToInt();
                        lstExamDetails.Add(oExam);
                }
            }

            return lstExamDetails;
        }

        /// <summary>
        /// This method is used to fill marks assignement details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<MarkAssignment> GetMarkAssignmentDetails(SqlDataReader aoSqlDataReader)
        {
            List<MarkAssignment> lstMarkAssignment = new List<MarkAssignment>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    MarkAssignment oMarkAssignment = new MarkAssignment();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oMarkAssignment.SubjectId = aoSqlDataReader["Subject_Id"].ToInt();
                    if (aoSqlDataReader["Marks"] != DBNull.Value)
                        oMarkAssignment.Marks = aoSqlDataReader["Marks"].ToString();
                    if (aoSqlDataReader["SchoolWise_Test_Id"] != DBNull.Value)
                        oMarkAssignment.SchoolWiseTestId = aoSqlDataReader["SchoolWise_Test_Id"].ToInt();
                    if (aoSqlDataReader["Original_SchoolWise_Test_Id"] != DBNull.Value)
                        oMarkAssignment.OriginalShcoolWiseTestId = aoSqlDataReader["Original_SchoolWise_Test_Id"].ToInt();
                    if (aoSqlDataReader["SchoolWise_Test_Name"] != DBNull.Value)
                        oMarkAssignment.SchoolWiseTestName = aoSqlDataReader["SchoolWise_Test_Name"].ToString();
                    if (aoSqlDataReader["Subject_Name"] != DBNull.Value)
                        oMarkAssignment.SubjectName = aoSqlDataReader["Subject_Name"].ToString();
                    if (aoSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                        oMarkAssignment.TotalMarksScored = aoSqlDataReader["Total_Marks_Scored"].ToDecimal();
                    if (aoSqlDataReader["Subject_Total_Marks"] != DBNull.Value)
                        oMarkAssignment.SubjectTotalMarks = aoSqlDataReader["Subject_Total_Marks"].ToInt();
                    if (aoSqlDataReader["Passing_Total_Marks"] != DBNull.Value)
                        oMarkAssignment.PassingTotalMarks = aoSqlDataReader["Passing_Total_Marks"].ToDecimal();
                    if (aoSqlDataReader["Subject_Total"] != DBNull.Value)
                        oMarkAssignment.SubjectTotal = aoSqlDataReader["Subject_Total"].ToString();
                    if (aoSqlDataReader["Grade_Or_Marks"] != DBNull.Value)
                        oMarkAssignment.GradeOrMarks = aoSqlDataReader["Grade_Or_Marks"].ToString();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        oMarkAssignment.TestTypeId = aoSqlDataReader["TestType_Id"].ToInt();
                    if (aoSqlDataReader["Marks_Scored"] != DBNull.Value)
                        oMarkAssignment.MarksScored = aoSqlDataReader["Marks_Scored"].ToDecimal();
                    if (aoSqlDataReader["TestType_Name"] != DBNull.Value)
                        oMarkAssignment.TestTypeName = aoSqlDataReader["TestType_Name"].ToString();
                    if (aoSqlDataReader["ShortenTestType_Name"] != DBNull.Value)
                        oMarkAssignment.ShortenTestTypeName = aoSqlDataReader["ShortenTestType_Name"].ToString();
                    if (aoSqlDataReader["Grade"] != DBNull.Value)
                        oMarkAssignment.Grade = aoSqlDataReader["Grade"].ToString();
                    if (aoSqlDataReader["TotalGrade"] != DBNull.Value)
                        oMarkAssignment.TotalGrade = aoSqlDataReader["TotalGrade"].ToString();
                    if (aoSqlDataReader["TestType_Total_Marks"] != DBNull.Value)
                        oMarkAssignment.TestTypeTotalMarks = aoSqlDataReader["TestType_Total_Marks"].ToInt();
                    if (aoSqlDataReader["TestType_Passing_Marks"] != DBNull.Value)
                        oMarkAssignment.TestTypePassingMarks = aoSqlDataReader["TestType_Passing_Marks"].ToDecimal();
                    if (aoSqlDataReader["Is_Absent"] != DBNull.Value)
                        oMarkAssignment.IsAbsent = aoSqlDataReader["Is_Absent"].ToString();
                    if (aoSqlDataReader["SchoolWise_Student_Test_Marks_Id"] != DBNull.Value)
                        oMarkAssignment.SchoolWiseStudentTestId = aoSqlDataReader["SchoolWise_Student_Test_Marks_Id"].ToInt();
                    if (aoSqlDataReader["TestWise_Subject_Marks_Id"] != DBNull.Value)
                        oMarkAssignment.TestWiseSubjectId = aoSqlDataReader["TestWise_Subject_Marks_Id"].ToInt();
                    if (aoSqlDataReader["ConsiderExamStatus"] != DBNull.Value)
                        oMarkAssignment.ConsiderExamStatus = aoSqlDataReader["ConsiderExamStatus"].ToString();
                    if (aoSqlDataReader["ConsiderInResult"] != DBNull.Value)
                        oMarkAssignment.ConsiderInResult = aoSqlDataReader["ConsiderInResult"].ToString();
                    if (aoSqlDataReader["ShowOnlyGrades"] != DBNull.Value)
                        oMarkAssignment.ShowOnlyGrades = aoSqlDataReader["ShowOnlyGrades"].ToBool();
                    if (aoSqlDataReader["AllowDecimal"] != DBNull.Value)
                        oMarkAssignment.AllowDecimal = aoSqlDataReader["AllowDecimal"].ToBool();
                    if (aoSqlDataReader["Is_CoCurricularActivity"] != DBNull.Value)
                        oMarkAssignment.IsCoCurricularActivity = aoSqlDataReader["Is_CoCurricularActivity"].ToBool();
                    lstMarkAssignment.Add(oMarkAssignment);
                }
            }

            return lstMarkAssignment;
        }

        /// <summary>
        /// This method is used to fill exam wise percentage details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<ExamWisePercentage> GetExamWisePercentageDetails(SqlDataReader aoSqlDataReader)
        {
            List<ExamWisePercentage> lstExamWisePercentage = new List<ExamWisePercentage>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    StudentWiseProgressReportExamWisePercentage oExamWisePercentage = new StudentWiseProgressReportExamWisePercentage();
                    if (aoSqlDataReader["SchoolWise_Test_Id"] != DBNull.Value)
                        oExamWisePercentage.SchoolWiseTestId = aoSqlDataReader["SchoolWise_Test_Id"].ToInt();
                    if (aoSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                        oExamWisePercentage.TotalMarksScored = aoSqlDataReader["Total_Marks_Scored"].ToDecimal();
                    if (aoSqlDataReader["Subjects_Total_Marks"] != DBNull.Value)
                        oExamWisePercentage.SubjectTotalMarks = aoSqlDataReader["Subjects_Total_Marks"].ToInt();
                    if (aoSqlDataReader["Percentage"] != DBNull.Value)
                        oExamWisePercentage.Percentage = aoSqlDataReader["Percentage"].ToDecimal();
                    if (aoSqlDataReader["Grade_Name"] != DBNull.Value)
                        oExamWisePercentage.Grade = aoSqlDataReader["Grade_Name"].ToString();
                    if (aoSqlDataReader["Grade_id"] != DBNull.Value)
                        oExamWisePercentage.GradeId = aoSqlDataReader["Grade_id"].ToInt();
                    if (aoSqlDataReader["Result"] != DBNull.Value)
                        oExamWisePercentage.Result = aoSqlDataReader["Result"].ToString();
                    if (aoSqlDataReader["rank"] != DBNull.Value)
                        oExamWisePercentage.Rank = aoSqlDataReader["rank"].ToInt();
                    lstExamWisePercentage.Add(oExamWisePercentage);
                }
            }

            return lstExamWisePercentage;
        }

        /// <summary>
        /// This method is used to fill subject test group total details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<SubjectTestGroupTotal> GetSubjectTestGroupTotalDetails(SqlDataReader aoSqlDataReader)
        {
            List<SubjectTestGroupTotal> lstSubjectTestGroupTotal = new List<SubjectTestGroupTotal>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    StudentWiseProgressReportSubjectTestGroupTotal oSubjectTestGroupTotal = new StudentWiseProgressReportSubjectTestGroupTotal();
                    if (aoSqlDataReader["Test_Id"] != DBNull.Value)
                        oSubjectTestGroupTotal.SchoolWiseTestId = aoSqlDataReader["Test_Id"].ToInt();
                    if (aoSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                        oSubjectTestGroupTotal.ParentSubjectId = aoSqlDataReader["Parent_Subject_Id"].ToInt();
                    if (aoSqlDataReader["Parent_Subject_Name"] != DBNull.Value)
                        oSubjectTestGroupTotal.ParentSubjectName = aoSqlDataReader["Parent_Subject_Name"].ToString();
                    if (aoSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                        oSubjectTestGroupTotal.TotalMarksScored = aoSqlDataReader["Total_Marks_Scored"].ToDecimal();
                    if (aoSqlDataReader["ChildSubject_Marks_Total"] != DBNull.Value)
                        oSubjectTestGroupTotal.ChildSubjectMarksTotal = aoSqlDataReader["ChildSubject_Marks_Total"].ToDecimal();
                    if (aoSqlDataReader["Grade"] != DBNull.Value)
                        oSubjectTestGroupTotal.Grade = aoSqlDataReader["Grade"].ToString();

                    if (aoSqlDataReader["AverageMarks"] != DBNull.Value)
                        oSubjectTestGroupTotal.AverageMarks = aoSqlDataReader["AverageMarks"].ToDecimal();
                    if (aoSqlDataReader["OutOfMarks"] != DBNull.Value)
                        oSubjectTestGroupTotal.OutOfMarks = aoSqlDataReader["OutOfMarks"].ToDecimal();

                    lstSubjectTestGroupTotal.Add(oSubjectTestGroupTotal);
                }
            }

            return lstSubjectTestGroupTotal;
        }

        /// <summary>
        /// This method is used to fill subejct test type group total details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<SubjectTestTypeGroupTotal> GetSubjectTestTypeGroupTotalDetails(SqlDataReader aoSqlDataReader)
        {
            List<SubjectTestTypeGroupTotal> lstSubjectTestTypeGroupTotal = new List<SubjectTestTypeGroupTotal>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    SubjectTestTypeGroupTotal oSubjectTestTypeGroupTotal = new SubjectTestTypeGroupTotal();
                    if (aoSqlDataReader["Test_Id"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.SchoolWiseTestId = aoSqlDataReader["Test_Id"].ToInt();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.TestTypeId = aoSqlDataReader["TestType_Id"].ToInt();
                    if (aoSqlDataReader["TestTypeSort_Order"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.TestTypeSortOrder = aoSqlDataReader["TestTypeSort_Order"].ToInt();
                    if (aoSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.ParentSubjectId = aoSqlDataReader["Parent_Subject_Id"].ToInt();
                    if (aoSqlDataReader["TestType_Total_Marks_Scored"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.TestTypeTotalMarksScored = aoSqlDataReader["TestType_Total_Marks_Scored"].ToDecimal();
                    if (aoSqlDataReader["TestType_Total_Marks"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.TestTypeTotalMarks = aoSqlDataReader["TestType_Total_Marks"].ToDecimal();
                    if (aoSqlDataReader["Grade"] != DBNull.Value)
                        oSubjectTestTypeGroupTotal.Grade = aoSqlDataReader["Grade"].ToString();
                  
                    
                    lstSubjectTestTypeGroupTotal.Add(oSubjectTestTypeGroupTotal);
                }
            }

            return lstSubjectTestTypeGroupTotal;
        }

        /// <summary>
        /// This method is used to get subject test type details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<SubjectTestType> GetSubjectTestTypeDetails(SqlDataReader aoSqlDataReader)
        {
            List<SubjectTestType> lstSubjectTestType = new List<SubjectTestType>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    SubjectTestType oSubjectTestType = new SubjectTestType();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oSubjectTestType.SubjectId = aoSqlDataReader["Subject_Id"].ToInt();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        oSubjectTestType.TestTypeId = aoSqlDataReader["TestType_Id"].ToInt();
                    if (aoSqlDataReader["ShortenTestType_Name"] != DBNull.Value)
                        oSubjectTestType.ShortenTestTypeName = aoSqlDataReader["ShortenTestType_Name"].ToString();
                    if (aoSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                        oSubjectTestType.TotalMarksScored = aoSqlDataReader["Total_Marks_Scored"].ToDecimal();
                    if (aoSqlDataReader["TestTypeSort_Order"] != DBNull.Value)
                        oSubjectTestType.TestTypeSortOrder = aoSqlDataReader["TestTypeSort_Order"].ToInt();
                    lstSubjectTestType.Add(oSubjectTestType);
                }
            }

            return lstSubjectTestType;
        }

        /// <summary>
        /// This method is used to get test type details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<TestType> GetTestTypeDetails(SqlDataReader aoSqlDataReader)
        {
            List<TestType> lstTestType = new List<TestType>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    TestType oTestType = new TestType();
                    if (aoSqlDataReader["TestType_Id"] != DBNull.Value)
                        oTestType.TestTypeId = aoSqlDataReader["TestType_Id"].ToInt();
                    if (aoSqlDataReader["TestType_Name"] != DBNull.Value)
                        oTestType.TestTypeName = aoSqlDataReader["TestType_Name"].ToString();
                    if (aoSqlDataReader["ShortenTestType_Name"] != DBNull.Value)
                        oTestType.ShortenTestTypeName = aoSqlDataReader["ShortenTestType_Name"].ToString();
                    if (aoSqlDataReader["TestTypeSort_Order"] != DBNull.Value)
                        oTestType.TestTypeSortOrder = aoSqlDataReader["TestTypeSort_Order"].ToInt();
                    lstTestType.Add(oTestType);
                }
            }

            return lstTestType;
        }

        /// <summary>
        /// This method is used to get grade details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<Grade> GetGradeDetails(SqlDataReader aoSqlDataReader)
        {
            List<Grade> lstGrade = new List<Grade>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    Grade oGrade = new Grade();
                    if (aoSqlDataReader["Marks_Grades_Configuration_Detail_ID"] != DBNull.Value)
                        oGrade.GradeId = aoSqlDataReader["Marks_Grades_Configuration_Detail_ID"].ToInt();
                    if (aoSqlDataReader["Grade_Name"] != DBNull.Value)
                        oGrade.GradeName = aoSqlDataReader["Grade_Name"].ToString();
                    if (aoSqlDataReader["Remarks"] != DBNull.Value)
                        oGrade.Remarks = aoSqlDataReader["Remarks"].ToString();
                    if (aoSqlDataReader["IsForCoCurricularSubjects"] != DBNull.Value)
                        oGrade.IsForCoCurricularSubjects = aoSqlDataReader["IsForCoCurricularSubjects"].ToBool();
                    lstGrade.Add(oGrade);
                }
            }

            return lstGrade;
        }

        /// <summary>
        /// This method is used to fill exam status details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<ExamStatus> GetExamStatusDetails(SqlDataReader aoSqlDataReader)
        {
            List<ExamStatus> lstExamStatus = new List<ExamStatus>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    ExamStatus oExamStatus = new ExamStatus();
                    if (aoSqlDataReader["DisplayName"] != DBNull.Value)
                        oExamStatus.DisplayName = aoSqlDataReader["DisplayName"].ToString();
                    if (aoSqlDataReader["DisplayValue"] != DBNull.Value)
                        oExamStatus.DisplayValue = aoSqlDataReader["DisplayValue"].ToString();
                    if (aoSqlDataReader["ShortName"] != DBNull.Value)
                        oExamStatus.ShortName = aoSqlDataReader["ShortName"].ToString();
                    if (aoSqlDataReader["ForeColor"] != DBNull.Value)
                        oExamStatus.ForeColor = aoSqlDataReader["ForeColor"].ToString();
                    if (aoSqlDataReader["BackColor"] != DBNull.Value)
                        oExamStatus.BackColor = aoSqlDataReader["BackColor"].ToString();
                    lstExamStatus.Add(oExamStatus);
                }
            }

            return lstExamStatus;
        }

        /// <summary>
        /// This method is used to fill dependant exam details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<DependentExam> GetDependentExamDetails(SqlDataReader aoSqlDataReader)
        {
            List<DependentExam> lstDependentExam = new List<DependentExam>();
            if (aoSqlDataReader.NextResult())
            {
                while (aoSqlDataReader.Read())
                {
                    DependentExam oDependentExam = new DependentExam();
                    if (aoSqlDataReader["ParentExamId"] != DBNull.Value)
                        oDependentExam.ParentExamId = aoSqlDataReader["ParentExamId"].ToInt();
                    if (aoSqlDataReader["ExamName"] != DBNull.Value)
                        oDependentExam.ExamName = aoSqlDataReader["ExamName"].ToString();
                    if (aoSqlDataReader["DependentExamId"] != DBNull.Value)
                        oDependentExam.DependentExamId = aoSqlDataReader["DependentExamId"].ToInt();
                    lstDependentExam.Add(oDependentExam);
                }
            }

            return lstDependentExam;
        }

        public DataSet GetAllStudentProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("iStdDivId", aiStdDivId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("[usp_GetAllStudentsProgressSheet]");
            }
        }

        /// <summary>
        /// This method is used to get progress report of a class in xml format.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiPageCount"></param>
        /// <param name="aiTestID"></param>
        /// <returns></returns>
        public DataSet GetAllStudentProgressResult(int aiSchoolId, int aiAcademicYrId, int aiStdDivId, int aiStartIndex, int aiPageCount, int aiTestID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("iStdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageCount", aiPageCount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestID", aiTestID, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("[usp_GetAllStudentsTestProgressSheet]");
            }
        }

        /// <summary>
        /// This method is used to get student final result details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public StudentProgressReport GetStudentResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", aiStudentId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_StudentResult"))
                {
                    StudentProgressReport oStudentProgressReport = new StudentProgressReport();
                    oStudentProgressReport.StudentDetails = GetStudentDetails(oSqlDataReader);

                    oStudentProgressReport.SubjectDetails = new List<Subject>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultSubjectDetails oSubject = new FinalResultSubjectDetails();
                            if (oSqlDataReader["ID_Num"] != DBNull.Value)
                                oSubject.Id = oSqlDataReader["ID_Num"].ToInt();
                            if (oSqlDataReader["Subject_Name"] != DBNull.Value)
                                oSubject.SubjectName = oSqlDataReader["Subject_Name"].ToString();
                            if (oSqlDataReader["Subject_Id"] != DBNull.Value)
                                oSubject.SubjectId = oSqlDataReader["Subject_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                                oSubject.ParentSubjectId = oSqlDataReader["Parent_Subject_Id"].ToInt();
                            if (oSqlDataReader["Total_Consideration"] != DBNull.Value)
                                oSubject.TotalConsideration = oSqlDataReader["Total_Consideration"].ToString();
                            if (oSqlDataReader["Grace_Marks"] != DBNull.Value)
                                oSubject.GraceMarks = oSqlDataReader["Grace_Marks"].ToInt();
                            if (oSqlDataReader["Grade_Or_Marks"] != DBNull.Value)
                                oSubject.GradeOrMarks = oSqlDataReader["Grade_Or_Marks"].ToString();
                            if (oSqlDataReader["Marks_Scored"] != DBNull.Value)
                                oSubject.MarksScored = oSqlDataReader["Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subject_Total_Marks"] != DBNull.Value)
                                oSubject.SubjectTotalMarks = oSqlDataReader["Subject_Total_Marks"].ToInt();
                            if (oSqlDataReader["Grade"] != DBNull.Value)
                                oSubject.Grade = oSqlDataReader["Grade"].ToString();
                            if (oSqlDataReader["Subject_MaxGrace"] != DBNull.Value)
                                oSubject.SubjectMaxGrace = oSqlDataReader["Subject_MaxGrace"].ToInt();
                            if (oSqlDataReader["Standard_MaxGrace"] != DBNull.Value)
                                oSubject.StandardMaxGrace = oSqlDataReader["Standard_MaxGrace"].ToInt();
                            if (oSqlDataReader["IsAbsent"] != DBNull.Value)
                                oSubject.IsAbsent = oSqlDataReader["IsAbsent"].ToBool();
                            if (oSqlDataReader["IsThirdLanguage"] != DBNull.Value)
                                oSubject.IsThirdLanguage = oSqlDataReader["IsThirdLanguage"].ToBool();
                            oStudentProgressReport.SubjectDetails.Add(oSubject);
                        }
                    }

                    oStudentProgressReport.ExamWisePercentageDetails = new List<ExamWisePercentage>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultExamWisePercentage oExamWisePercentage = new FinalResultExamWisePercentage();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oExamWisePercentage.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subjects_Total_Marks"] != DBNull.Value)
                                oExamWisePercentage.SubjectTotalMarks = oSqlDataReader["Subjects_Total_Marks"].ToInt();
                            if (oSqlDataReader["Percentage"] != DBNull.Value)
                                oExamWisePercentage.Percentage = oSqlDataReader["Percentage"].ToDecimal();
                            if (oSqlDataReader["Grade_Name"] != DBNull.Value)
                                oExamWisePercentage.Grade = oSqlDataReader["Grade_Name"].ToString();
                            if (oSqlDataReader["Grade_id"] != DBNull.Value)
                                oExamWisePercentage.GradeId = oSqlDataReader["Grade_id"].ToInt();
                            if (oSqlDataReader["Result"] != DBNull.Value)
                                oExamWisePercentage.Result = oSqlDataReader["Result"].ToString();
                            if (oSqlDataReader["rank"] != DBNull.Value)
                                oExamWisePercentage.Rank = oSqlDataReader["rank"].ToInt();
                            if (oSqlDataReader["Student_Id"] != DBNull.Value)
                                oExamWisePercentage.StudentId = oSqlDataReader["Student_Id"].ToInt();

                            oStudentProgressReport.ExamWisePercentageDetails.Add(oExamWisePercentage);
                        }
                    }

                    oStudentProgressReport.SubjectTestGroupTotalDetails = new List<SubjectTestGroupTotal>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultSubjectTestGroupTotal oSubjectTestGroupTotal = new FinalResultSubjectTestGroupTotal();
                            if (oSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                                oSubjectTestGroupTotal.ParentSubjectId = oSqlDataReader["Parent_Subject_Id"].ToInt();
                            if (oSqlDataReader["Original_Subject_Id"] != DBNull.Value)
                                oSubjectTestGroupTotal.OriginalSubjectId = oSqlDataReader["Original_Subject_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Name"] != DBNull.Value)
                                oSubjectTestGroupTotal.ParentSubjectName = oSqlDataReader["Parent_Subject_Name"].ToString();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oSubjectTestGroupTotal.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Grace_Marks"] != DBNull.Value)
                                oSubjectTestGroupTotal.GraceMarks = oSqlDataReader["Grace_Marks"].ToInt();
                            if (oSqlDataReader["Subject_MaxGrace"] != DBNull.Value)
                                oSubjectTestGroupTotal.SubjectMaxGrace = oSqlDataReader["Subject_MaxGrace"].ToInt();
                            if (oSqlDataReader["Standard_MaxGrace"] != DBNull.Value)
                                oSubjectTestGroupTotal.StandardMaxGrace = oSqlDataReader["Standard_MaxGrace"].ToInt();
                            if (oSqlDataReader["Grade_Or_Marks"] != DBNull.Value)
                                oSubjectTestGroupTotal.GradeOrMarks = oSqlDataReader["Grade_Or_Marks"].ToString();
                            if (oSqlDataReader["Subject_Total_Marks"] != DBNull.Value)
                                oSubjectTestGroupTotal.SubjectTotalMarks = oSqlDataReader["Subject_Total_Marks"].ToInt();
                            if (oSqlDataReader["Percentage"] != DBNull.Value)
                                oSubjectTestGroupTotal.Percentage = oSqlDataReader["Percentage"].ToDecimal();

                            if (oSqlDataReader["AverageMarks"] != DBNull.Value)
                                oSubjectTestGroupTotal.AverageMarks = oSqlDataReader["AverageMarks"].ToDecimal();
                            if (oSqlDataReader["OutOfMarks"] != DBNull.Value)
                                oSubjectTestGroupTotal.OutOfMarks = oSqlDataReader["OutOfMarks"].ToDecimal();

                            oStudentProgressReport.SubjectTestGroupTotalDetails.Add(oSubjectTestGroupTotal);
                        }
                    }

                    oStudentProgressReport.GradeDetails = new List<Grade>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultGrade oGrade = new FinalResultGrade();
                            if (oSqlDataReader["Marks_Grades_Configuration_Detail_ID"] != DBNull.Value)
                                oGrade.GradeId = oSqlDataReader["Marks_Grades_Configuration_Detail_ID"].ToInt();
                            if (oSqlDataReader["Grade"] != DBNull.Value)
                                oGrade.GradeName = oSqlDataReader["Grade"].ToString();
                            if (oSqlDataReader["Remarks"] != DBNull.Value)
                                oGrade.Remarks = oSqlDataReader["Remarks"].ToString();
                            if (oSqlDataReader["Range"] != DBNull.Value)
                                oGrade.Range = oSqlDataReader["Range"].ToString();
                            oStudentProgressReport.GradeDetails.Add(oGrade);
                        }
                    }

                    if (oSqlDataReader.NextResult())
                        while (oSqlDataReader.Read())
                            if (oSqlDataReader["GraceMarkMessage"] != DBNull.Value)
                                oStudentProgressReport.GraceMarksMessage = oSqlDataReader["GraceMarkMessage"].ToString();

                    return oStudentProgressReport;
                }
            }
        }
        /// <summary>
        /// This method is used to get student result with grace marks.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public StudentProgressReport GetStudentGraceResult(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iWithGrace", 1, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_StudentResult"))
                {
                    StudentProgressReport oStudentProgressReport = new StudentProgressReport();
                    oStudentProgressReport.StudentDetails = GetStudentDetails(oSqlDataReader);

                    oStudentProgressReport.SubjectDetails = new List<Subject>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultSubjectDetails oSubject = new FinalResultSubjectDetails();
                            if (oSqlDataReader["ID_Num"] != DBNull.Value)
                                oSubject.Id = oSqlDataReader["ID_Num"].ToInt();
                            if (oSqlDataReader["Subject_Name"] != DBNull.Value)
                                oSubject.SubjectName = oSqlDataReader["Subject_Name"].ToString();
                            if (oSqlDataReader["Subject_Id"] != DBNull.Value)
                                oSubject.SubjectId = oSqlDataReader["Subject_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                                oSubject.ParentSubjectId = oSqlDataReader["Parent_Subject_Id"].ToInt();
                            if (oSqlDataReader["Total_Consideration"] != DBNull.Value)
                                oSubject.TotalConsideration = oSqlDataReader["Total_Consideration"].ToString();
                            if (oSqlDataReader["Grace_Marks"] != DBNull.Value)
                                oSubject.GraceMarks = oSqlDataReader["Grace_Marks"].ToInt();
                            if (oSqlDataReader["Grade_Or_Marks"] != DBNull.Value)
                                oSubject.GradeOrMarks = oSqlDataReader["Grade_Or_Marks"].ToString();
                            if (oSqlDataReader["Marks_Scored"] != DBNull.Value)
                                oSubject.MarksScored = oSqlDataReader["Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subject_Total_Marks"] != DBNull.Value)
                                oSubject.SubjectTotalMarks = oSqlDataReader["Subject_Total_Marks"].ToInt();
                            if (oSqlDataReader["Grade"] != DBNull.Value)
                                oSubject.Grade = oSqlDataReader["Grade"].ToString();
                            if (oSqlDataReader["Subject_MaxGrace"] != DBNull.Value)
                                oSubject.SubjectMaxGrace = oSqlDataReader["Subject_MaxGrace"].ToInt();
                            if (oSqlDataReader["Standard_MaxGrace"] != DBNull.Value)
                                oSubject.StandardMaxGrace = oSqlDataReader["Standard_MaxGrace"].ToInt();
                            if (oSqlDataReader["IsAbsent"] != DBNull.Value)
                                oSubject.IsAbsent = oSqlDataReader["IsAbsent"].ToBool();
                            if (oSqlDataReader["IsThirdLanguage"] != DBNull.Value)
                                oSubject.IsThirdLanguage = oSqlDataReader["IsThirdLanguage"].ToBool();
                            oStudentProgressReport.SubjectDetails.Add(oSubject);
                        }
                    }

                    oStudentProgressReport.ExamWisePercentageDetails = new List<ExamWisePercentage>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultExamWisePercentage oExamWisePercentage = new FinalResultExamWisePercentage();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oExamWisePercentage.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subjects_Total_Marks"] != DBNull.Value)
                                oExamWisePercentage.SubjectTotalMarks = oSqlDataReader["Subjects_Total_Marks"].ToInt();
                            if (oSqlDataReader["Percentage"] != DBNull.Value)
                                oExamWisePercentage.Percentage = oSqlDataReader["Percentage"].ToDecimal();
                            if (oSqlDataReader["Grade_Name"] != DBNull.Value)
                                oExamWisePercentage.Grade = oSqlDataReader["Grade_Name"].ToString();
                            if (oSqlDataReader["Grade_id"] != DBNull.Value)
                                oExamWisePercentage.GradeId = oSqlDataReader["Grade_id"].ToInt();
                            if (oSqlDataReader["Result"] != DBNull.Value)
                                oExamWisePercentage.Result = oSqlDataReader["Result"].ToString();
                            if (oSqlDataReader["rank"] != DBNull.Value)
                                oExamWisePercentage.Rank = oSqlDataReader["rank"].ToInt();
                            if (oSqlDataReader["Student_Id"] != DBNull.Value)
                                oExamWisePercentage.StudentId = oSqlDataReader["Student_Id"].ToInt();

                            oStudentProgressReport.ExamWisePercentageDetails.Add(oExamWisePercentage);
                        }
                    }

                    oStudentProgressReport.SubjectTestGroupTotalDetails = new List<SubjectTestGroupTotal>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultSubjectTestGroupTotal oSubjectTestGroupTotal = new FinalResultSubjectTestGroupTotal();
                            if (oSqlDataReader["Parent_Subject_Id"] != DBNull.Value)
                                oSubjectTestGroupTotal.ParentSubjectId = oSqlDataReader["Parent_Subject_Id"].ToInt();
                            if (oSqlDataReader["Original_Subject_Id"] != DBNull.Value)
                                oSubjectTestGroupTotal.OriginalSubjectId = oSqlDataReader["Original_Subject_Id"].ToInt();
                            if (oSqlDataReader["Parent_Subject_Name"] != DBNull.Value)
                                oSubjectTestGroupTotal.ParentSubjectName = oSqlDataReader["Parent_Subject_Name"].ToString();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oSubjectTestGroupTotal.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Grace_Marks"] != DBNull.Value)
                                oSubjectTestGroupTotal.GraceMarks = oSqlDataReader["Grace_Marks"].ToInt();
                            if (oSqlDataReader["Subject_MaxGrace"] != DBNull.Value)
                                oSubjectTestGroupTotal.SubjectMaxGrace = oSqlDataReader["Subject_MaxGrace"].ToInt();
                            if (oSqlDataReader["Standard_MaxGrace"] != DBNull.Value)
                                oSubjectTestGroupTotal.StandardMaxGrace = oSqlDataReader["Standard_MaxGrace"].ToInt();
                            if (oSqlDataReader["Grade_Or_Marks"] != DBNull.Value)
                                oSubjectTestGroupTotal.GradeOrMarks = oSqlDataReader["Grade_Or_Marks"].ToString();
                            if (oSqlDataReader["Subject_Total_Marks"] != DBNull.Value)
                                oSubjectTestGroupTotal.SubjectTotalMarks = oSqlDataReader["Subject_Total_Marks"].ToInt();
                            if (oSqlDataReader["Percentage"] != DBNull.Value)
                                oSubjectTestGroupTotal.Percentage = oSqlDataReader["Percentage"].ToDecimal();

                            if (oSqlDataReader["AverageMarks"] != DBNull.Value)
                                oSubjectTestGroupTotal.AverageMarks = oSqlDataReader["AverageMarks"].ToDecimal();
                            if (oSqlDataReader["OutOfMarks"] != DBNull.Value)
                                oSubjectTestGroupTotal.OutOfMarks = oSqlDataReader["OutOfMarks"].ToDecimal();

                            oStudentProgressReport.SubjectTestGroupTotalDetails.Add(oSubjectTestGroupTotal);
                        }
                    }

                    oStudentProgressReport.GradeDetails = new List<Grade>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            FinalResultGrade oGrade = new FinalResultGrade();
                            if (oSqlDataReader["Marks_Grades_Configuration_Detail_ID"] != DBNull.Value)
                                oGrade.GradeId = oSqlDataReader["Marks_Grades_Configuration_Detail_ID"].ToInt();
                            if (oSqlDataReader["Grade"] != DBNull.Value)
                                oGrade.GradeName = oSqlDataReader["Grade"].ToString();
                            if (oSqlDataReader["Remarks"] != DBNull.Value)
                                oGrade.Remarks = oSqlDataReader["Remarks"].ToString();
                            if (oSqlDataReader["Range"] != DBNull.Value)
                                oGrade.Range = oSqlDataReader["Range"].ToString();
                            oStudentProgressReport.GradeDetails.Add(oGrade);
                        }
                    }

                    if (oSqlDataReader.NextResult())
                        while (oSqlDataReader.Read())
                            if (oSqlDataReader["Grace_Marks"] != DBNull.Value)
                                oStudentProgressReport.GraceMarks = oSqlDataReader["Grace_Marks"].ToInt();

                    return oStudentProgressReport;
                }
            }
        }
        public DataSet GenerateAllStudentsResult(int aiSchoolId, int aiAcademicYrId, int aiStdDivId, int aiUserId, char acUseAvarageFinalResult)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iInsertedBy_Id", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("cUseAvarageFinalResult", acUseAvarageFinalResult, SqlDbType.Char);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GenerateAllStudentsResult",true);
            }
        }

        /// <summary>
        /// This method is used to set annual result.
        /// </summary>
        /// <param name="aiUpdated_By_id"></param>
        /// <param name="asStudentMarkDetails"></param>
        public static void UpdateAnnualResultGraceMarks(int iStudentId, int aiUpdated_By_id, string asStudentMarkDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AnnualResultGrace", asStudentMarkDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("iStudentId", iStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iUpdatedById", aiUpdated_By_id, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateGraceToAnnualResult");
            }
        }

        /// <summary>
        /// This method is used to get marks assignemnt details for student wise marks assignment.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrID"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public StudentProgressReport GetMarksDetailsForExamwiseStudentMarksAssignment(int aiSchoolId, int aiAcademicYrID, int aiStudentId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYrID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iUserId", aiUserId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMarksDetailsForExamwiseStudentMarksAssignment"))
                {
                    StudentProgressReport oStudentProgressReport = new StudentProgressReport();
                    StudentWiseProgressReportStudentDetails oStudentWiseProgressReportStudentDetails = new StudentWiseProgressReportStudentDetails();
                    while (oSqlDataReader.Read())
                    {
                        oStudentWiseProgressReportStudentDetails = new StudentWiseProgressReportStudentDetails()
                        {
                            YearWiseStudentId = oSqlDataReader["YearWise_Student_Id"].ToInt(),
                            StudentName = oSqlDataReader["Student_Name"].ToString(),
                            StandardDivisionDetails = new MasterEntities.StandardDivisionMaster()
                            {
                                StandardId = oSqlDataReader["Standard_Id"].ToInt(),
                                StandardName = oSqlDataReader["Standard_Name"].ToString(),
                                DivisionName = oSqlDataReader["Division_Name"].ToString(),
                                StandardDivisionId = oSqlDataReader["Standard_Division_Id"].ToInt(),
                                IsPreprimaryStandard = oSqlDataReader["IsPreprimaryStandard"].ToBool()
                            },
                            AcademicYear = oSqlDataReader["Academic_Year"].ToString(),
                            RollNo = oSqlDataReader["Roll_No"].ToInt(),
                            EnrolmentNumber = oSqlDataReader["Enrolment_Number"].ToString(),
                            SchoolName = oSqlDataReader["School_Name"].ToString(),
                            OrganizationName = oSqlDataReader["School_Orgn_Name"].ToString(),
                            ShowOnlyGrades = oSqlDataReader["ShowOnlyGrades"].ToString().Trim().ToBool(),
                            IsFailCriteriaNotApplicable = oSqlDataReader["IsFailCriteriaNotApplicable"].ToString(),
                        };

                        if (oSqlDataReader["IsGradesStandard"] != DBNull.Value)
                            oStudentWiseProgressReportStudentDetails.IsGradesStandard = oSqlDataReader["IsGradesStandard"].ToBool();
                        if (oSqlDataReader["UserId"] != DBNull.Value)
                            oStudentWiseProgressReportStudentDetails.UserId = oSqlDataReader["UserId"].ToInt();
                    }

                    oStudentProgressReport.StudentDetails = oStudentWiseProgressReportStudentDetails;

                    oStudentProgressReport.SubjectDetails = GetSubjectDetails(oSqlDataReader);
                    oStudentProgressReport.ExamDetails = GetExamDetails(oSqlDataReader);

                    oStudentProgressReport.MarkAssignmentDetails = new List<MarkAssignment>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            StudentWiseProgressReportMarkAssignment oMarkAssignment = new StudentWiseProgressReportMarkAssignment();
                            if (oSqlDataReader["Subject_Id"] != DBNull.Value)
                                oMarkAssignment.SubjectId = oSqlDataReader["Subject_Id"].ToInt();
                            if (oSqlDataReader["Marks"] != DBNull.Value)
                                oMarkAssignment.Marks = oSqlDataReader["Marks"].ToString();
                            if (oSqlDataReader["SchoolWise_Test_Id"] != DBNull.Value)
                                oMarkAssignment.SchoolWiseTestId = oSqlDataReader["SchoolWise_Test_Id"].ToInt();
                            if (oSqlDataReader["Original_SchoolWise_Test_Id"] != DBNull.Value)
                                oMarkAssignment.OriginalShcoolWiseTestId = oSqlDataReader["Original_SchoolWise_Test_Id"].ToInt();
                            if (oSqlDataReader["SchoolWise_Test_Name"] != DBNull.Value)
                                oMarkAssignment.SchoolWiseTestName = oSqlDataReader["SchoolWise_Test_Name"].ToString();
                            if (oSqlDataReader["Subject_Name"] != DBNull.Value)
                                oMarkAssignment.SubjectName = oSqlDataReader["Subject_Name"].ToString();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oMarkAssignment.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subject_Total_Marks"] != DBNull.Value)
                                oMarkAssignment.SubjectTotalMarks = oSqlDataReader["Subject_Total_Marks"].ToInt();
                            if (oSqlDataReader["Passing_Total_Marks"] != DBNull.Value)
                                oMarkAssignment.PassingTotalMarks = oSqlDataReader["Passing_Total_Marks"].ToDecimal();
                            if (oSqlDataReader["Subject_Total"] != DBNull.Value)
                                oMarkAssignment.SubjectTotal = oSqlDataReader["Subject_Total"].ToString();
                            if (oSqlDataReader["Grade_Or_Marks"] != DBNull.Value)
                                oMarkAssignment.GradeOrMarks = oSqlDataReader["Grade_Or_Marks"].ToString();
                            if (oSqlDataReader["TestType_Id"] != DBNull.Value)
                                oMarkAssignment.TestTypeId = oSqlDataReader["TestType_Id"].ToInt();
                            if (oSqlDataReader["Marks_Scored"] != DBNull.Value)
                                oMarkAssignment.MarksScored = oSqlDataReader["Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["TestType_Name"] != DBNull.Value)
                                oMarkAssignment.TestTypeName = oSqlDataReader["TestType_Name"].ToString();
                            if (oSqlDataReader["ShortenTestType_Name"] != DBNull.Value)
                                oMarkAssignment.ShortenTestTypeName = oSqlDataReader["ShortenTestType_Name"].ToString();
                            if (oSqlDataReader["Grade"] != DBNull.Value)
                                oMarkAssignment.Grade = oSqlDataReader["Grade"].ToString();
                            if (oSqlDataReader["TotalGrade"] != DBNull.Value)
                                oMarkAssignment.TotalGrade = oSqlDataReader["TotalGrade"].ToString();
                            if (oSqlDataReader["TestType_Total_Marks"] != DBNull.Value)
                                oMarkAssignment.TestTypeTotalMarks = oSqlDataReader["TestType_Total_Marks"].ToInt();
                            if (oSqlDataReader["TestType_Passing_Marks"] != DBNull.Value)
                                oMarkAssignment.TestTypePassingMarks = oSqlDataReader["TestType_Passing_Marks"].ToDecimal();
                            if (oSqlDataReader["Is_Absent"] != DBNull.Value)
                                oMarkAssignment.IsAbsent = oSqlDataReader["Is_Absent"].ToString();
                            if (oSqlDataReader["SchoolWise_Student_Test_Marks_Id"] != DBNull.Value)
                                oMarkAssignment.SchoolWiseStudentTestId = oSqlDataReader["SchoolWise_Student_Test_Marks_Id"].ToInt();
                            if (oSqlDataReader["TestWise_Subject_Marks_Id"] != DBNull.Value)
                                oMarkAssignment.TestWiseSubjectId = oSqlDataReader["TestWise_Subject_Marks_Id"].ToInt();
                            if (oSqlDataReader["ConsiderExamStatus"] != DBNull.Value)
                                oMarkAssignment.ConsiderExamStatus = oSqlDataReader["ConsiderExamStatus"].ToString();
                            if (oSqlDataReader["ConsiderInResult"] != DBNull.Value)
                                oMarkAssignment.ConsiderInResult = oSqlDataReader["ConsiderInResult"].ToString();
                            if (oSqlDataReader["ShowOnlyGrades"] != DBNull.Value)
                                oMarkAssignment.ShowOnlyGrades = oSqlDataReader["ShowOnlyGrades"].ToBool();
                            if (oSqlDataReader["AllowDecimal"] != DBNull.Value)
                                oMarkAssignment.AllowDecimal = oSqlDataReader["AllowDecimal"].ToBool();
                            if (oSqlDataReader["Test_Date"] != DBNull.Value)
                                oMarkAssignment.TestDate = oSqlDataReader["Test_Date"].ToDateTime();
                            if (oSqlDataReader["Is_CoCurricularActivity"] != DBNull.Value)
                                oMarkAssignment.IsCoCurricularActivity = oSqlDataReader["Is_CoCurricularActivity"].ToBool();
                            if (oSqlDataReader["IsActivitySubject"] != DBNull.Value)
                                oMarkAssignment.IsActivitySubject = oSqlDataReader["IsActivitySubject"].ToBool();
                            if (oSqlDataReader["TestOutOfMarks"] != DBNull.Value)
                                oMarkAssignment.TestOutOfMarks = oSqlDataReader["TestOutOfMarks"].ToInt();
                            if (oSqlDataReader["TestTypeOutOfMarks"] != DBNull.Value)
                                oMarkAssignment.TestTypeOutOfMarks = oSqlDataReader["TestTypeOutOfMarks"].ToInt();
                            if (oSqlDataReader["Total_Consideration"] != DBNull.Value)
                                oMarkAssignment.TotalConsideration = oSqlDataReader["Total_Consideration"].ToString();
                            if (oSqlDataReader["IsExamStatusApplicable"] != DBNull.Value)
                                oMarkAssignment.IsExamStatusApplicable = oSqlDataReader["IsExamStatusApplicable"].ToBool();
                            if (oSqlDataReader["StudentWiseTestPublishStatus"] != DBNull.Value)
                                oMarkAssignment.StudentWiseTestPublishStatus = oSqlDataReader["StudentWiseTestPublishStatus"].ToString();
                            if (oSqlDataReader["ExamPublishStatus"] != DBNull.Value)
                                oMarkAssignment.ExamPublishStatus = oSqlDataReader["ExamPublishStatus"].ToString();
                            oStudentProgressReport.MarkAssignmentDetails.Add(oMarkAssignment);
                        }
                    }

                    oStudentProgressReport.ExamWisePercentageDetails = new List<ExamWisePercentage>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            StudentWiseProgressReportExamWisePercentage oExamWisePercentage = new StudentWiseProgressReportExamWisePercentage();
                            if (oSqlDataReader["SchoolWise_Test_Id"] != DBNull.Value)
                                oExamWisePercentage.SchoolWiseTestId = oSqlDataReader["SchoolWise_Test_Id"].ToInt();
                            if (oSqlDataReader["Total_Marks_Scored"] != DBNull.Value)
                                oExamWisePercentage.TotalMarksScored = oSqlDataReader["Total_Marks_Scored"].ToDecimal();
                            if (oSqlDataReader["Subjects_Total_Marks"] != DBNull.Value)
                                oExamWisePercentage.SubjectTotalMarks = oSqlDataReader["Subjects_Total_Marks"].ToInt();
                            if (oSqlDataReader["Percentage"] != DBNull.Value)
                                oExamWisePercentage.Percentage = oSqlDataReader["Percentage"].ToDecimal();
                            if (oSqlDataReader["Grade_Name"] != DBNull.Value)
                                oExamWisePercentage.Grade = oSqlDataReader["Grade_Name"].ToString();
                            if (oSqlDataReader["Grade_id"] != DBNull.Value)
                                oExamWisePercentage.GradeId = oSqlDataReader["Grade_id"].ToInt();
                            if (oSqlDataReader["Result"] != DBNull.Value)
                                oExamWisePercentage.Result = oSqlDataReader["Result"].ToString();
                            if (oSqlDataReader["rank"] != DBNull.Value)
                                oExamWisePercentage.Rank = oSqlDataReader["rank"].ToInt();
                            if (oSqlDataReader["StudentWiseTestPublishStatus"] != DBNull.Value)
                                oExamWisePercentage.StudentWiseTestPublishStatus = oSqlDataReader["StudentWiseTestPublishStatus"].ToString();
                            if (oSqlDataReader["ExamPublishStatus"] != DBNull.Value)
                                oExamWisePercentage.ExamPublishStatus = oSqlDataReader["ExamPublishStatus"].ToString();
                            if (oSqlDataReader["ExamSubmitStatus"] != DBNull.Value)
                                oExamWisePercentage.ExamSubmitStatus = oSqlDataReader["ExamSubmitStatus"].ToString();
                            if (oSqlDataReader["SchoolWise_Test_Name"] != DBNull.Value)
                                oExamWisePercentage.SchoolWiseTestName = oSqlDataReader["SchoolWise_Test_Name"].ToString();
                            oStudentProgressReport.ExamWisePercentageDetails.Add(oExamWisePercentage);
                        }
                    }

                    oStudentProgressReport.SubjectTestGroupTotalDetails = GetSubjectTestGroupTotalDetails(oSqlDataReader);
                    oStudentProgressReport.SubjectTestTypeGroupTotalDetails = GetSubjectTestTypeGroupTotalDetails(oSqlDataReader);
                    oStudentProgressReport.SubjectTestTypeDetails = GetSubjectTestTypeDetails(oSqlDataReader);
                    oStudentProgressReport.TestTypeDetails = GetTestTypeDetails(oSqlDataReader);

                    oStudentProgressReport.GradeDetails = new List<Grade>();
                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            StudentWiseProgressReportGrade oGrade = new StudentWiseProgressReportGrade();
                            if (oSqlDataReader["Marks_Grades_Configuration_Detail_ID"] != DBNull.Value)
                                oGrade.GradeId = oSqlDataReader["Marks_Grades_Configuration_Detail_ID"].ToInt();
                            if (oSqlDataReader["Grade_Name"] != DBNull.Value)
                                oGrade.GradeName = oSqlDataReader["Grade_Name"].ToString();
                            if (oSqlDataReader["Remarks"] != DBNull.Value)
                                oGrade.Remarks = oSqlDataReader["Remarks"].ToString();
                            if (oSqlDataReader["Starting_Marks_Range"] != DBNull.Value)
                                oGrade.StartingMarksRange = oSqlDataReader["Starting_Marks_Range"].ToInt();
                            if (oSqlDataReader["Actual_Ending_Marks_Range"] != DBNull.Value)
                                oGrade.ActualEndingMarksRange = oSqlDataReader["Actual_Ending_Marks_Range"].ToDecimal();
                            if (oSqlDataReader["IsForCoCurricularSubjects"] != DBNull.Value)
                                oGrade.IsForCoCurricularSubjects = oSqlDataReader["IsForCoCurricularSubjects"].ToBool();
                            if (oSqlDataReader["IsAttitudeSubject"] != DBNull.Value)
                                oGrade.IsActivitySubject = oSqlDataReader["IsAttitudeSubject"].ToBool();
                            oStudentProgressReport.GradeDetails.Add(oGrade);
                        }
                    }

                    oStudentProgressReport.ExamStatusDetails = GetExamStatusDetails(oSqlDataReader);
                    oStudentProgressReport.DependentExamDetails = GetDependentExamDetails(oSqlDataReader);

                    return oStudentProgressReport;
                }
            }
        }
        public List<TransferStudentSubjectsMarkDetails> GetStudentsToTransferMarks(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, string asName, int aiEndIndex, int aiStartRowIndex)
        {
            List<TransferStudentSubjectsMarkDetails> lstStudentInfo = new List<TransferStudentSubjectsMarkDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", CreateFilter(aiStandardDivisionId, asName), SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedStudentsForMarksTransfer"))
                {
                    GenericClass<TransferStudentSubjectsMarkDetails> oStudentInfo = new GenericClass<TransferStudentSubjectsMarkDetails>();
                    lstStudentInfo = oStudentInfo.GetFilledObjectList(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        while (oSqlDataReader.Read())
                            miStudentMarksTransferCount = Convert.ToInt32(oSqlDataReader["COUNT"]);
                }
            }

            return lstStudentInfo;
        }

        private string CreateFilter(int aiStandardDivisionId, string asName)
        {
            string sFilter = string.Empty;
            asName = asName.IsNullOrEmpty() ? string.Empty : StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false);
            if (aiStandardDivisionId != 0)
                sFilter = " AND vw_standard_division.SchoolWise_Standard_Division_Id =+ CAST(" + aiStandardDivisionId + "AS VARCHAR(15))";

            if (!asName.IsNullOrEmpty())
                sFilter = sFilter + " AND (First_Name LIKE '%" + asName + "%' OR Middle_Name LIKE '%" + asName + "%' OR Last_Name LIKE '%" + asName + "%' OR Enrolment_Number LIKE '%" + asName + "%' OR Enrolment_Number +' - '+ Name LIKE '%" + asName + "%') ";

            return sFilter;
        }

        public DataTable Transfer(string asStudentTransferMarksXml, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentTrnsferMarksXML", asStudentTransferMarksXml, SqlDbType.Xml);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_TransferSubjectMarks");
            }
        }

        #endregion        
    
        public void SaveStudentwiseRemarks(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("InsertedByid", moStudentSubjectMarksStruct.miInsertedByid, SqlDbType.Int);             
                oSQLServerDbUtility.AddParameter("RemarkXml", moStudentSubjectMarksStruct.msRemarkXml, SqlDbType.Xml);                
                oSQLServerDbUtility.AddParameter("TestId", moStudentSubjectMarksStruct.miTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", moStudentSubjectMarksStruct.miSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentwiseRemarks");
            }
        }
    }
}
