using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Utility;
namespace DataCommunicator
{

    public class SchoolWiseTestMasterDC
    {

        #region Constant and structures

        #region structure

        public struct SchoolWiseTestMasterStruct
        {
            public int miSchoolWiseTestId;
            public string msSchoolWiseTestName;
            public int miOriginalSchoolWiseTestId;
            public int miSchoolId;
            public int miAcademicYearId;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public int miTermId;
            public int miIsFinalExam;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private SchoolWiseTestMasterStruct moSchoolWiseTestMasterStruct;

        #endregion
        #region Properties

        public SchoolWiseTestMasterStruct SchoolWiseTestMasterStructDetails
        {

            get { return moSchoolWiseTestMasterStruct; }
            set { moSchoolWiseTestMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SchoolWiseTestMasterDC()
        {
        }

        #endregion

        #region Public Methods

        public string GetInsertStatementForTestMaster()
        {

            string sInsertStatement = "INSERT INTO SchoolWise_Test_Master ( " +
                "  schoolwise_test_name" +
                " , original_schoolwise_test_id" +
                " , school_id" +
                " , is_deleted" +
                " , inserted_by_id" +
                " , updated_by_id" +
                " , academic_year_id" +
                " , Term_Id" +
                " , IsFinalExam" +
            ") VALUES (" +
                 "  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseTestMasterStruct.msSchoolWiseTestName, false) + "' " +
                 " , " + moSchoolWiseTestMasterStruct.miOriginalSchoolWiseTestId +
                 " , " + moSchoolWiseTestMasterStruct.miSchoolId +
                 " , N'" + Constants.C_NO + "' " +
                 " , " + moSchoolWiseTestMasterStruct.miInsertedByid +
                 " , " + moSchoolWiseTestMasterStruct.miUpdatedById +
                 " , " + moSchoolWiseTestMasterStruct.miAcademicYearId +
                 " , " + moSchoolWiseTestMasterStruct.miTermId +
                 " , " + moSchoolWiseTestMasterStruct.miIsFinalExam +
            " ) ";

            return sInsertStatement;

        }

        public string GetUpdateStamentForTestMaster()
        {

            string sUpdateStatement = " UPDATE SchoolWise_Test_Master SET " +
                "  schoolwise_test_name =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseTestMasterStruct.msSchoolWiseTestName, false) + "' " +
                " , original_schoolwise_test_id =  " + moSchoolWiseTestMasterStruct.miOriginalSchoolWiseTestId +
                " , school_id =  " + moSchoolWiseTestMasterStruct.miSchoolId +
                " , updated_by_id =  " + moSchoolWiseTestMasterStruct.miUpdatedById +
                " , Term_Id =  " + moSchoolWiseTestMasterStruct.miTermId +
                " , IsFinalExam = " + moSchoolWiseTestMasterStruct.miIsFinalExam +
                " , Update_Date = dbo.GetLocalDate(DEFAULT)" +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_test_id =  " + moSchoolWiseTestMasterStruct.miSchoolWiseTestId;

            return sUpdateStatement;
        }

        public string GetDeleteStatementForTestMaster()
        {

            string sDeleteStatement = " DELETE SchoolWise_Test_Master " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_test_id =  N'" + moSchoolWiseTestMasterStruct.miSchoolWiseTestId + "'";
            return sDeleteStatement;
        }

        /// <summary>
        /// This method is used to get latest exam id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public static int GetLatestExamId(int aiSchoolId, int aiAccYearId, int aiStandardDivisionId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAccYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iStandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iStandard_Id", aiStandardId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("prm_iLatest_Exam_Id", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetLatestExamId");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        #endregion

    }
    public class TestCollectionDC
    {
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private int miStanderedDivisionId = 0;

        #region constructor
        public TestCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public TestCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public TestCollectionDC(int aiSchoolId, int aiAcademicYearId, int aiStanderedDivisionId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miStanderedDivisionId = aiStanderedDivisionId;
        }

        #endregion

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public DataTable GetAllTests()
        {
            // This method returns dataset populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT " +
                       " -9999 as school_id" +
                       " , original_schoolwise_test_id " +
                       " , schoolwise_test_id " +
                       " , schoolwise_test_name " +
                       " , ISNULL(Term_Id,0) Term_Id" +
                       " , IsFinalExam" +
                   " FROM " +
                        " schoolwise_test_master " +
                   " WHERE " +
                        " is_deleted = N'" + Constants.C_NO + "'" +
                        " AND school_id is null " +
                        " AND original_schoolwise_test_id NOT IN " +
                        " ( " +
                         " SELECT  " +
                               " original_schoolwise_test_id " +
                           " FROM " +
                                " schoolwise_test_master " +
                           " WHERE " +
                                " is_deleted = N'" + Constants.C_NO + "'" +
                                " AND school_id = " + miSchoolId +
                                " AND academic_year_id = " + miAcademicYearId +
                           " )" +
                   " UNION " +
                    " SELECT  " +
                       " school_id " +
                       " , original_schoolwise_test_id " +
                       " , schoolwise_test_id " +
                       " , schoolwise_test_name " +
                       " , ISNULL(Term_Id,0) Term_Id" +
                        " , IsFinalExam" +
                   " FROM " +
                        " schoolwise_test_master " +
                   " WHERE " +
                        " is_deleted = N'" + Constants.C_NO + "'" +
                        " AND school_id = " + miSchoolId +
                        " AND academic_year_id = " + miAcademicYearId +
                    " ORDER BY " +
                         " original_schoolwise_test_id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        /// <summary>
        /// Get all test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForSchool()
        {
            // This method returns dataset populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT  " +
                       " school_id " +
                       " , original_schoolwise_test_id " +
                       " , schoolwise_test_id " +
                       " , schoolwise_test_name " +
                       ", IsFinalExam "+
                   " FROM " +
                        " schoolwise_test_master " +
                   " WHERE " +
                        " is_deleted = N'" + Constants.C_NO + "'" +
                        " AND school_id = " + miSchoolId +
                        " AND academic_year_id = " + miAcademicYearId +
                    " ORDER BY " +
                         " original_schoolwise_test_id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get test details to fill Combo.
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <returns></returns>
        public DataTable GetAllTestsForClass(int aiStandardDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetClasswiseTestForCombobox");
            }            
        }

        /// <summary>
        /// Get all Published test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllpublishedTestsForClass()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStanderedDivisionId", miStanderedDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllpublishedTestsForclass");
            }

        }

        /// <summary>
        /// Get all tests for which toppers are generated.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForWhichToppersGenerated()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StanderedDivisionId", miStanderedDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllTestsForWhichToppersGenerated");
            }
        }

        /// <summary>
        /// Get all Published test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllpublishedTestsForStandard(int @Standard_Id, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandard_Id", @Standard_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllpublishedTestsForStandard");
            }
        }

        /// <summary>
        /// Get all Published test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForStandard(int @Standard_Id, bool abIsServiceCall = false, int aiSchoolId = 0, int aiAcademicYearId = 0)
        {
            // This method returns dataset populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = "SELECT     SchoolWise_Test_Master.SchoolWise_Test_Id" +
                            " , SchoolWise_Test_Master.SchoolWise_Test_Name " +
                            " FROM   SchoolWise_Test_Master INNER JOIN " +
                              " Schoolwise_Standard_Test_Master ON " +
                              " SchoolWise_Test_Master.SchoolWise_Test_Id = Schoolwise_Standard_Test_Master.SchoolWise_Test_Id AND " +
                              " SchoolWise_Test_Master.School_Id = Schoolwise_Standard_Test_Master.School_Id AND " +
                              " SchoolWise_Test_Master.academic_year_id = Schoolwise_Standard_Test_Master.academic_Year_Id " +
                            " WHERE     (SchoolWise_Test_Master.Is_Deleted = 'N') " +
                            " AND (SchoolWise_Test_Master.School_Id = " + miSchoolId + " ) " +
                            " AND (SchoolWise_Test_Master.academic_year_id = " + miAcademicYearId + ") " +
                            " AND (Schoolwise_Standard_Test_Master.Standard_Id = " + @Standard_Id + ") " +
                            " ORDER BY SchoolWise_Test_Master.Original_SchoolWise_Test_Id ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility( miSchoolId, miAcademicYearId, Constants.I_ZERO, abIsServiceCall))
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// Get all Published test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForStandard(int aiStandard_Id, int aiHeaderId)
        {
            // This method returns dataset populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = "SELECT     SchoolWise_Test_Master.SchoolWise_Test_Id" +
                            " , SchoolWise_Test_Master.SchoolWise_Test_Name " +
                            " , [dbo].[Udf_CanDevelopmentAreaApplicable](SchoolWise_Test_Master.SchoolWise_Test_Id," + aiStandard_Id + "," + aiHeaderId + ") AS IsApplicable" +
                            " FROM   SchoolWise_Test_Master INNER JOIN " +
                              " Schoolwise_Standard_Test_Master ON " +
                              " SchoolWise_Test_Master.SchoolWise_Test_Id = Schoolwise_Standard_Test_Master.SchoolWise_Test_Id AND " +
                              " SchoolWise_Test_Master.School_Id = Schoolwise_Standard_Test_Master.School_Id AND " +
                              " SchoolWise_Test_Master.academic_year_id = Schoolwise_Standard_Test_Master.academic_Year_Id " +
                            " WHERE     (SchoolWise_Test_Master.Is_Deleted = 'N') " +
                            " AND (SchoolWise_Test_Master.School_Id = " + miSchoolId + " ) " +
                            " AND (SchoolWise_Test_Master.academic_year_id = " + miAcademicYearId + ") " +
                            " AND (Schoolwise_Standard_Test_Master.Standard_Id = " + aiStandard_Id + ") " +
                            " ORDER BY SchoolWise_Test_Master.Original_SchoolWise_Test_Id ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>

        public void UpdateTests(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public DataTable GetAllExamsForTestwiseTopperReport(int aiStandardId)
        {
            string sSelectStatement = "SELECT SchoolWise_Test_Id as Value_Member, SchoolWise_Test_Name as Display_Member" +
                                " FROM dbo.vw_TestwiseTopperExams" +
                                " WHERE School_Id=" + miSchoolId + " AND academic_year_id = " + miAcademicYearId + " AND Standard_Id = " + aiStandardId;
            using (SQLServerDbUtility OSQLServerDbUtility = new SQLServerDbUtility())
                return OSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        #endregion
    }

}
