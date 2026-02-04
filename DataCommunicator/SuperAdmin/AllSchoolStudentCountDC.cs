using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class AllSchoolStudentCountDC : DataCommunicatorBaseDC
    {
        #region --Data Members--

        private string connectionString = "Data Source= " + ConfigurationManager.AppSettings["SchoolLocationsDataSource"] + "; Database=" + ConfigurationManager.AppSettings["SchoolLocationsDataBaseName"]
                            + "; User ID=" + ConfigurationManager.AppSettings["SchoolLocationsUserId"] + "; Password=" + ConfigurationManager.AppSettings["SchoolLocationsPassword"];
        #endregion

        #region --Public Methods--
        /// <summary>
        /// This method is used to get all academic years.
        /// </summary>
        /// <returns></returns>
        public List<AcademicYear> GetAllAcademicYears()
        {
            List<AcademicYear> lstAcademicYears = new List<AcademicYear>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(connectionString))
            {
                using (SqlDataReader osqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAcademicYears"))
                {
                    AcademicYear oAcademicYear = null;
                    while (osqlDataReader.Read())
                    {
                        lstAcademicYears.Add(
                                oAcademicYear = new AcademicYear
                                {
                                    Id = Convert.ToInt32(osqlDataReader["Academic_Year_ID"]),
                                    Year = Convert.ToString(osqlDataReader["Year"])
                                }
                        );
                    }
                }
                return lstAcademicYears;
            }
        }

        /// <summary>
        /// This method is used to get list of all schools student count.
        /// </summary>
        /// <returns></returns>
        public List<AllStudentCount> GetStudentsCountList(string asAcademicYear)
        {
            string[] years = asAcademicYear.Split('-');

            List<ConnectionDetails> lstConnectionDetails = new List<ConnectionDetails>();
            List<AllStudentCount> lstStudentsCount = new List<AllStudentCount>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(connectionString))
            {
                using (SqlDataReader osqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetConnectionDetails"))
                {
                    ConnectionDetails oConnectionDetails = null;
                    while (osqlDataReader.Read())
                    {
                        lstConnectionDetails.Add(
                                oConnectionDetails = new ConnectionDetails
                                {
                                    SchoolId = Convert.ToInt32(osqlDataReader["SchoolId"]),
                                    DatabaseServer = Convert.ToString(osqlDataReader["SchoolDatabaseServer"]),
                                    DatabaseName = Convert.ToString(osqlDataReader["SchoolDatabaseName"]),
                                    UserId = Convert.ToString(osqlDataReader["SchoolUserID"]),
                                    Password = Convert.ToString(osqlDataReader["SchoolPassword"])
                                }
                        );
                    }
                }
            }

            foreach (var connection in lstConnectionDetails)
            {
                string schoolPassword = CommonUtility.GetDecryptedPassword(connection.UserId, connection.Password);
                string schoolConnectionString = "Data Source= " + connection.DatabaseServer + "; Database=" + connection.DatabaseName
                             + "; User ID=" + connection.UserId + "; Password=" + schoolPassword;

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(schoolConnectionString))
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", connection.SchoolId, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("AcademicStartYear", years[0], SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("AcademicEndYear", years[1], SqlDbType.NVarChar);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolStudentCount"))
                    {

                        AllStudentCount oAllStudentCount = null;
                        while (oSqlDataReader.Read())
                        {
                            lstStudentsCount.Add
                                (
                                    oAllStudentCount = new AllStudentCount
                                    {
                                        SchoolId = Convert.ToInt32(oSqlDataReader["School_Id"]),
                                        SchoolName = Convert.ToString(oSqlDataReader["SchoolName"]),
                                        Total = Convert.ToString(oSqlDataReader["Total"]),
                                        Girls = Convert.ToString(oSqlDataReader["Girls"]),
                                        Boys = Convert.ToString(oSqlDataReader["Boys"])
                                    }
                                );
                        }
                    }
                }
            }
            return lstStudentsCount;
        }
        #endregion
    }
}