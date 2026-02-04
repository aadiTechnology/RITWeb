using System;
using System.Data;
using System.Data.SqlClient;
using Utility;
using StandardWiseExamConfigurationEntities;
namespace DataCommunicator
{

    public class SubjectTestConfigurationDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        //structure for master table 
        public struct SubjectTestConfigurationMasterStruct
        {
            public int miTestWiseSubjectMarksId;
            public int miSchoolId;
            public int miAcademicYearId;
            public int miStandardDivisionId;
            public int miSubjectId;
            public int miSchoolWiseTestId;
            public string msGradeOrMarks;
            public int miSubjectTotalMarks;
            public decimal mdPassingTotalMarks;
            public int miPassingGradeId;
            public int miOutOfMarks;
            public bool mbIsExamStatusApplicable;
            public string msIsDeleted;
            public string msResultConsideration;
            public string msTotalConsideration;
            public double mdRsltFactor;
            public bool mbDisplayGrade;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public bool mbAllowDecimal;
        }

        #endregion

        #region DataMembers and properties

        #region Data members

        private SubjectTestConfigurationMasterStruct moSubjectTestConfigurationMasterStruct;
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private SubjectTestConfigurationDC moSubjectTestConfigurationDC = null;

        #endregion

        #region Properties

        public SubjectTestConfigurationMasterStruct SubjectTestConfigurationMasterStructDetails
        {

            get { return moSubjectTestConfigurationMasterStruct; }
            set { moSubjectTestConfigurationMasterStruct = value; }
        }

        #endregion

        #endregion

        #region Constructors

        public SubjectTestConfigurationDC()
        {
        }

        public SubjectTestConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
            moSubjectTestConfigurationDC = new SubjectTestConfigurationDC();
        }
        public SubjectTestConfigurationDC(int aiId)
        {
            LoadSchoolWiseTestSubjectMarksMasterDetails(aiId);
        }
        #endregion

        #region Private Methods

        public void LoadSchoolWiseTestSubjectMarksMasterDetails(int aiId)
        {

            using (SqlDataReader oDR = FetchSchoolWiseTestSubjectMarksMasterDataFromDatabase(aiId))
            {
                if (oDR != null)
                {
                    while (oDR.Read())
                    {

                        if (oDR["TestWise_Subject_Marks_Id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId = Convert.ToInt32(oDR["TestWise_Subject_Marks_Id"].ToString());
                        if (oDR["School_Id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                        if (oDR["Standard_Division_Id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miStandardDivisionId = Convert.ToInt32(oDR["Standard_Division_Id"].ToString());
                        if (oDR["Subject_Id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miSubjectId = Convert.ToInt32(oDR["Subject_Id"].ToString());
                        if (oDR["SchoolWise_Test_Id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miSchoolWiseTestId = Convert.ToInt32(oDR["SchoolWise_Test_Id"].ToString());
                        if (oDR["Grade_Or_Marks"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.msGradeOrMarks = oDR["Grade_Or_Marks"].ToString();
                        if (oDR["Subject_Total_Marks"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miSubjectTotalMarks = Convert.ToInt32(oDR["Subject_Total_Marks"].ToString());
                        if (oDR["Passing_Total_Marks"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.mdPassingTotalMarks = Convert.ToDecimal(oDR["Passing_Total_Marks"].ToString());
                        if (oDR["Is_Deleted"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                        if (oDR["Insert_Date"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                        if (oDR["Inserted_By_id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"].ToString());
                        if (oDR["Update_Date"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                        if (oDR["Updated_By_Id"] != DBNull.Value)
                            moSubjectTestConfigurationMasterStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());

                    }
                }
            }
        }
        public SqlDataReader FetchSchoolWiseTestSubjectMarksMasterDataFromDatabase(int aiId)
        {

            string sSelectStatement = " SELECT  " +
                "testwise_subject_marks_id" +
                " , school_id" +
                " , standard_division_id" +
                " , subject_id" +
                " , schoolwise_test_id" +
                " , grade_or_marks" +
                " , subject_total_marks" +
                " , passing_total_marks" +
                " , is_deleted" +
                " , insert_date" +
                " , inserted_by_id" +
                " , update_date" +
                " , updated_by_id" +

            " FROM  " +
                "SchoolWise_Test_Subject_Marks_Master " +
            " WHERE  " +
                 "testwise_subject_marks_id = " + aiId +
                " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asXMLString"></param>
        public void AddSubjectTestConfiguration(string asXMLString)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSubjectTestConfigurationMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", moSubjectTestConfigurationMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", moSubjectTestConfigurationMasterStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Subject_Id", moSubjectTestConfigurationMasterStruct.miSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolWise_Test_Id", moSubjectTestConfigurationMasterStruct.miSchoolWiseTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Grade_Or_Marks", moSubjectTestConfigurationMasterStruct.msGradeOrMarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Subject_Total_Marks", moSubjectTestConfigurationMasterStruct.miSubjectTotalMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Passing_Total_Marks", moSubjectTestConfigurationMasterStruct.mdPassingTotalMarks, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Passing_Grade_Id", moSubjectTestConfigurationMasterStruct.miPassingGradeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OutOfMarks", moSubjectTestConfigurationMasterStruct.miOutOfMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsExamStatusApplicable", moSubjectTestConfigurationMasterStruct.mbIsExamStatusApplicable, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("DisplayGrade", moSubjectTestConfigurationMasterStruct.mbDisplayGrade, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("inserted_By_id", moSubjectTestConfigurationMasterStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Result_Consideration", moSubjectTestConfigurationMasterStruct.msResultConsideration, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Result_Factor", moSubjectTestConfigurationMasterStruct.mdRsltFactor, SqlDbType.Float);
                oSQLServerDbUtility.AddParameter("Test_Type_Details", asXMLString, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Total_Consideration", moSubjectTestConfigurationMasterStruct.msTotalConsideration, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AllowDecimal", moSubjectTestConfigurationMasterStruct.mbAllowDecimal, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_AddTestSubjectMarks", true);
            }

        }
        public DataTable DeleteAllExams(int aiStandardDivisionId, int aiSubjectId, int aiUserId, int aiAcademicYearId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteAllExams");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(string asXMLString)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestWise_Subject_Marks_Id", moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Grade_Or_Marks", moSubjectTestConfigurationMasterStruct.msGradeOrMarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Subject_Total_Marks", moSubjectTestConfigurationMasterStruct.miSubjectTotalMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Passing_Total_Marks", moSubjectTestConfigurationMasterStruct.mdPassingTotalMarks, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Passing_Grade_Id", moSubjectTestConfigurationMasterStruct.miPassingGradeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OutOfMarks", moSubjectTestConfigurationMasterStruct.miOutOfMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsExamStatusApplicable", moSubjectTestConfigurationMasterStruct.mbIsExamStatusApplicable, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("DisplayGrade", moSubjectTestConfigurationMasterStruct.mbDisplayGrade, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moSubjectTestConfigurationMasterStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Result_Consideration", moSubjectTestConfigurationMasterStruct.msResultConsideration, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Result_Factor", moSubjectTestConfigurationMasterStruct.mdRsltFactor, SqlDbType.Float);
                oSQLServerDbUtility.AddParameter("Test_Type_Details", asXMLString, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Total_Consideration", moSubjectTestConfigurationMasterStruct.msTotalConsideration, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AllowDecimal", moSubjectTestConfigurationMasterStruct.mbAllowDecimal, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_UpdateTestSubjectMarks", true);
            }
        }


        public void DeleteSubjectTestConfiguration()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestWise_Subject_Marks_Id", moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_DeleteTestSubjectMarks", true);
            }
        }
        public void DeleteTestExamMarkDetails(bool abDeleteStudentWiseSavedMarks)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestWiseSubjectMarksId", moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", moSubjectTestConfigurationMasterStruct.miSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", moSubjectTestConfigurationMasterStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", moSubjectTestConfigurationMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moSubjectTestConfigurationMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DeleteStudentwiseProgressReportMarks", abDeleteStudentWiseSavedMarks, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_DeleteTestExamMarkDetails", true);
            }
        }


        public DataSet CopyTestConfiguration(int aiStandardDivisionId, int aiSubjectId, string asTestConfiguration, int aiUserId, int aiAcademicYearId, string ids)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSource_StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iSource_Subject_Id", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sStandardDivisionSubjectIds", asTestConfiguration, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("iCurrentUserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("STestwiseSubjectId", ids, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_CopyTestConfiguration", true);
            }
        }

        public string CheckDependenciesForExamConfUpdate(int aiTestWise_Subject_Marks_Id, string asExamName)
        {
            string sSelectStament = " select COUNT(*) As Rec_Count  from " +
                                    " vw_StudentTestMarks  " +
                                    " Where TestWise_Subject_Marks_Id = " + aiTestWise_Subject_Marks_Id.ToString() +
                                    " AND Is_Deleted=N'" + Constants.C_NO + "'";

            int iRowCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iRowCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStament);
            if (iRowCount > 0)
                return "Exam Configuration can not be modified since marks are already assigned to students.";
            return string.Empty;
        }

        /// <summary>
        /// This method is used to update result factor.
        /// </summary>
        public void UpdateResultFactor()
        {
            string sSelectStament = "UPDATE Schoolwise_Division_Subject_Master " +
                                    " SET Result_Factor= " + moSubjectTestConfigurationMasterStruct.mdRsltFactor.ToString() +
                                    " WHERE School_Id = " + moSubjectTestConfigurationMasterStruct.miSchoolId.ToString() +
                                    " AND academic_Year_Id = " + moSubjectTestConfigurationMasterStruct.miAcademicYearId.ToString() +
                                    " AND Standard_Division_Id= " + moSubjectTestConfigurationMasterStruct.miStandardDivisionId.ToString() +
                                    " AND Subject_Id = " + moSubjectTestConfigurationMasterStruct.miSubjectId.ToString();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSelectStament);
        }

        public DataTable GetFillStandardWiseSubjects(int iStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetFillStandardWiseSubjects]");
            }
        }

        #endregion

    }


    public class SubjectTestConfigurationCollectionDC
    {
        #region data members
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        
        #endregion

        #region constructor

        public SubjectTestConfigurationCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region

        public DataTable FetchTestsConfigurationForTeacher(int aiTeacherId, int aiTestId, string asAllowPartialSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AllowPartialSubmit", asAllowPartialSubmit, SqlDbType.Char);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestMarksStatus");
            }
        }

        public DataTable GetSubjectTeachers(int aiTeacherId, int aiTestId, bool abIsClassTeacher, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsClassTeacher", abIsClassTeacher, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSubjectTeachers");
            }
        }

        public void PublishObservationTest(int aiTestId, int aiStandardDivId, int aiInsertedById, bool abPublish)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Publish", abPublish, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishObservationExam");
            }
        }

        public DataTable FetchTestsConfigurationForMySubjects(int aiTeacherId, int aiTestId, string asAllowPartialSubmit, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AllowPartialSubmit", asAllowPartialSubmit, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestMarksStatusForClass");
            }
        }

        public DataTable FetchTestsConfigurationForMyClass(int aiTeacherId, int aiTestId, string asAllowPartialSubmit, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AllowPartialSubmit", asAllowPartialSubmit, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestMarksStatusForClassTeacher");
            }
        }


        public DataTable FetchStandardDivisionTestsConfigurationForTeacher(int aiTeacherId, int aiTestId, string asAllowPartialSubmit, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Test_Id", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AllowPartialSubmit", asAllowPartialSubmit, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestMarksStatus");
            }
        }


      
        /// <summary>
        /// vw_subject_Test_Master
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public DataTable RetriveAllTestConfiguration(int aiStandardDivisionId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetClasswiseTestConfigurationDetails");
            }
        }

        public DataSet GetClassSubjectTestsAssociation(int aiStandardId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ClassSubjectTestsAssociation");
            }
        }

        public StandardGradeConfiguration CheckPreConditioOfGrades(int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_CheckPreConditionForStandardGrade"))
                {
                    if (oSqlDataReader.Read())
                    {
                        StandardGradeConfiguration oStandardGradeConfiguration = new StandardGradeConfiguration
                        {
                            StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"].ToString()),
                            IsSubjectConfigure = oSqlDataReader["IsSubjectConfigure"].ToString(),
                            IsCocoricularConfigure = oSqlDataReader["IsCocoricularConfig"].ToString(),
                            IsFailCriteriaNotConfigure = oSqlDataReader["IsFailCriteriaNotConfig"].ToString()

                        };
                        return oStandardGradeConfiguration;
                    }
                }
            }

            return null;
        }
        
        public DataTable GetAllClasses(int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllClassesOfClassTeacher");
            }
        }

        #endregion
    }

}
