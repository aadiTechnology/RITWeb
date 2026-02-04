using System;
using System.Data;
using Utility;
using System.Data.SqlClient;

namespace DataCommunicator
{

    public class SubjectTestTypeConfigurationDC
    {

        #region Constant and structures

        #region structure

        public struct SubjectTestTypeConfigurationStruct
        {
            public int miTestWiseSubjectMarksDetailId;
            public int miTestWiseSubjectMarksId;
            public int miTestTypeId;
            public int miTestTypeTotalMarks;
            public int miTestTypePassingMarks;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private SubjectTestTypeConfigurationStruct moSubjectTestTypeConfigurationStruct;

        #endregion
        #region Properties

        public SubjectTestTypeConfigurationStruct SubjectTestTypeConfigurationStructDetails
        {

            get { return moSubjectTestTypeConfigurationStruct; }
            set { moSubjectTestTypeConfigurationStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SubjectTestTypeConfigurationDC()
        {
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiTestWiseSubjectMarksId"></param>
        /// <returns></returns>
        public static DataTable FetchAllTestSubjectMarksDetailsDataFromDatabase(int aiTestWiseSubjectMarksId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TestWiseSubjectMarksId", aiTestWiseSubjectMarksId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestSubjectMarksConfigDetails");
            }
        }


        #endregion

        #region Public Methods

        public Int32 InsertSchoolWiseTestSubjectMarksDetails()
        {

            string sInsertStatement = "INSERT INTO SchoolWise_Test_Subject_Marks_Details ( " +
                "testwise_subject_marks_detail_id" +
                " , testwise_subject_marks_id" +
                " , testtype_id" +
                " , testtype_total_marks" +
                " , testtype_passing_marks" +
                " , is_deleted" +
                " , insert_date" +
                " , inserted_by_id" +
                " , update_date" +
                " , updated_by_id" +

            ") VALUES (" + "  " + moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksDetailId +
                 " , " + moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksId +
                 " , " + moSubjectTestTypeConfigurationStruct.miTestTypeId +
                 " , " + moSubjectTestTypeConfigurationStruct.miTestTypeTotalMarks +
                 " , " + moSubjectTestTypeConfigurationStruct.miTestTypePassingMarks +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSubjectTestTypeConfigurationStruct.msIsDeleted, false) + "' " +
                 " , N'" + moSubjectTestTypeConfigurationStruct.mdtInsertDate + "' " +
                 " , " + moSubjectTestTypeConfigurationStruct.miInsertedByid +
                 " , N'" + moSubjectTestTypeConfigurationStruct.mdtUpdateDate + "' " +
                 " , " + moSubjectTestTypeConfigurationStruct.miUpdatedById +
            " ) ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }
        public void UpdateSchoolWiseTestSubjectMarksDetails()
        {

            string sUpdateStatement = " UPDATE SchoolWise_Test_Subject_Marks_Details SET " +
                "testwise_subject_marks_detail_id =  " + moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksDetailId +
                " , testwise_subject_marks_id =  " + moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksId +
                " , testtype_id =  " + moSubjectTestTypeConfigurationStruct.miTestTypeId +
                " , testtype_total_marks =  " + moSubjectTestTypeConfigurationStruct.miTestTypeTotalMarks +
                " , testtype_passing_marks =  " + moSubjectTestTypeConfigurationStruct.miTestTypePassingMarks +
                " , is_deleted =  N'" + StringUtility.ReplaceSingleQuoteInString(moSubjectTestTypeConfigurationStruct.msIsDeleted, false) + "' " +
                " , insert_date =  N'" + moSubjectTestTypeConfigurationStruct.mdtInsertDate + "' " +
                " , inserted_by_id =  " + moSubjectTestTypeConfigurationStruct.miInsertedByid +
                " , update_date =  N'" + moSubjectTestTypeConfigurationStruct.mdtUpdateDate + "' " +
                " , updated_by_id =  " + moSubjectTestTypeConfigurationStruct.miUpdatedById +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND testwise_subject_marks_detail_id =  " + moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksDetailId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        #endregion

    }
    /// <summary>
    /// 
    /// </summary>
    public class SubjectTestTypeConfigurationCollectionDC
    {
        public static DataSet GetAllTestTypesForStandardDivisionSubjectTest(int aiStandardDivisionId,
                                                                            int aiSubjectId,
                                                                            int aiTestId,
                                                                            int aiSchoolId,
                                                                            int aiAcademicYrId,
                                                                            string asShowTotalAsPerOutOfMarks)
        {
            string sProcedureName = "usp_GetAllTestTypesForStandardDivisionSubjectTest";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Subject_Id", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowTotalAsPerOutOfMarks", asShowTotalAsPerOutOfMarks, SqlDbType.VarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sProcedureName);
            }
        }


        public static DataSet GetAllTestsResultDetails(int aiStandardDivisionId,
                                                                            int aiSchoolId,
                                                                            int aiAcademicYrId)
        {
            string sProcedureName = "usp_GetAllTestsResultDetails";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYrId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sProcedureName);
            }
        }

        public static void SubmitTestMarksToClassTeacher(int aiStandardDivisionId,
                                                                            int aiSubjectId,
                                                                            int aiTestId,
                                                                            int aiSchoolId,
                                                                            int aiAcademicYrId,
                                                                            string asIsSubmitted)
        {
            string sUpdateQry = " UPDATE SchoolWise_Test_Subject_Marks_Master " +
                                " SET Is_Submitted= N'" + asIsSubmitted + "'," +
                                " SubmitionDate = dbo.GetLocalDate(DEFAULT)" +
                                " WHERE " +
                                " School_Id =" + aiSchoolId +
                                " AND Academic_Year_Id =" + aiAcademicYrId +
                                " AND Subject_Id =" + aiSubjectId +
                                " AND Standard_Division_Id =" + aiStandardDivisionId +
                                " AND SchoolWise_Test_Id =" + aiTestId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateQry);
        }


        public static bool IsTestAndSubjectConfiguredForRemark(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("ShowRemarks", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsTestAndSubjectConfiguredForRemark");
                return oSqlParameter.Value.ToBool();
            }
        }
    }

}
