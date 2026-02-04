using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Utility;

namespace DataCommunicator
{
    public class TeacherSubjectAssignmentDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct TeacherSubjectAssignmentStruct
        {
            public int miTeacherSubjectId;
            public int miSchoolId;
            public int miSubjectId;
            public int miStandardDivisionId;
            public int miTeacherId;
            public int miInsertedById;
            public int miUpdatedById;
            public bool mbIsExclusive;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private TeacherSubjectAssignmentStruct moTeacherSubjectAssignmentStruct;

        #endregion
        #region Properties

        public TeacherSubjectAssignmentStruct TeacherSubjectAssignmentStructDetails
        {

            get { return moTeacherSubjectAssignmentStruct; }
            set { moTeacherSubjectAssignmentStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public TeacherSubjectAssignmentDC()
        {
        }

        public TeacherSubjectAssignmentDC(int aiId)
        {
            LoadTeacherSubjectAssignmentDetails(aiId);
        }
        #endregion

        #region Private Methods

        public void LoadTeacherSubjectAssignmentDetails(int aiId)
        {

           using( SqlDataReader oDR = FetchTeacherSubjectAssignmentDataFromDatabase(aiId))
           {
               if (oDR != null)
               {
                   while (oDR.Read())
                   {

                       if (oDR["Teacher_Subject_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miTeacherSubjectId = Convert.ToInt32(oDR["Teacher_Subject_Id"].ToString());
                       if (oDR["School_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                       if (oDR["Subject_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miSubjectId = Convert.ToInt32(oDR["Subject_Id"].ToString());
                       if (oDR["Standard_Division_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miStandardDivisionId = Convert.ToInt32(oDR["Standard_Division_Id"].ToString());
                       if (oDR["Teacher_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miTeacherId = Convert.ToInt32(oDR["Teacher_Id"].ToString());
                       if (oDR["Inserted_By_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"].ToString());
                       if (oDR["Updated_By_Id"] != DBNull.Value)
                           moTeacherSubjectAssignmentStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());

                   }
               }
            }

        }
        public SqlDataReader FetchTeacherSubjectAssignmentDataFromDatabase(int aiId)
        {

            string sSelectStatement = " SELECT  " +
                "teacher_subject_id" +
                " , school_id" +
                " , subject_id" +
                " , standard_division_id" +
                " , teacher_id" +
                " , is_deleted" +
                " , insert_date" +
                " , inserted_by_id" +
                " , update_date" +
                " , updated_by_id" +

            " FROM  " +
                "Teacher_Subject_Assignment " +
            " WHERE  " +
                 "teacher_subject_id = " + aiId +
                " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement);
        }

        #endregion

        #region Public Methods

        public Int32 InsertTeacherSubjectAssignment()
        {

            string sInsertStatement = "INSERT INTO Teacher_Subject_Assignment ( " +
                "  school_id" +
                " , subject_id" +
                " , standard_division_id" +
                " , IsExclusive " +
                " , teacher_id" +
                " , inserted_by_id" +
                " , updated_by_id" +

            ") VALUES (" +
                 "  " + moTeacherSubjectAssignmentStruct.miSchoolId +
                 " , " + moTeacherSubjectAssignmentStruct.miSubjectId +
                 " , " + moTeacherSubjectAssignmentStruct.miStandardDivisionId +
                 " , N'" + moTeacherSubjectAssignmentStruct.mbIsExclusive +
                 "' , " + moTeacherSubjectAssignmentStruct.miTeacherId +
                 " , " + moTeacherSubjectAssignmentStruct.miInsertedById +
                 " , " + moTeacherSubjectAssignmentStruct.miUpdatedById +
            " ) ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);

        }

        public void UpdateTeacherSubjectAssignment()
        {
            string sUpdateStatement = " UPDATE Teacher_Subject_Assignment SET " +
             " school_id =  " + moTeacherSubjectAssignmentStruct.miSchoolId +
                //" , standard_division_id =  " + moTeacherSubjectAssignmentStruct.miStandardDivisionId + 
             " , teacher_id =  " + moTeacherSubjectAssignmentStruct.miTeacherId +
             " , IsExclusive = N'" + moTeacherSubjectAssignmentStruct.mbIsExclusive +
             "' , inserted_by_id =  " + moTeacherSubjectAssignmentStruct.miInsertedById +
             " , updated_by_id =  " + moTeacherSubjectAssignmentStruct.miUpdatedById +
          " WHERE " +
             " is_deleted = N'" + Constants.C_NO + "'" +
                //" AND teacher_id =  " + moTeacherSubjectAssignmentStruct.miTeacherId +
              " AND standard_division_id = " + moTeacherSubjectAssignmentStruct.miStandardDivisionId +
              " AND subject_id =  " + moTeacherSubjectAssignmentStruct.miSubjectId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public bool DeleteAssignSubjectForTeacher(ArrayList aoArrDeleteTeacherIds)
        {
            string sDeleteUserList = "(";
            for (int iCount = 0; iCount < aoArrDeleteTeacherIds.Count; iCount++)
            {
                sDeleteUserList = sDeleteUserList + aoArrDeleteTeacherIds[iCount];
                sDeleteUserList = sDeleteUserList + ",";

            }
            sDeleteUserList = sDeleteUserList + ")";
            sDeleteUserList = sDeleteUserList.Remove(sDeleteUserList.Length - 2, 1);

            string sSqlDeleteUser = " DELETE Teacher_Subject_Assignment " +
                               " WHERE " +
                                   " teacher_id in " + sDeleteUserList;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
            return true;

        }

        public bool DeleteAssignTeacherForSubject(int aiStandardDivisionId, int aiSubjectId)
        {
            string sSqlDeleteUser = " DELETE Teacher_Subject_Assignment " +
                               " WHERE " +
                                   " Standard_Division_Id=" + aiStandardDivisionId +
                                   " AND Subject_Id=" + aiSubjectId +
                                   " AND teacher_subject_id=" + moTeacherSubjectAssignmentStruct.miTeacherSubjectId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
            return true;

        }

        public bool IsSubjectAssignToTeacher(int aiStandardDivisionId, int aiSubjectId)
        {
            // This method checks if the speicified Buyer login is duplicate or not.


            string sSelectStatement = " SELECT " +
                       " count(*) " +
                   " FROM " +
                       " Teacher_Subject_Assignment " +
                   " WHERE " +
                       " Standard_Division_Id =" + aiStandardDivisionId +
                       " AND Subject_Id =" + aiSubjectId;
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;

        }

        public string GetInsertStatementForTeacherSubjectAssignment()
        {
            string sInsertStatement = "INSERT INTO Teacher_Subject_Assignment ( " +
            "  school_id" +
            " , subject_id" +
            " , standard_division_id" +
            " , IsExclusive " +
            " , teacher_id" +
            " , inserted_by_id" +
            " , updated_by_id" +

        ") VALUES (" +
             "  " + moTeacherSubjectAssignmentStruct.miSchoolId +
             " , " + moTeacherSubjectAssignmentStruct.miSubjectId +
             " , " + moTeacherSubjectAssignmentStruct.miStandardDivisionId +
             " ,N'" + moTeacherSubjectAssignmentStruct.mbIsExclusive +
             "', " + moTeacherSubjectAssignmentStruct.miTeacherId +
             " , " + moTeacherSubjectAssignmentStruct.miInsertedById +
             " , " + moTeacherSubjectAssignmentStruct.miUpdatedById +
        " ) ";
            return sInsertStatement;

        }

        public string GetUpdateStatementForTeacherSubjectAssignment()
        {
            string sUpdateStatement = " UPDATE Teacher_Subject_Assignment SET " +
            " school_id =  " + moTeacherSubjectAssignmentStruct.miSchoolId +
            " , subject_id =  " + moTeacherSubjectAssignmentStruct.miSubjectId +
            " , standard_division_id =  " + moTeacherSubjectAssignmentStruct.miStandardDivisionId +
            " , IsExclusive =N'" + moTeacherSubjectAssignmentStruct.mbIsExclusive +
            "', inserted_by_id =  " + moTeacherSubjectAssignmentStruct.miInsertedById +
            " , updated_by_id =  " + moTeacherSubjectAssignmentStruct.miUpdatedById +
         " WHERE " +
            " is_deleted = N'" + Constants.C_NO + "'" + 
             " AND  teacher_id =  " + moTeacherSubjectAssignmentStruct.miTeacherId+
             " AND teacher_subject_id=" + moTeacherSubjectAssignmentStruct.miTeacherSubjectId;

            return sUpdateStatement;
        }


        public string GetDeleteStatementForTeacherSubjectAssignment()
        {
            string sUpdateStatement = " DELETE Teacher_Subject_Assignment " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
               " AND teacher_id =  " + moTeacherSubjectAssignmentStruct.miTeacherId+
            " AND teacher_subject_id=" + moTeacherSubjectAssignmentStruct.miTeacherSubjectId;
            return sUpdateStatement;
        }


        public string GetDeleteStatementForTeacherId(int aiTeacherId, int aiSubjectId, int aiStandardDivisionId)
        {
            string sUpdateStatement = " DELETE Teacher_Subject_Assignment " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                " AND subject_id =  " + aiSubjectId +
                " AND standard_division_id =  " + aiStandardDivisionId +
               " AND teacher_id =  " + aiTeacherId;
            return sUpdateStatement;
        }

        public bool IsTeacherAssignedForSubject(int aiSchoolId, int aiTeacherId)
        {
            string sSelectStatement = " SELECT " +
                                    " count(*) " +
                                " FROM " +
                                    " Teacher_subject_assignment " +
                                 " WHERE " +
                                    " School_Id=" + aiSchoolId +
                                    " AND teacher_id=" + aiTeacherId +
                                    " AND Is_Deleted = N'" + Constants.C_NO + "'";

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iCount == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// This method is used to get the teacher name 
        /// for whom the selected subject is assigned.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>

        public DataTable GetSubjectAssignedTeacherName(int aiStandardDivisionId, int aiSubjectId, int aiTeacherId)
        {
            string sTeacherName = " SELECT " +
                                  " TeacherName " +
                                  " ,Teacher_Subject_Id " +
                                " FROM " +
                                   " vw_Get_Subject_Assigned_TeacherName" +
                                " WHERE " +
                                    " Standard_Division_Id =" + aiStandardDivisionId +
                                    " AND Subject_Id =" + aiSubjectId +
                                    " AND Teacher_Id !=" + aiTeacherId +
                                    " AND Is_Deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sTeacherName);
        }

        public void DeleteTeacherSubjectAssignmentForTeacher(int aiTeacherId, int aiSubjectId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sDeleteStmt = GetDeleteStatementForTeacherId(aiTeacherId, aiSubjectId, aiStandardDivisionId);
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStmt);
            }
        }


        public DataTable GetTeacherSubjectIdOfAlreadyAssignedSubject(int aiStandardDivisionId, int aiSubjectId)
        {
            string sTeacherDetails = " SELECT " +
                                  " Teacher_Subject_Id " +
                                  ", Teacher_Id " +
                               " FROM " +
                                   " vw_Get_Subject_Assigned_TeacherName" +
                                " WHERE " +
                                    " Standard_Division_Id =" + aiStandardDivisionId +
                                    " AND Subject_Id =" + aiSubjectId +
                                    " AND Is_Deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sTeacherDetails);
        }

        public DataTable GetListOfTeacherSubjectsforStudent(int aiUserId, int aiSchoolId, int aiAcademicYearId)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
               
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSubjectTeacherList");
            }
        }

        public DataTable GetSubjectAssignedTeacherDetails(int aiStandardDivisionId, int aiSubjectId, int aiSchoolId, int aiAcademicYearId, string sFilter)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Subject_Id", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sFilter", sFilter, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Teacher_Subject");
            }
        }

        public DataSet GetTeacherAndStandardForTT(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTeacherAndStandardForTT");
            }
        }

        public DataTable GetTeacherSubjectDetails(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTeacherSubjectDetails");
            }
        }

        public DataSet GetTeacherSubjectMaxLecDetails(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTeacherSubjectMaxLecDetails");
            }
        }

        /// <summary>
        /// This method is used to return standard division id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public int GetStdDivId(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("StdDivId", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetStandardDivisionId");
                return oSqlParameter.Value.ToInt();
            }
        }

        #endregion
    }


    public class TeacherSubjectAssignmentCollectionDC
    {
        private int miTeacherId = 0;
        public bool mbIsPublished;
        public bool mbToppersGenerated;
        #region constructors
        public TeacherSubjectAssignmentCollectionDC(int aiTeacherId)
        {
            miTeacherId = aiTeacherId;
            mbIsPublished = false;
        }
        public TeacherSubjectAssignmentCollectionDC()
        {
            mbIsPublished = false;
        }
        #endregion

        public void UpdateTeacherSubjects(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public void DeletePreviousSubjectList(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public DataTable GetAllDivisionSubjectsDetailsForTeacher(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Get_Teacher_Subject");
            }
        }



        #region Time table

        public DataSet RetriveTeacherClassSubjectsForTT(int aiSchoolId, int aiAcademicYearId, int aiWeekDayId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekDayId_ID", aiWeekDayId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTeacherSubjectDetails");
            }
        }
        public DataSet RetriveClassesForTT(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStdDivForTimeTable");
            }
        }

        #endregion

        /// <summary>
        /// This method retrives all the Subject passing, failed, absent students count
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
		public DataSet RetriveSubjectsDetailsForClassTeacher(int aiSchool_Id, int aiAcademicYearId, int aiStdDivId, int aiTestId)
        {
            DataSet oDS;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchool_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iTestId", aiTestId, SqlDbType.Int);
                SqlParameter osqlprmIspublished = oSQLServerDbUtility.AddParameter("Is_publish", null, SqlDbType.NVarChar, ParameterDirection.Output, 1);
                SqlParameter osqlprmToppersGenerated = oSQLServerDbUtility.AddParameter("ToppersGenerated", null, SqlDbType.NVarChar, ParameterDirection.Output, 1);
                oDS = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ClassPassFailDetailsForTest");
                mbIsPublished = osqlprmIspublished.Value.ToString() == "Y";
                mbToppersGenerated = osqlprmToppersGenerated.Value.ToString() == "Y";
                return oDS;
            }
        }
        /// <summary>
        /// This method retrives all the current year standard-divisions for the teacher
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>

        public DataTable RetriveSubjectTeachers(int aiAcademicYearId)
        {
            //vw_Get_Subject_Assigned_TeacherName
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSubjectTeachersForAssignExamMarks");
            }
            //string sSelectStatement = " SELECT " +
            //                           " DISTINCT Teacher_Id " +
            //                           ", TeacherName " +
            //                           ", Designation_Id " +
            //                           ",Teacher_First_Name " +
            //                           " FROM vw_Get_Subject_Assigned_TeacherName" +
            //                           " WHERE " +
            //                           "  academic_Year_Id =N'" + aiAcademicYearId + "'" +
            //                           " AND Is_deleted =N'" + Constants.C_NO + "'" +
            //                           " ORDER BY  Designation_Id, Teacher_First_Name ASC ";
            //using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            //    return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method retrives list of classes for the teacher
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>

        public DataTable RetriveSubjectTeacherClass(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {
            //vw_Get_Subject_Assigned_TeacherName
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSubjectTeacherClass");
            }
        }

        /// <summary>
        /// Returns dataset containing 5 tables.
        /// Table 1 - All associated standard-divisions for school
        /// Table 2 - All subjects associated to school
        /// Table 3 - All teachers with their subjects which they can teach
        /// Table 4 - All Subjects for Division
        /// Table 5 - All Subjects Assigned to TeachersName
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataSet GetTeacherSubjectAssociation(int aiSchoolId, int aiAcademicYearId, int aiStandardId, string asSearchText, string asCategoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SearchText", asSearchText, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CategoryId", asCategoryId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardDivisionSubjectsTeacherAssociation");
            }
        }

        /// <summary>
        /// This method is used to return teacher details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abShowHomeworkMtoClassTeacher"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public DataTable RetriveTeachersForHomework(int aiSchoolId, int aiAcademicYearId, bool abShowHomeworkMtoClassTeacher, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowHomeworkMtoClassTeacher", abShowHomeworkMtoClassTeacher, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTeachersForHomework");
            }
        }
    }



}
