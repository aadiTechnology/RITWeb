//// File Name  : SurveySchoolDetailsDC.cs
//// Created By : Yogesh
//// Date       : 31/10/2015
//// Description :This class is used to maintain data communication logic survey school record details functionality. 
////   

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
namespace DataCommunicator
{
    public class SurveySchoolDetailsDC
    {
        #region Public Method(s)

        /// <summary>
        /// This method is used to get all survey school details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public static List<SurveySchool> GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            List<SurveySchool> lstSchoolSettings = new List<SurveySchool>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSurveySchoolDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSchoolSettings.Add(new SurveySchool { Id = Convert.ToInt32(oSqlDataReader["Id"]), Name = Convert.ToString(oSqlDataReader["Name"]) });
                    }
                }
            }
            return lstSchoolSettings;
        }

        /// <summary>
        /// This method is used to add survey school details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiServaySchoolId"></param>
        /// <param name="asServaySchoolName"></param>
        /// <param name="aiUserId"></param>
        public static string Save(int aiSchoolId, int aiAcademicYearId, int SurveySchoolId, string SurveySchoolName, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServaySchoolId", SurveySchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServaySchoolName", SurveySchoolName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                SqlParameter oSqlParam = oSQLServerDbUtility.AddParameter("DuplicationErr", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AddSurveySchoolDetails");
                return Convert.ToString(oSqlParam.Value);
            }
        }

        /// <summary>
        /// This method is used to delete survey school details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiServaySchoolId"></param>
        /// <param name="asServaySchoolName"></param>
        /// <param name="aiUserId"></param>
        public static void Delete(int aiSchoolId, int aiAcademicYearId, int aiServaySchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServaySchoolId", aiServaySchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteSurveySchoolDetails");
            }
        }

        #endregion
    }
}
