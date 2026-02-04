/*
 *  File Name : -- MasterDataDC.cs
 *  Purpose   : -- This Class is used to handle all the database related operations on 
 *                 Master tables -- TurnOver_Master,Countries.
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Utility;
using XseedReportEntities;
using MasterEntities;
using PayrollReportingUserEntities;
using SchoolEntities.Admin;
using BookEntities;

namespace DataCommunicator
{

    public class MasterDataCollectionDC
    {
        public MasterDataCollectionDC()
        {

        }

        const string S_FIELD_NAME_IUSERID = "iUserId";

        public DataTable GetListOfAcedimicYear(int aiSchoolId)
        {
            string sSelectString = " SELECT " +
                                   " YEAR(SchoolWise_Academic_Year_Master.Start_date) As AcademicYear " +
                                   ", SchoolWise_Academic_Year_Master.Academic_Year_ID " +

                                 " FROM " +
                                      "SchoolWise_Academic_Year_Master " +
                                 " INNER JOIN " +
                                      " School_Master " +
                                 " ON " +
                                      " SchoolWise_Academic_Year_Master.School_Id = School_Master.School_Id " +
                                 " WHERE " +
                                      " SchoolWise_Academic_Year_Master.School_Id =" + aiSchoolId +
                                      " AND SchoolWise_Academic_Year_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                      " ORDER BY AcademicYear desc";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectString);
        }

        public static DataTable GetAllSchools()
        {
            string sGetStateCollectionSql = " SELECT " +
                                                  " School_Id " +
                                                  " , School_Name " +
                                                  " , dbo.Udf_GetDefaultMenuForSchool(School_Id) AS Default_Menu_Id " +
                                           " FROM " +
                                               " School_Master " +
                                           " WHERE " +
                                               " is_deleted = N'" + Constants.C_NO + "'" +
                                               " AND Is_Active = N'" + Constants.C_YES + "'" +
                                               " ORDER BY School_Name ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetStateCollectionSql);
        }

        public DataTable GetSalutationType()
        {
            //This method returns the datatable containing collection of all salutation type.
            string sGetSalutationTypeSql = " SELECT " +
                                                   " salutation_id " +
                                                   " , salutation_name " +
                                            " FROM " +
                                                " salutation_master " +
                                            " WHERE " +
                                                " is_deleted = N'" + Constants.C_NO + "'" +
                                                " AND For_Adult = N'" + Constants.C_YES + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetSalutationTypeSql);
        }

        /// <summary>
        /// This method is used to get teachers list to assign as a class teachers.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<ClassTeacher> GetAllTeachersForClassTeacherAssignment(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            List<ClassTeacher> lstClassTeacher = new List<ClassTeacher>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetTeachersForClassTeacherAssignment]"))
                {
                    while (oReader.Read())
                    {
                        lstClassTeacher.Add(new ClassTeacher()
                        {
                            TeacherId = oReader["Teacher_Id"].ToInt(),
                            TeacherName = oReader["TeacherName"].ToString(),
                            IsClassTeacher = oReader["IsClassTeacher"].ToBool(),
                            ClassName = oReader["Class"].ToString()
                        });
                    }
                }
            }
            return lstClassTeacher;
        }

        public DataTable GetAllTeachers(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId)
        {
            //This method returns the datatable containing collection of all salutation type.
            string sGetTeacherName = " SELECT " +
                                      " vw_BaseTeacherDetails.Teacher_Id " +
                                      ", vw_BaseTeacherDetails.TeacherName " +
                                      ", vw_BaseTeacherDetails.Designation_Id " +
                                      ", vw_BaseTeacherDetails.Teacher_First_Name " +
                                   " FROM " +
                                       " vw_BaseTeacherDetails " +
                                   " INNER JOIN " +
                                       " School_Master " +
                                   " ON " +
                                       " School_Master.School_Id = vw_BaseTeacherDetails.School_id " +
                                   " INNER JOIN " +
                                       " User_Master " +
                                   " ON " +
                                       " User_Master.User_Id =  vw_BaseTeacherDetails.User_Id " +
                                   " INNER JOIN " +
                                        " Teacher_Standard_Details " +
                                   " ON vw_BaseTeacherDetails.Teacher_Id = Teacher_Standard_Details.Teacher_Id " +
                                   " WHERE " +
                                       " vw_BaseTeacherDetails.School_Id = School_Master.School_Id " +
                                   " AND vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "' " +
                                   " AND School_Master.School_Id = " + aiSchoolId +
                                   " AND vw_BaseTeacherDetails.Academic_Year_Id = " + aiAcademicYearId +
                                   " AND vw_BaseTeacherDetails.Teacher_Id NOT IN ( " +
                                                                               " SELECT " +
                                                                               " SchoolWise_Standard_Division_Teacher_Assignment_Master.teacher_Id " +
                                                                            " FROM " +
                                                                                " SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                                                                            " INNER JOIN " +
                                                                                " vw_BaseTeacherDetails " +
                                                                            " ON " +
                                                                                " vw_BaseTeacherDetails.Teacher_Id  = SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id  " +
                                                                            " WHERE " +
                                                                                " SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                                                                 " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Academic_Year_Id = N'" + aiAcademicYearId + "' " +
                                                                                  " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = N'" + Constants.C_NO + "' " + ")" +
                                                                                 " AND Teacher_Standard_Details.Standard_Id =" + aiStandardId +
                                                                                 " AND vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "' " +

                               " UNION " +
                                      " SELECT " +
                                      " vw_BaseTeacherDetails.Teacher_Id " +
                                      ", vw_BaseTeacherDetails.TeacherName " +
                                      ", vw_BaseTeacherDetails.Designation_Id " +
                                      ", vw_BaseTeacherDetails.Teacher_First_Name " +
                                       " FROM " +
                                       " vw_BaseTeacherDetails " +
                                   " INNER JOIN " +
                                       " SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                                   " ON " +
                                       " SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id " +
                                   " WHERE " +
                                       " SchoolWise_Standard_Division_Teacher_Assignment_Master.Division_Id =" + aiDivisionId +
                                       " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Standard_Id =" + aiStandardId +
                                       " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_ClassTeacher = N'" + Constants.C_YES + "'" +
                                       " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id " +
                                       " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Academic_Year_Id =" + aiAcademicYearId +
                                       " AND vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "' " +
                                       " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                    " ORDER BY vw_BaseTeacherDetails.Designation_Id, vw_BaseTeacherDetails.Teacher_First_Name ASC";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetTeacherName);
        }

        public DataTable GetAllMobileNosForGivenTeacherUserID(int aiSchoolId, String asUserIDs, int aiAcademicYear)
        {
            //This method returns the datatable containing collection of all salutation type.
            string sGetTeacherName = " SELECT  User_Master.School_Id" +
                                        " , vw_BaseTeacherDetails.Teacher_Id" +
                                        " , vw_BaseTeacherDetails.Mobile_Number" +
                                        " , vw_BaseTeacherDetails.User_Id " +
                                    " FROM  " +
                                        " vw_BaseTeacherDetails INNER JOIN " +
                                        " User_Master " +
                                        " ON vw_BaseTeacherDetails.User_Id = User_Master.User_Id " +
                                    " WHERE  " +
                                        " User_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND vw_BaseTeacherDetails.School_Id = " + aiSchoolId +
                                        " AND vw_BaseTeacherDetails.academic_year_id= " + aiAcademicYear +
                                        " AND vw_BaseTeacherDetails.User_Id IN (" + asUserIDs + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetTeacherName);
        }

        /// <summary>
        /// This method is used to get mobile nos. of all teachers of a class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAllMobileNosForTeachers(int aiSchoolId, int aiAcademicYear)
        {
            //This method returns the datatable containing collection of all salutation type.
            string sGetTeacherName = String.Format("SELECT	b.School_Id, a.Teacher_Id, a.Mobile_Number,a.User_Id " +
                                                   "FROM	vw_BaseTeacherDetails a INNER JOIN User_Master b ON a.User_Id = b.User_Id " +
                                                   "WHERE  b.IsInternalUser=0 AND	b.Is_Deleted = 'N' AND a.academic_year_id= {1} AND b.School_Id = {0} AND b.IsConsideredForMessage = 1", aiSchoolId, aiAcademicYear);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetTeacherName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable RetriveAllTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            string sGetTeacherName = " SELECT " +
                                        " Teacher_Id " +
                                        ", TeacherName " +
                                        ", Designation_Id " +
                                        ", Teacher_First_Name " +
                                     " FROM " +
                                         " vw_StandardDivision_Teacher " +
                                     " WHERE " +
                                        " School_Id =" + aiSchoolId +
                                        " AND  Academic_Year_Id=" + aiAcademicYearId +
                                        " AND Is_Deleted = N'" + Constants.C_NO + "'" +
                                      " ORDER BY Designation_Id, Teacher_First_Name ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetTeacherName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetAllClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllClassTeachers");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetNonPrePrimaryClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetNonPrePrimaryClassTeachers");
            }

        }

        /// <summary>
        /// This method is used to get all class teahcer of those classes for which normal exam configuration is done.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<ClassTeacherDetails> GetClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ACademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClassTeachers"))
                {
                    List<ClassTeacherDetails> lstClassTeachers = new List<ClassTeacherDetails>();
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstClassTeachers.Add(new ClassTeacherDetails()
                                                     {
                                                         StandardDivisionId = oSqlDataReader["StdDivId"].ToInt(),
                                                         TeacherName = oSqlDataReader["TeacherName"].ToString(),
                                                         TeacherId = oSqlDataReader["Teacher_Id"].ToInt()
                                                     });
                        }
                    }
                    return lstClassTeachers;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetPrePrimaryClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPrePrimaryClassTeachers");
            }
        }

        public DataTable GetAllTeachersToAssignSubjects(int aiSchoolId, int aiSubjectId, int aiStandardDivisionId)
        {
            string sGetTeacherName = string.Empty;
            if (aiSchoolId == Constants.SchoolId.PPSN.ToInt())
            {
                sGetTeacherName = " SELECT " +
                                         " vw_BaseTeacherDetails.Teacher_Id " +
                                         ", vw_BaseTeacherDetails.TeacherName " +
                                         ", vw_BaseTeacherDetails.Designation_Id " +
                                         ", vw_BaseTeacherDetails.Teacher_First_Name " +
                                      " FROM " +
                                         " vw_BaseTeacherDetails " +
                                     " INNER JOIN " +
                                        " Teacher_Standard_Details " +
                                      " ON " +
                                        " Teacher_Standard_Details.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id  " +
                                      " INNER JOIN " +
                                        " Teacher_Subject_Details " +
                                      " ON " +
                                        " Teacher_Standard_Details.Teacher_Id = Teacher_Subject_Details.Teacher_Id " +
                                     " INNER JOIN " +
                                        " SchoolWise_Standard_Division_Master " +
                                     " ON " +
                                       " Teacher_Standard_Details.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                                     " INNER JOIN " +
                                        " User_Master " +
                                     " ON " +
                                       " User_Master.User_Id = vw_BaseTeacherDetails.User_Id " +
                                    " LEFT OUTER JOIN " +
                                        " UserBasicDetails " +
                                    " ON " +
                                        " User_Master.User_Id = UserBasicDetails.UserId "   +
                                    " LEFT OUTER JOIN vw_TeacherDesignations VTD ON vw_BaseTeacherDetails.Designation_Id = VTD.Teacher_Designation_Id" +
                                     " WHERE " +
                                        " Teacher_Subject_Details.Subject_Id =" + aiSubjectId +
                                        " AND SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id =" + aiStandardDivisionId +
                                        " AND vw_BaseTeacherDetails.School_Id = " + aiSchoolId +
                                        " AND vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND Teacher_Standard_Details.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND Teacher_Subject_Details.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND User_Master.Is_Locked = 'N'" +
                                     " ORDER BY VTD.DesignationSortOrder, vw_BaseTeacherDetails.Teacher_First_Name ASC,vw_BaseTeacherDetails.Teacher_Middle_Name,vw_BaseTeacherDetails.Teacher_Last_Name";
            }
            else
            {
                sGetTeacherName = " SELECT " +
                                         " vw_BaseTeacherDetails.Teacher_Id " +
                                         ", vw_BaseTeacherDetails.TeacherName " +
                                         ", vw_BaseTeacherDetails.Designation_Id " +
                                         ", vw_BaseTeacherDetails.Teacher_First_Name " +
                                      " FROM " +
                                         " vw_BaseTeacherDetails " +
                                     " INNER JOIN " +
                                        " Teacher_Standard_Details " +
                                      " ON " +
                                        " Teacher_Standard_Details.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id  " +
                                      " INNER JOIN " +
                                        " Teacher_Subject_Details " +
                                      " ON " +
                                        " Teacher_Standard_Details.Teacher_Id = Teacher_Subject_Details.Teacher_Id " +
                                     " INNER JOIN " +
                                        " SchoolWise_Standard_Division_Master " +
                                     " ON " +
                                       " Teacher_Standard_Details.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                                     " INNER JOIN " +
                                        " User_Master " +
                                     " ON " +
                                       " User_Master.User_Id = vw_BaseTeacherDetails.User_Id " +
                                     " LEFT OUTER JOIN vw_TeacherDesignations VTD ON vw_BaseTeacherDetails.Designation_Id = VTD.Teacher_Designation_Id" +
                                     " WHERE " +
                                        " Teacher_Subject_Details.Subject_Id =" + aiSubjectId +
                                        " AND SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id =" + aiStandardDivisionId +
                                        " AND vw_BaseTeacherDetails.School_Id = " + aiSchoolId +
                                        " AND vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND Teacher_Standard_Details.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND Teacher_Subject_Details.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND User_Master.Is_Locked = 'N'" +
                                     " ORDER BY VTD.DesignationSortOrder, vw_BaseTeacherDetails.Teacher_First_Name ASC,vw_BaseTeacherDetails.Teacher_Middle_Name,vw_BaseTeacherDetails.Teacher_Last_Name";
            }
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetTeacherName);
        }

        public string GetStandardName(int aiSchoolId, int aiStandardId)
        {
            string sSelectStandardName = " SELECT " +
                                   " Standard_Name " +
                               " FROM " +
                                   " Standard_Master " +
                                " WHERE " +
                                   " Standard_Id= " + aiStandardId +
                                   " AND School_Id=" + aiSchoolId +
                                   " AND Is_Deleted= N'" + Constants.C_NO + "' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStandardName);
        }

        public string GetDivisionName(int aiSchoolId, int aiDivisionId)
        {
            string sDivisionName = " SELECT " +
                                   " Division_Name " +
                               " FROM " +
                                   " Division_Master " +
                                " WHERE " +
                                   " Division_Id= " + aiDivisionId +
                                   " AND School_Id=" + aiSchoolId +
                                   " AND Is_Deleted= N'" + Constants.C_NO + "' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sDivisionName);
        }

        public string GetClassName(int aiSchoolId, int aiStandardId, int aiDivisionId)
        {
            string sDivisionName = " SELECT " +
                                   " ClassName " +
                               " FROM " +
                                   " vw_standard_division " +
                                " WHERE " +
                                   " Division_Id= " + aiDivisionId +
                                   " AND Standard_Id= " + aiStandardId +
                                   " AND School_Id=" + aiSchoolId;
                                   
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sDivisionName);
        }

        public static DataTable GetListOfClassType()
        {
            //This method returns the datatable containing collection of all types of classes.
            string sGetTypesOfClassesSql = " SELECT " +
                                                   " class_id " +
                                                   ",class_name " +
                                            " FROM " +
                                                " class_master " +
                                            " WHERE " +
                                                " is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sGetTypesOfClassesSql);
        }

        public DataTable GetAllStandardsForSchool(int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        {

            string sSelectStatement = " SELECT DISTINCT " +
                                      " Standard_Master.Standard_Id " +
                                      ", Standard_Master.Standard_Name " +
                                      ", Standard_Master.Original_Standard_Id " +
                                    " FROM " +
                                        " Standard_Master " +
                                    " INNER JOIN " +
                                        " Teacher_Standard_Details " +
                                    " ON " +
                                        " Standard_Master.Standard_Id = Teacher_Standard_Details.Standard_Id " +
                                    " INNER JOIN " +
                                        " SchoolWise_Standard_Division_Master " +
                                    " ON " +
                                        " Standard_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                                    " INNER JOIN " +
                                        " vw_BaseTeacherDetails " +
                                    " ON " +
                                        " Teacher_Standard_Details.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id " +
                                    " INNER JOIN " +
                                        " Division_Master " +
                                    " ON " +
                                        " SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id " +
                                    " WHERE " +
                                        " vw_BaseTeacherDetails.School_Id =" + aiSchoolId +
                                        " AND  Teacher_Standard_Details.Teacher_Id =" + aiTeacherId +
                                        " AND  SchoolWise_Standard_Division_Master.academic_Year_Id =" + aiAcademicYearId +
                                        " AND  vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND  Standard_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND  Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND  Teacher_Standard_Details.Is_Deleted = '" + Constants.C_NO + "'" +
                                        " AND  SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                        " AND SchoolWise_Standard_Division_Master.Division_Id NOT IN (" +
                                                                                        " SELECT " +
                                                                                        " Division_Id " +
                                                                                    " FROM " +
                                                                                        " SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                                                                                    " WHERE " +
                                                                                        " Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                                                                                        " AND Teacher_Id !=" + aiTeacherId +
                                                                                        " AND Is_ClassTeacher = N'" + Constants.C_YES + "'" +
                                                                                        " AND Is_Deleted =  N'" + Constants.C_NO + "'" +
                                                                                        ")" +
                                        " ORDER BY Standard_Master.Original_Standard_Id ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAllDivisionForStandard(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiTeacherId)
        {
            string sSelectStatement = " SELECT DISTINCT " +
                                      "  SchoolWise_Standard_Division_Master.School_Id " +
                                      ", SchoolWise_Standard_Division_Master.Division_Id " +
                                      ", Division_Master.Division_Name " +
                                      ", Standard_Master.Standard_Id " +
                                 " FROM " +
                                       " SchoolWise_Standard_Division_Master " +
                                   " INNER JOIN " +
                                        " Division_Master " +
                                   " ON " +
                                       " SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id  " +
                                   " INNER JOIN " +
                                        " Standard_Master " +
                                   " ON " +
                                        " SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id " +
                                   " INNER JOIN " +
                                        " School_Master " +
                                   " ON  " +
                                       " SchoolWise_Standard_Division_Master.School_Id = School_Master.School_Id " +
                                   " WHERE " +
                                       " School_Master.School_Id =" + aiSchoolId +
                                       " AND Standard_Master.Standard_Id =" + aiStandardId +
                                       " AND Standard_Master.Academic_Year_id =" + aiAcademicYearId +
                                       " AND SchoolWise_Standard_Division_Master.Division_Id NOT IN (" +
                                                            " SELECT SchoolWise_Standard_Division_Teacher_Assignment_Master.Division_Id " +
                                                            " FROM " +
                                                            " SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                                                            " WHERE " +
                                                            " SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_ClassTeacher = N'" + Constants.C_YES + "' " +
                                                            " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id != " + aiTeacherId +
                                                            " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Standard_Id = " + aiStandardId +
                                                            " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = N'" + Constants.C_NO + "' )" +
                                       " AND Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                       " AND SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                       " AND School_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                       " AND Standard_Master.Is_Deleted = N'" + Constants.C_NO + "' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAllUserRoles()
        {
            // This method returns datatable populated with user roles from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT  " +
                                   " User_Role_Id " +
                                   " , User_Role_Name " +
                                   " , Is_Admin" +
                               " FROM " +
                                    " User_Role_Master " +
                               " WHERE " +
                                    " Is_Deleted = N'" + Constants.C_NO + "'" +
                //" AND User_Role_Id <> 8" +
                               " ORDER BY " +
                                    " User_Role_Name";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetDesignations()
        {
            string sSelecStatement = "SELECT " +
                                      "Teacher_Designation_Id" +
                                      ", Teacher_Designation_Name" +
                                      " FROM " +
                                      "Teacher_Designation_Master" +
                                      " WHERE  Is_Deleted='N' and " +
                                      " Teacher_Designation_Id NOT IN (135,140) ORDER BY SortOrder";

            using (SQLServerDbUtility OSQLServerDbUtility = new SQLServerDbUtility())
                return OSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelecStatement);
        }

        public DataTable GetAllUserRolesExceptAdmin()
        {
            // This method returns datatable populated with user roles from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT  " +
                                   " User_Role_Id " +
                                   " , User_Role_Name " + "+'s'" +
                                   " as" +
                                   " User_Role_Name " +
                               " FROM " +
                                    " User_Role_Master " +
                               " WHERE " +
                                    " Is_Deleted = N'" + Constants.C_NO + "'" +
                                    " AND User_Role_Id <> 1" +
                                    " AND User_Role_Id <> 9" +
                               " ORDER BY " +
                                    " User_Role_Id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }


        public DataTable GetAllOcupations()
        {
            string sSelectAllOcupations = "SELECT " +
                                            " Ocupation_Id " +
                                            ",Ocupation_Name " +
                                            " FROM Ocupation_Master " +
                                            " WHERE Ocupation_Master.Is_Deleted= N'" + Constants.C_NO + "'" +
                                            " ORDER BY Ocupation_Name ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectAllOcupations);
        }

        public DataTable GetAllSubCastes(Int32 aiCasteId)
        {
            string sSelectAllCastes = "SELECT " +
                                            "Sub_Caste_Id " +
                                            ",Sub_Caste_Name " +
                                            " FROM Sub_Caste_Master  " +
                                            " WHERE Sub_Caste_Master.Is_Deleted= N'" + Constants.C_NO + "'" +
                                            " AND Sub_Caste_Master.Caste_Id = N'" + aiCasteId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectAllCastes);
        }

        public DataSet GetAllConfiguration(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllConfigurations");
            }
        }

        public DataSet GetAllConfigurationsForAcademicData(int aiSchoolId, int aiAcademicYearId, Boolean bIsOnlyInMidAcademic)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsOnlyInMidAcademic", bIsOnlyInMidAcademic ? "Y" : "N", SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllConfigurationsForAcademicData");
            }
        }

        public DataTable GetStandardIdsOfOriginalStandradId(int aiSchoolId, int aiStandardId)
        {
            string sSelectStatement = " SELECT " +
                                      " SchoolWise_Standard_Division_Master.Standard_Id " +
                                      ",Standard_Master.Original_Standard_Id " +
                                    " FROM " +
                                        " Standard_Master " +
                                    " INNER JOIN " +
                                        " SchoolWise_Standard_Division_Master " +
                                    " ON " +
                                        " SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.standard_id " +
                                    " where " +
                                        " SchoolWise_Standard_Division_Master.School_id =" + aiSchoolId +
                                        " AND Standard_Master.Original_Standard_Id IN (" +
                                                                                    " SELECT " +
                                                                                    " Standard_Master.Original_Standard_Id " +
                                                                                " FROM " +
                                                                                    " Standard_Master " +
                                                                                " WHERE " +
                                                                                    " Standard_Master.standard_id =" + aiStandardId + ")" +
                                       " AND  Standard_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                       " AND  SchoolWise_Standard_Division_Master.Is_Deleted =N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }

        public DataSet GetConfigurationsForFinalAcademicYearGeneration()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetConfigurationsForFinalAcademicYearGeneration");
        }

        public DataTable GetStandardDivisionName(int aiSchoolId, int aiTeacherId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardDivisions");
            }
        }

		public DataTable GetStandardDivisionNameOfStudents(int aiSchoolId, int aiTeacherId, int aiAcademicYearId, int aiLoginUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardDivisionsOfStudents");
            }
        }
		
        public DataTable GetSubjectNameForTeacherStandardDivision(int aiSchoolId, int aiTeacherId, int aiStandardDivisionId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT " +
                                      " Subject_Master.Subject_Id " +
                                      ", Subject_Master.Subject_Name " +
                                    " FROM " +
                                        " Teacher_Subject_Details " +
                                    " INNER JOIN " +
                                        " vw_BaseTeacherDetails  " +
                                    " ON " +
                                        " Teacher_Subject_Details.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id " +
                                    " INNER JOIN " +
                                        " Subject_Master " +
                                    " ON " +
                                        " Teacher_Subject_Details.Subject_Id = Subject_Master.Subject_Id " +
                                    " INNER JOIN " +
                                         " Schoolwise_Division_Subject_Master " +
                                    " ON " +
                                        " Teacher_Subject_Details.Subject_Id = Schoolwise_Division_Subject_Master.Subject_Id " +
                                    " WHERE " +
                                        " vw_BaseTeacherDetails.School_Id =" + aiSchoolId +
                                        " AND Teacher_Subject_Details.Teacher_Id =" + aiTeacherId +
                                        " AND Schoolwise_Division_Subject_Master.Standard_Division_Id =" + aiStandardDivisionId +
                                        " AND Subject_Master.academic_Year_Id =" + aiAcademicYearId +
                                        " AND vw_BaseTeacherDetails.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Teacher_Subject_Details.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Subject_Master.Is_Deleted =  N'" + Constants.C_NO + "' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get standard, division and subject name.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>

        public DataTable GetStandardDivisionSubjectName(int aiStandardDivisionId, int aiSubjectId)
        {
            string sSelectStatement = " SELECT " +
                                     " Standard_Name + ' - ' +  Division_Name As StandardDivisionName" +
                                     " ,Subject_Name " +
                                      " ,Standard_Id " +
                                  " FROM " +
                                     " vw_Standard_Division_Subject " +
                                  " WHERE " +
                                       " Subject_Id =" + aiSubjectId +
                                       " AND Standard_Division_Id =" + aiStandardDivisionId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }

        public DataTable GetGradeNameForFailCriteria(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            string sSelectStatement = " SELECT DISTINCT " +
                                      " Marks_Grades_Configuration_Detail_ID " +
                                      ", Grade_Name " +
                                      ", Starting_Marks_Range " +
                                      ", Actual_Ending_Marks_Range " +
                                     " FROM " +
                                        " vw_GetStandardsAndGradeName " +
                                     " WHERE " +
                                        " School_Id = " + aiSchoolId +
                                        " AND Academic_Year_Id=" + aiAcademicYearId +
                                        " AND Standard_Id = " + aiStandardId +
                                        " ORDER BY Marks_Grades_Configuration_Detail_ID";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetGradeListForFailCriteria(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            string sSelectStatement = " SELECT DISTINCT " +
                                              " vw_GetStandardsAndGradeName.Marks_Grades_Configuration_Detail_ID, vw_GetStandardsAndGradeName.Grade_Name, " +
                                              " vw_GetStandardsAndGradeName.Starting_Marks_Range, vw_GetStandardsAndGradeName.Actual_Ending_Marks_Range " +
                                        " FROM  vw_GetStandardsAndGradeName INNER JOIN " +
                                              " SchoolWise_Standard_Division_Master ON " +
                                              " vw_GetStandardsAndGradeName.Academic_Year_Id = SchoolWise_Standard_Division_Master.academic_year_id AND " +
                                              " vw_GetStandardsAndGradeName.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id AND " +
                                              " vw_GetStandardsAndGradeName.School_Id = SchoolWise_Standard_Division_Master.School_Id " +
                                        " WHERE     (vw_GetStandardsAndGradeName.School_Id = " + aiSchoolId + ") " +
                                              " AND (vw_GetStandardsAndGradeName.Academic_Year_Id = " + aiAcademicYearId + ")" +
                                              " AND (SchoolWise_Standard_Division_Master.Is_Deleted = 'N') " +
                                              " AND SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = " + aiStandardDivisionId +
                                        " ORDER BY vw_GetStandardsAndGradeName.Marks_Grades_Configuration_Detail_ID";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetStandardDivisionNameForMessageDetails(int aiSchoolId, int aiAcademicYearID, int aiTypeId, int aiLoginUserId)
        {
            //This function is used to get the standard divition Name, student count for Message.
          using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStdDivNameForMsgDetails");
            }
        }

        /// <summary>
        /// This method is used to get category id for that category name.
        /// </summary>
        /// <param name="asCategoryName"></param>
        /// <returns></returns>
        public int GetCategoryIdForCategory(string asCategoryName)
        {
            int iCategoryId = 0;
            string sSelectStatement = " SELECT " +
                                          " Category_Id " +
                                      " FROM " +
                                          " Category_Master " +
                                      " WHERE Category_Name=N'" + StringUtility.ReplaceSingleQuoteInString(asCategoryName.ToUpper(), false) + "'" +
                                      " AND Is_Deleted= N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCategoryId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            return iCategoryId;

        }

        /// <summary>
        /// This method is used to get student category name for the Vategory.
        /// </summary>
        /// <param name="asCategoryName"></param>
        /// <returns></returns>       
        public DataSet GetAllFeeCategoriesForImport(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllFeeCategoriesForImport");
            }
        }

        /// <summary>
        /// This method is used to validate fee area name.
        /// </summary>
        /// <param name="asFeeAreaName"></param>
        /// <returns></returns>
        public List<string> GetFeeAreas()
        {
            List<string> lstFeeAreaName = new List<string>();
            string sSelect = " SELECT FeeAreaName " +
                            " FROM FeeAreaName " +
                            " WHERE IsDeleted = 0 ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelect))
                {
                    string sFeeAreaName;
                    while (oSqlDataReader.Read())
                    {
                        sFeeAreaName = oSqlDataReader["FeeAreaName"].ToString();
                        lstFeeAreaName.Add(sFeeAreaName);
                    }
                }
            }
            return lstFeeAreaName;
        }

        /// <summary>
        /// This method is used to get User Types from database.
        /// </summary>        
        /// <returns></returns>
        public DataTable GetAllUserTypes()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())            
               return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllUserTypes");
        }

        /// <summary>
        /// This method is used to get RTE category id for that category name.
        /// </summary>
        /// <param name="asCategoryName"></param>
        /// <returns></returns>
        public int GetRTECategoryIdForCategory(string asRTECategoryName)
        {
            int iRTECategoryId = 0;
            string sSelectStatement = " SELECT " +
                                          " Id " +
                                      " FROM " +
                                          " RTE_CategoryMaster " +
                                      " WHERE CategoryName=N'" + StringUtility.ReplaceSingleQuoteInString(asRTECategoryName.ToUpper(), false) + "'" +
                                      " AND IsDeleted= " + Constants.I_ZERO;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iRTECategoryId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            return iRTECategoryId;

        }

        /// <summary>
        /// This method is used to get parent occupation id from inserted parent occupation. 
        /// </summary>
        /// <param name="asParentOccupation"></param>
        /// <returns></returns>
        public int GetParentOccupationIdForParentOccupationName(string asParentOccupation)
        {
            int iParentOccupationId = 0;
            string sSelectStatement = " SELECT " +
                                         " Ocupation_Id " +
                                      " FROM " +
                                         " Ocupation_Master " +
                                      " WHERE " +
                                         " Ocupation_Name =N'" + StringUtility.ReplaceSingleQuoteInString(asParentOccupation, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iParentOccupationId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            if (iParentOccupationId == 0)
            {
                string sSelectStmt = " SELECT " +
                                          " Ocupation_Id " +
                                        " FROM " +
                                            " Ocupation_Master " +
                                        " WHERE " +
                                            " Ocupation_Name = 'Other'" +
                                            " AND Is_Deleted=N'" + Constants.C_NO + "' ";
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    iParentOccupationId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStmt);

            }
            return iParentOccupationId;

        }

        public int GetRuleIdForRule(string asRule, int iSchoolId, int aiAcademicYrId)
        {
            string sSelectStatement = " SELECT Rule_Id FROM Schoolwise_ConcessionRule " +
                                     " WHERE  [RuleName] = '" + asRule + "' AND " +
                                      " School_Id = " + iSchoolId + " AND " +
                                      " Academic_Year_Id=  " + aiAcademicYrId + " AND " +
                                      " Is_Deleted= " + Constants.I_ZERO;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        public int GetDesignationIdForDesignationName(string asDesignation)
        {
            int iDesignationId = 0;
            string sSelectStatement = " SELECT " +
                                         " Teacher_Designation_Id " +
                                      " FROM " +
                                         " Teacher_Designation_Master " +
                                      " WHERE " +
                                         " Teacher_Designation_Name =N'" + StringUtility.ReplaceSingleQuoteInString(asDesignation, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iDesignationId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iDesignationId;
        }

        public int GetReligionIdForReligionName(string asReligion)
        {
            int iReligionId = 0;
            string sSelectStatement = " SELECT " +
                                         " Religion_Id " +
                                      " FROM " +
                                         " Religion_Master " +
                                      " WHERE " +
                                         " Religion_Name =N'" + StringUtility.ReplaceSingleQuoteInString(asReligion, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iReligionId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iReligionId;

        }

        public int GetQualiIdForQualiName(string asQuali)
        {
            int iQualiId = 0;
            string sSelectStatement = " SELECT " +
                                         " Qualification_Id " +
                                      " FROM " +
                                         " Qualification_Master " +
                                      " WHERE " +
                                         " Qualification_Name =N'" + StringUtility.ReplaceSingleQuoteInString(asQuali, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iQualiId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iQualiId;
        }

        public int GetClassIdForClassName(string asClass)
        {
            int iClassId = 0;
            string sSelectStatement = " SELECT " +
                                         " Class_Id " +
                                      " FROM " +
                                         " Class_Master " +
                                      " WHERE " +
                                         " Class_Name =N'" + StringUtility.ReplaceSingleQuoteInString(asClass, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iClassId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iClassId;

        }

        public int GetSalutationIdForSalutationName(string asSalutation)
        {
            int iSalutationId = 0;
            string sSelectStatement = " SELECT " +
                                         " Salutation_Id " +
                                      " FROM " +
                                         " Salutation_Master " +
                                      " WHERE " +
                                         " Salutation_Name =N'" + StringUtility.ReplaceSingleQuoteInString(asSalutation, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iSalutationId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iSalutationId;

        }


        public int GetLivingLocationIdForLivingLocationName(string asLivingLocation)
        {
            int iLivingLocationId = 0;
            string sSelectStatement = " SELECT " +
                                         " LivingLocationId " +
                                      " FROM " +
                                         " StudentLivingLocations " +
                                      " WHERE " +
                                         " LivingLocationName =N'" + StringUtility.ReplaceSingleQuoteInString(asLivingLocation, false) + "' " +
                                         " AND Is_Deleted=N'" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iLivingLocationId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iLivingLocationId;

        }

        /// <summary>
        /// This method is used to get supportive data for import student admission details.
        /// </summary>
        /// <returns></returns>
        public DataSet GetResidanceTypeMasterDataForadmission()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetMasterDateForValidation");
        }


        /// <summary>
        /// This method retrives all the master data for teacher details.
        /// 	--The data contains:
        ///  1. Salutations
        ///	 2. Categogy 
        ///  3. Designation
        ///	 4. Religions 
        ///	 5. Qualification 
        ///	 6. Class  
        ///	 7. User Roles
        /// </summary>
        /// <returns>
        /// </returns>
        public static DataSet GetAllMasterData()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_MasterData");
        }
        /// <summary>
        /// This method retrives all the master data for student details.
        /// 	--The data contains:
        ///	 1. Categogy 
        ///	 2. Occupation
        /// </summary>
        /// <returns>
        /// </returns>
        public static DataSet GetAllMasterDataForStudent(int iSchoolId, int iAcademicYear, int iStandardId, int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYear", iAcademicYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iDivisionId", aiDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_MasterData_student");
            }
        }
        /// <summary>
        /// This method retrives stream details.
        
        /// </summary>
        /// <returns>
        /// </returns>
        public  DataTable GetAllStreams()
        {
            string sSelectStatement = " SELECT " +
                                         " Id ," +
                                          " Name " +
                                      " FROM " +
                                         " StreamDetails " +
                                      " WHERE " +
                                         
                                         "  IsDeleted=N'" + Constants.I_ZERO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
              return  oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
          
        }
        /// <summary>
        /// This method retrives stream wise group details.

        /// </summary>
        /// <returns>
        /// </returns>
        public DataTable GetAllGroupsOfStream(int aiStream)
        {
            string sSelectStatement = " SELECT " +
                                         " Id ," +
                                          " GroupName " +
                                      " FROM " +
                                         " StreamwiseGroups " +
                                      " WHERE " +
                                         " StreamId =N'" + aiStream + "' " +
                                         " AND IsDeleted=N'" + Constants.I_ZERO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
       
        /// <summary>
        /// This method retrives group wise compulsary subjects details.

        /// </summary>
        /// <returns>
        /// </returns>
        public DataSet GetAllCompulsarySubjects( int aigroup, int aiAcademicYearId)
        {
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
               
                oSQLServerDbUtility.AddParameter("StreamGroupId", aigroup, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStreamwiseSubjectDetails");
            }
        }
        public static DataSet GetAllMasterDataForStudentAdmission(int aiSchoolID, int aiStudentAdmissionId, string acAdmissionForCurrentYear, int aiAcademicYearId = Constants.I_ZERO)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentAdmissionId", aiStudentAdmissionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AdmissionForCurrentYear", acAdmissionForCurrentYear, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_MasterData_Admission");
            }
        }

        /// <summary>
        /// This Method is used to get master details for student registration.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="acAdmissionForCurrentYear"></param>
        /// <returns></returns>
        public static DataSet GetAllMasterDataForStudentRegistration(int aiSchoolId, int aiAcademicYearId, string acAdmissionForCurrentYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AdmissionForCurrentYear", acAdmissionForCurrentYear, SqlDbType.VarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetMasterDataForRegistration");
            }
        }

        public static DataSet GetAllLectureLimings(int aiSchoolID, int aiAcademicYrId, int aiSection)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("aiSchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("aiAcdYrId", aiAcademicYrId, SqlDbType.Int);
                if (aiSection != 0)
                    oSQLServerDbUtility.AddParameter("aiSectionId", aiSection, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetLectureTimings");
            }
        }

        public static DataTable GetConfigurationDetails(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiParentId, int aiScreenLevel, int aiUserId, int aiUserRoleId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParentId", aiParentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetConfigurationsDetails");
            }
        }

        /// <summary>
        /// This method is used to get menu item details for 
        /// </summary>
        /// <param name="aiScreenLevel"></param>
        /// <returns></returns>
        public static DataTable GetMenuItemDetails(int aiScreenLevel)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iScreenLevel", aiScreenLevel, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetMainConfigurationsList");
            }
        }

        public static List<ClassTeacherDetails> GetClassTeacher(int aiSchoolId, int aiAcademicYearId)
        {
            List<ClassTeacherDetails> lstClassTeachers = new List<ClassTeacherDetails>();
            string sSelect = " SELECT DISTINCT " +
                            " Standard_Name + '-' + Division_Name + ' : ' + TeacherName AS TeacherName" +
                            " , Teacher_Id, Designation_Id, Teacher_First_Name" +
                            " , Original_Standard_Id, Original_Division_Id" +
                            " , SchoolWise_Standard_Division_Id" +
                            " FROM vw_ClassTeacher " +
                            " WHERE vw_ClassTeacher.Standard_Id NOT IN (SELECT Standard_Id " +
                                                                    " FROM Xseed.StandardwiseAssessmentMaster " +
                                                                    " WHERE Is_Deleted=N'" + Constants.C_NO + "' " +
                                                                    " AND academic_Year_Id=" + aiAcademicYearId + " AND SchoolId=" + aiSchoolId + " )" +
                            " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                            " AND Academic_Year_Id=" + aiAcademicYearId +
                            " AND School_Id=" + aiSchoolId +
                            " AND Is_ClassTeacher=N'" + Constants.C_YES + "'" +
                            " ORDER BY Original_Standard_Id, Original_Division_Id, Designation_Id, Teacher_First_Name";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelect))
                {
                    ClassTeacherDetails oClassTeachers;
                    while (oSqlDataReader.Read())
                    {
                        oClassTeachers = new ClassTeacherDetails
                        {
                            TeacherId = Convert.ToInt32(oSqlDataReader["Teacher_Id"]),
                            TeacherName = Convert.ToString(oSqlDataReader["TeacherName"]),
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["SchoolWise_Standard_Division_Id"])

                        };
                        lstClassTeachers.Add(oClassTeachers);
                    }
                }
            }
            return lstClassTeachers;
        }

        public static List<DesignationMaster> GetDesignationsDetails(int aiUserId, int aiUserRoleId, int aiFilter, int aiSchoolId, int aiAcademicYearId)
        {
            List<DesignationMaster> lstDesignation = new List<DesignationMaster>();
            DesignationMaster oDesignationMaster = null;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", aiFilter, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForAssignTask"))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oDesignationMaster = new DesignationMaster
                            {
                                DesignationId = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                                Designation = oSqlDataReader["Display_Member"].ToString()
                            };
                            lstDesignation.Add(oDesignationMaster);
                        }
                    }
                }
            };
            return lstDesignation;
        }

        /// <summary>
        /// This method is used to get the selected roles.
        /// </summary>
        /// <returns></returns>
        public static List<UserRoles> GetUserRoles()
        {
            List<UserRoles> olstUserRoles = new List<UserRoles>();
            string sSqlStatement = "SELECT User_Role_Id,User_Role_Name" +
                                 " FROM" +
                                 " User_Role_Master" +
                                 " WHERE" +
                                 " Is_Deleted = 'N'" +
                                 " AND User_Role_Id NOT IN(" +
                                 Convert.ToInt32(Constants.UserRoles.Student) + "," +
                                 Convert.ToInt32(Constants.UserRoles.TransportStaff) + "," +
                                 Convert.ToInt32(Constants.UserRoles.Parent) + "," +
                                 Convert.ToInt32(Constants.UserRoles.OtherStaff) +
                                 ");";

            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                GenericClass<UserRoles> oGenricClass = new GenericClass<UserRoles>();
                olstUserRoles = oGenricClass.GetFilledObjectList(oReader);
            }

            return olstUserRoles;
        }

        /// <summary>
        /// This method is used for getting the users for selected role.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiRoleId"></param>
        /// <returns></returns>
        public static List<AttendanceAlertConfigDetails> GetUsers(int aiRoleId, int aiSchoolId, int aiAcademicYearId)
        {
            List<AttendanceAlertConfigDetails> olstAttendanceConfigDetails = new List<AttendanceAlertConfigDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiRoleId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRolewiseUserDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        AttendanceAlertConfigDetails oAttendanceAlertConfigDetails = new AttendanceAlertConfigDetails
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["User_Id"]),
                            UserName = oSqlDataReader["UserName"].ToString()
                        };
                        olstAttendanceConfigDetails.Add(oAttendanceAlertConfigDetails);
                    }
                }
            }
            return olstAttendanceConfigDetails;
        }

        /// <summary>
        /// This method is used for get all user information based on the selected role.
        /// </summary>
        /// <param name="aiRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<int> GetAllUsersIdForUserRole(int aiRoleId, int aiSchoolId, int aiAcademicYearId, string asStdDivIds)
        {
            List<int> lstUserIds = new List<int>();
            string sAllUserId = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivIds", asStdDivIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRolewiseUserIds"))
                {
                    while (oSqlDataReader.Read())
                    {
                            lstUserIds.Add((int)oSqlDataReader["User_Id"]);
                    }
                }
            }
            return lstUserIds;
        }
        /// <summary>
        /// This method is used to get Rolewise Users details.
        /// </summary>
        /// <param name="aiRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<ReportingUserConfiguration> GetReportingUsers(int aiRoleId, int aiSchoolId, int aiAcademicYearId)
        {
            List<ReportingUserConfiguration> olstReportingUserConfiguration = new List<ReportingUserConfiguration>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiRoleId, SqlDbType.Int);
                // SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRolewiseReportingUserDetails");
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRolewiseUserDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ReportingUserConfiguration oReportingUserConfiguration = new ReportingUserConfiguration
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["User_Id"]),
                            UserName = oSqlDataReader["UserName"].ToString()
                        };
                        olstReportingUserConfiguration.Add(oReportingUserConfiguration);
                    }
                }
            }
            return olstReportingUserConfiguration;
        }
        /// <summary>
        /// This method is used to get list of all qualifications.
        /// </summary>
        /// <returns></returns>
        public static List<Qualification> GetAllQualification()
        {
            List<Qualification> lstQualifications = new List<Qualification>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllQualification"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstQualifications.Add(new Qualification
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Qualification_Id"]),
                            Name = oSqlDataReader["Qualification_Name"].ToString()
                        });
                    }
                }
            }
            return lstQualifications;
        }

        /// <summary>
        /// This method return the user roles that will be applicable to mailing group functionality.
        /// </summary>
        /// <returns></returns>
        public static List<UserRoles> GetUserRolesForMailingList()
        {
            List<UserRoles> olstUserRoles = new List<UserRoles>();
            string sSqlStatement = "SELECT User_Role_Id,User_Role_Name" +
                                 " FROM" +
                                 " User_Role_Master" +
                                 " WHERE" +
                                 " Is_Deleted = 'N'" +
                                 " AND User_Role_Id NOT IN(" +
                //Convert.ToInt32(Constants.UserRoles.Student) + "," +
                                 Convert.ToInt32(Constants.UserRoles.TransportStaff) + "," +
                                 Convert.ToInt32(Constants.UserRoles.Parent) + "," +
                                 Convert.ToInt32(Constants.UserRoles.OtherStaff) +
                                 ");";

            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                GenericClass<UserRoles> oGenricClass = new GenericClass<UserRoles>();
                olstUserRoles = oGenricClass.GetFilledObjectList(oReader);
            }

            return olstUserRoles;
        }

        /// <summary>
        /// This method return all the user roles.
        /// </summary>
        /// <returns></returns>
        public static List<UserRoles> GetAllRoles()
        {
            List<UserRoles> olstUserRoles = new List<UserRoles>();
            string sSqlStatement = "SELECT User_Role_Id,User_Role_Name" +
                                 " FROM" +
                                 " User_Role_Master" +
                                 " WHERE" +
                                 " Is_Deleted = 'N' ";

            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                GenericClass<UserRoles> oGenricClass = new GenericClass<UserRoles>();
                olstUserRoles = oGenricClass.GetFilledObjectList(oReader);
            }

            return olstUserRoles;
        }

        /// <summary>
        /// This method return whether passing user id is class teacher or not.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static bool IsClassTeacher(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            bool bResult = false;
            using (SQLServerDbUtility oSqlServerDbUtility = new SQLServerDbUtility())
            {
                oSqlServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_IsClassTeacher"))
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
    }
}
