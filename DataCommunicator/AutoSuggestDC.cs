// -----------------------------------------------------------------------
// <copyright file="GetDataForAutoSuggest.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
	public class AutoSuggestDC
	{
	/// <summary>
	/// This method is returns all records for auto complete search for given filter and result set.
	/// </summary>
	/// <param name="asResult"></param>
	/// <param name="asFilter"></param>
	/// <returns></returns>
        public static List<Student> GetStudentDataForAutoSearch(int aiSchoolId, int aiAcademicYearId, string asYearwiseStudentIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentIds", asYearwiseStudentIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDataForAutoSearch"))
                {
                    List<Student> lstStudents = new List<Student>();
                    if (oSqlDataReader != null)
                    {
                        Student oStudent;
                        while (oSqlDataReader.Read())
                        {
                            oStudent = new Student
                            {
                                StudentId = Convert.ToInt32(oSqlDataReader["StudentId"]),
                                StandardId = Convert.ToInt32(oSqlDataReader["StandardId"]),
                                DivisionId = Convert.ToInt32(oSqlDataReader["DivisionId"]),
                                StdDivId = Convert.ToInt32(oSqlDataReader["StdDivId"]),
                                Name = oSqlDataReader["Name"].ToString(),
                                RegistraionNo = oSqlDataReader["RegistrationNo"].ToString(),
                                IsLeft = Convert.ToBoolean(oSqlDataReader["IsLeftStudent"]),
                                YearWiseStudentId = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                                NameFL = oSqlDataReader["NameFL"].ToString()
                            };

                            lstStudents.Add(oStudent);
                        }
                    }
                    return lstStudents;
                }
            }
        }
        /// <summary>
        /// This method is returns all records for auto complete search for given filter and result set.
        /// </summary>
        /// <param name="asResult"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public static List<Staff> GetStaffDataForAutoSearch(int aiSchoolId, int aiAcademicYearId, string asUserIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserIds", asUserIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStaffDataForAutoSearch"))
                {
                    List<Staff> lstUsers = new List<Staff>();
                    if (oSqlDataReader != null)
                    {
                        Staff oStaff;
                        while (oSqlDataReader.Read())
                        {
                            oStaff = new Staff
                            {
                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                UserRoleId = Convert.ToInt32(oSqlDataReader["UserRoleId"]),
                                Name = oSqlDataReader["Name"].ToString(),
                                IsDeleted = Convert.ToBoolean(oSqlDataReader["IsDeleted"]),
                                NameFL = oSqlDataReader["NameFL"].ToString(),
                                StatusId = Convert.ToInt32(oSqlDataReader["StatusId"])
                            };

                            lstUsers.Add(oStaff);
                        }
                    }
                    return lstUsers;
                }
            }
        }

        public static List<AutoSearchUser> GetUserDataForMessageCenter(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId, out List<TeacherClassAsso> lstAsso)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                lstAsso = new List<TeacherClassAsso>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDataForMessageCenter"))
                {
                    List<AutoSearchUser> lstUsers = new List<AutoSearchUser>();
                    if (oSqlDataReader != null)
                    {   
                        while (oSqlDataReader.Read())
                        {
                            lstAsso.Add(
                                new TeacherClassAsso 
                                {
                                    StdDivId = oSqlDataReader["StdDivId"].ToInt(),
                                    TeacherUserId = oSqlDataReader["TeacherUserId"].ToInt()
                                }
                                );
                        }

                        oSqlDataReader.NextResult();
                        AutoSearchUser oAutoSearchUser;
                        while (oSqlDataReader.Read())
                        {
                            oAutoSearchUser = new AutoSearchUser
                            {
                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                UserRoleId = Convert.ToInt32(oSqlDataReader["UserRoleId"]),
                                Name = oSqlDataReader["Name"].ToString(),
                                HasFullAccess = Convert.ToBoolean(oSqlDataReader["HasFullAccess"]),
                                NameFL = oSqlDataReader["NameFL"].ToString(),
                                StdDivId = Convert.ToInt32(oSqlDataReader["StdDivId"]),
                                IsCoordinator = Convert.ToBoolean(oSqlDataReader["IsCoordinator"])
                            };

                            lstUsers.Add(oAutoSearchUser);
                        }
                    }
                    return lstUsers;
                }
            }
        }
    }
}
