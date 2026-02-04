using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Utility;
using System.Data.SqlClient;
using XseedReportEntities;

namespace DataCommunicator
{
    public class TeacherStandardDetailsDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct TeacherStandardDetailsStruct
        {
            public int miTeacherStandardId;
            public int miTeacherId;
            public int miStandardId;
            public string msStandardName;
            public int miInsertedById;
            public int miUpdatedById;
        }
        TeacherStandardDetailsStruct moTeacherStandardInfoStruct;

        #endregion

        #endregion

        #region DataMembers and properties

        public TeacherStandardDetailsStruct TeacherStandardInfoStructure
        {

            get { return moTeacherStandardInfoStruct; }
            set { moTeacherStandardInfoStruct = value; }
        }

        #endregion

        #region Constructors

        public TeacherStandardDetailsDC()
        {
            moTeacherStandardInfoStruct.miTeacherId = 0;
        }

        #endregion

        #region " Public Methods "

        /// <summary>
        /// constructs a statement for inserting an item.
        /// </summary>
        /// <returns></returns>
        public string GetStandardDetailsInsertStatement()
        {
            string sTeacherId;
            if (moTeacherStandardInfoStruct.miTeacherId != 0)
                sTeacherId = "   " + moTeacherStandardInfoStruct.miTeacherId;
            else
                sTeacherId = "   " + Constants.S_LAST_INSERTED_P_KEY;

            string sInsertStatement = "INSERT INTO Teacher_Standard_Details (" +
                                  " Teacher_Id " +
                                  ",Standard_Id " +
                //  ",Comments" +
                                  ",Inserted_By_id " +
                                  ",Updated_By_Id " +

                " ) VALUES ( " +
                         sTeacherId +
                    ",   " + moTeacherStandardInfoStruct.miStandardId +
                    " ,  " + moTeacherStandardInfoStruct.miInsertedById +
                    " ,  " + moTeacherStandardInfoStruct.miUpdatedById +
            " ) ";

            return sInsertStatement;
        }

        public DataTable FetchStandardDetailsForTeacherId(int aiTeacherId)
        {
            string sFetchSubjectsDetails = " SELECT " +
                                           " Teacher_Standard_Details.Standard_Id " +
                                           ", Standard_Master.Standard_Name " +
                                           ", Standard_Master.Original_Standard_Id " +
                                           ", Standard_Master.School_Id " +
                                       " FROM  " +
                                           " Standard_Master " +
                                       " INNER JOIN " +
                                            " Teacher_Standard_Details " +
                                       " ON Standard_Master.Standard_Id = Teacher_Standard_Details.Standard_Id " +
                                       " INNER JOIN " +
                                           " vw_BaseTeacherDetails " +
                                       " ON Teacher_Standard_Details.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id " +
                                       " WHERE " +
                                         " Teacher_Standard_Details.Teacher_Id = " + aiTeacherId +
                                         " AND Teacher_Standard_Details.Is_Deleted =N'" + Constants.C_NO + "'" +
                                         " AND Standard_Master.Is_Deleted =N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchSubjectsDetails);
        }

        public DataTable FetchStandardDetailsForEditDetails(int aiTeacherId, int aiSchoolId, int aiAcademicYearId)
        {

            string sFetchStandardsDetails = " SELECT " +
                                           " Teacher_Standard_Details.Standard_Id " +
                                           ",Standard_Master.Original_Standard_Id " +
                                           ",Teacher_Standard_Details.Teacher_Id " +
                                           ",Standard_Master.Standard_Name " +
                                      " FROM " +
                                           " Teacher_Standard_Details " +
                                      " INNER JOIN " +
                                           " Standard_Master " +
                                      " ON Teacher_Standard_Details.Standard_Id = Standard_Master.Standard_Id " +
                                      " WHERE " +
                                          " Teacher_Standard_Details.Teacher_Id =" + aiTeacherId +
                                          " AND Standard_Master.Academic_Year_Id =" + aiAcademicYearId +
                                          " AND Standard_Master.Is_Deleted =N'" + Constants.C_NO + "'" +
                                          " AND Teacher_Standard_Details.Is_Deleted =N'" + Constants.C_NO + "'" +
                               " UNION " +
                                  " SELECT  " +
                                  " Standard_Id " +
                                  ",Original_Standard_Id " +
                                  ",'0000' As Teacher_Id " +
                                  ",Standard_Master.Standard_Name " +
                               " FROM " +
                                  " Standard_master " +
                               " WHERE " +
                                   " School_Id = " + aiSchoolId +
                                   " AND Academic_Year_Id =" + aiAcademicYearId +
                                   " AND Standard_Master.Is_Deleted =N'" + Constants.C_NO + "'" +
                                   " AND Original_Standard_Id NOT IN (" +
                                                             " SELECT " +
                                                                 " Standard_Master.Original_Standard_Id " +
                                                             " FROM " +
                                                                 " Teacher_Standard_Details " +
                                                             " INNER JOIN " +
                                                                 " Standard_Master " +
                                                             " ON Teacher_Standard_Details.Standard_Id = Standard_Master.Standard_Id " +
                                                             " WHERE " +
                                                                 " Teacher_Standard_Details.Teacher_Id =" + aiTeacherId +
                                                                 " AND Standard_Master.Academic_Year_Id =" + aiAcademicYearId +
                                                                 " AND Standard_Master.Is_Deleted =N'" + Constants.C_NO + "'" +
                                                                 " AND Teacher_Standard_Details.Is_Deleted =N'" + Constants.C_NO + "'" +
                                                            ")" +
                                " ORDER BY  Standard_Master.Original_Standard_Id ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchStandardsDetails);
        }

        public DataTable IsTeacherPrePrimary(int iSchoolId, int iAcademicYearId, int iTeacherId)
        {
            string sQueryString = "SELECT     Standard_Master.Is_PrePrimary " +
                                 " FROM Standard_Master INNER JOIN " +
                                 " SchoolWise_Standard_Division_Teacher_Assignment_Master ON " +
                                 " Standard_Master.Standard_Id = SchoolWise_Standard_Division_Teacher_Assignment_Master.Standard_Id AND " +
                                 " Standard_Master.School_Id = SchoolWise_Standard_Division_Teacher_Assignment_Master.School_Id AND " +
                                 " Standard_Master.academic_Year_Id = SchoolWise_Standard_Division_Teacher_Assignment_Master.Academic_Year_Id " +
                                 " WHERE     (Standard_Master.Is_PrePrimary = 'Y') " +
                                 " AND (Standard_Master.School_Id = " + iSchoolId + ") " +
                                 " AND (Standard_Master.academic_Year_Id = " + iAcademicYearId + ") AND " +
                                 " (SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_ClassTeacher = 'Y') AND " +
                                 " (SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = 'N') " +
                                 " AND (Standard_Master.Is_Deleted = 'N') AND " +
                                 " (SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id = " + iTeacherId + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQueryString);
        }

        public List<ClassTeacherDetails> GetClassTeachersForOptionalSubjectClasses(int aiAcademicYearId, int aiSchoolId, int aiTeacherId)
        {
            List<ClassTeacherDetails> olstClassTeacherDetails = new List<ClassTeacherDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClassTeachersForOptionalSubjectClasses"))
                {
                    GenericClass<ClassTeacherDetails> oStudentInfo = new GenericClass<ClassTeacherDetails>();
                    olstClassTeacherDetails = oStudentInfo.GetFilledObjectList(oSqlDataReader);
                }
            }

            return olstClassTeacherDetails;
        }

		/// <summary>
		/// This method returns true if pre-primary configuration is done for standard.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiTeacherId"></param>
		/// <returns></returns>
		public bool IsPreprimaryExamConfiguration(int aiSchoolId,int aiAcademicYearId, int aiStdDivId,string asUserRole)
		{
			bool bResult = false;
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("Id", aiStdDivId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("UserRole", asUserRole, SqlDbType.VarChar);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("ups_IsPrePrimaryExamConfiguration"))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        oSqlDataReader.Read();
                        bResult = oSqlDataReader["Result"].ToBool();
                    }
                }
            }
			return bResult;
		}

	    #endregion

    }

    public class TeacherStandardDetailsCollectionDC : DataCommunicatorBaseDC
    {
        int miSchool_Id;
        int miAcademic_Year_Id;

        public TeacherStandardDetailsCollectionDC()
        {
        }

        public TeacherStandardDetailsCollectionDC(int aiSchool_Id, int aiAcademic_Year_Id)
        {
            miSchool_Id = aiSchool_Id;
            miAcademic_Year_Id = aiAcademic_Year_Id;
        }

        public bool DeleteTeacherStandardDetails(ArrayList aoArrDeleteTeacherIds)
        {
            string sDeleteTeacherIdList = "(";
            for (int iCount = 0; iCount < aoArrDeleteTeacherIds.Count; iCount++)
            {
                sDeleteTeacherIdList = sDeleteTeacherIdList + aoArrDeleteTeacherIds[iCount];
                sDeleteTeacherIdList = sDeleteTeacherIdList + ",";
            }
            sDeleteTeacherIdList = sDeleteTeacherIdList + ")";
            sDeleteTeacherIdList = sDeleteTeacherIdList.Remove(sDeleteTeacherIdList.Length - 2, 1);

            string sSqlDeleteEducationDetails = " UPDATE Teacher_Standard_Details " +
                                " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                " WHERE Teacher_Id in " + sDeleteTeacherIdList;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteEducationDetails);
            return true;
        }


        public bool DeleteTeacherStandardDetails(int aiTeacherId)
        {
            string sSqlDeleteEducationDetails = " UPDATE Teacher_Standard_Details " +
                                 " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                 " WHERE Teacher_Id =" + aiTeacherId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteEducationDetails);
            return true;
        }

        public static string RemoveAllStandardsForTeacherId(int aiTeacherId)
        {
            // This procedure accepts parameter as asBusinessId. This method logically deletes all the 
            // locations for the passed businessid from the database.
            string sDeleteStatement;

            sDeleteStatement = " DELETE Teacher_Standard_Details " +
                               " WHERE " +
                                   " teacher_id in (" + aiTeacherId + ")";

            return sDeleteStatement;
        }

        public Int32 GetStdDivIdOfClassTeacher(int aiTeacherId)
        {
            // This procedure accepts parameter as asBusinessId. This method logically deletes all the 
            // locations for the passed businessid from the database.
            string sStmtQry;

            sStmtQry = " SELECT SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id " +
                                " FROM   SchoolWise_Standard_Division_Teacher_Assignment_Master INNER JOIN " +
                                      " SchoolWise_Standard_Division_Master ON " +
                                      " SchoolWise_Standard_Division_Teacher_Assignment_Master.School_Id = SchoolWise_Standard_Division_Master.School_Id AND " +
                                      " SchoolWise_Standard_Division_Teacher_Assignment_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id AND " +
                                      " SchoolWise_Standard_Division_Teacher_Assignment_Master.Division_Id = SchoolWise_Standard_Division_Master.Division_Id " +
                                " WHERE     (SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id ='" + aiTeacherId + "')" +
                                    " AND (SchoolWise_Standard_Division_Master.School_Id ='" + miSchool_Id + "')" +
                                    " AND (SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_ClassTeacher =N'" + Constants.C_YES + "')" +
                                    " AND (SchoolWise_Standard_Division_Teacher_Assignment_Master.Academic_Year_Id =N'" + miAcademic_Year_Id + "')";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStmtQry);
            //return using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            //oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sStmtQry);
        }

		/// <summary>
		/// /* This method is used to get class teacher of provided class*/
		/// </summary>
		/// <param name="aiStdDivId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public int GetClassTeacher(int aiStdDivId, int aiSchoolId, int aiAcademicYearId)
		{
			int iClassTecherId = 0;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);

				using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClassTeacherId"))
					if (oReader.HasRows && oReader.Read())
						iClassTecherId= oReader["Teacher_Id"].ToInt();
				return iClassTecherId;
			}
		}

        public char CheckIfStandardHasOnlyGradeSystem(int aiStdDivId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
				oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Result", null, SqlDbType.NVarChar, ParameterDirection.Output,1);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CheckIfStandardHasOnlyGradeSystem");
                return Convert.ToChar(oSqlParameter.Value);
            }
        }

        public bool IsMonthConfiguration(int aiStandardDivisionId)
        {
			string sSelectStatement = " IF NOT EXISTS (SELECT      PPNPR.PrePrimaryStandardId " +
									  " FROM  SchoolWise_Standard_Division_Master SSDM INNER JOIN " +
									  " PrePrimaryStandardForNewProgressReport PPNPR ON SSDM.Standard_Id = PPNPR.Standard_Id " +
									  " AND SSDM.academic_year_id=PPNPR.Academic_Year_Id" +
									  " WHERE SSDM.SchoolWise_Standard_Division_Id = " + aiStandardDivisionId +
									   " AND PPNPR.Is_Deleted =0" +
									   "AND  PPNPR.academic_year_id="+miAcademic_Year_Id+
									   ") " +
                                      " SELECT 0 " +
                                      " ELSE " +
                                      " SELECT 1 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return Convert.ToBoolean(oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement));
        }

        public static DataTable GetTeachersForPrePrimaryProgressReport(int aiStdDivId, int aiSchoolId, int aiAcademicYearId)
        {
			string sSelectStatement = " SELECT Teacher_Id " +
									  " FROM vw_ClassTeacher " +
									  " WHERE SchoolWise_Standard_Division_Id " +
									  " IN( SELECT SchoolWise_Standard_Division_Id " +
									  " FROM SchoolWise_Standard_Division_Master " +
									  " WHERE Standard_Id IN " +
									  " (SELECT Standard_Id " +
									  " FROM PrePrimaryStandardForNewProgressReport " +
									  " WHERE Is_Deleted = 0 AND School_Id=" + aiSchoolId +
									  " AND academic_Year_Id=" + aiAcademicYearId + "))" +
									  " AND SchoolWise_Standard_Division_Id= " + aiStdDivId;

			 //string sSelectStatement =" SELECT Teacher_Id "+
			 //                         " FROM vw_ClassTeacher "+
			 //                         " WHERE SchoolWise_Standard_Division_Id ="+aiTeacherId+
			 //                         " AND School_Id="+aiSchoolId+
			 //                         " AND academic_year_id="+aiAcademicYearId+
			 //                         " AND Is_Deleted='N'";
                      
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
    }
}
