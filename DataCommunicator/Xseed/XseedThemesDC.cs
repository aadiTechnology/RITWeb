using System;
using System.Collections.Generic;
using System.Data;
using Utility;
using System.Data.SqlClient;
using XseedReportEntities;

namespace DataCommunicator
{
    public class XseedThemesDC
    {
        #region "Private member"
        int miSchoolId = 0;
        int miAcademicYearId = 0;
        #endregion
        #region "Constructor"
        public XseedThemesDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }

        public XseedThemesDC(int aiSchoolId, int aiAcademicyearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicyearId;
        }

        #endregion
        #region "Public method"
        /// <summary>
        /// This method is used to save Xseed Theme details.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiFlag"></param>
        public void Save(int aiStandardwiseAssessmentId, string asTheme, int aiSortOrder,int aiThemeId, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
              
                    oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StandardwiseAssessmentId", aiStandardwiseAssessmentId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SortOrder", aiSortOrder, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Theme", asTheme, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("ThemeId", aiThemeId, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Xseed.usp_InsertXseedThemeDetails");
            };
        }
        /// <summary>
        /// This method is used to get theme details.
        /// </summary>
        /// <param name="asFilter"></param>
        /// <param name="asOrder"></param>
        /// <returns></returns>
        public static List<XseedTheme> GetAll(string asFilter,int aiStandardWiseAssessmentId, string asOrder, int aiSchoolId)
        {
            using (SQLServerDbUtility OSQLServerDbUtility = new SQLServerDbUtility())
            {
                OSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                OSQLServerDbUtility.AddParameter("filter", asFilter, SqlDbType.NVarChar);
                OSQLServerDbUtility.AddParameter("Order", asOrder, SqlDbType.NVarChar);
                OSQLServerDbUtility.AddParameter("StandardWiseAssessmentId", aiStandardWiseAssessmentId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = OSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Xseed.usp_GetXseedThemeDetails"))
                return LoadThemeList(oSqlDataReader);
            }
        }
        #endregion
        #region " Private method"
         /// <summary>
        /// This method is used to set properties of XseedTheme.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private static List<XseedTheme> LoadThemeList(SqlDataReader oSqlDataReader)
        {
            List<XseedTheme> lstXseedTheme = new List<XseedTheme>();
            XseedTheme oXseedTheme = null;
            if (oSqlDataReader.HasRows)
            {
                while (oSqlDataReader.Read())
                {
                    oXseedTheme = new XseedTheme
                    {
                        ThemeId = Convert.ToInt32(oSqlDataReader["ThemeId"]),
                        Theme = oSqlDataReader["Theme"].ToString(),
                        StandardwiseAssessmentId = Convert.ToInt32(oSqlDataReader["StandardwiseAssessmentId"]),
                        AssessmentName = oSqlDataReader["Name"].ToString(),
                        SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"]),
                        StandardName = oSqlDataReader["Standard_Name"].ToString(),
                        Is_Deleted = oSqlDataReader["Is_Deleted"].ToString(),
                        StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"])
                    };
                    lstXseedTheme.Add(oXseedTheme);
                }
            }
            return lstXseedTheme;
        }
        /// <summary>
        /// This method is used to delete theme.
        /// </summary>
        /// <param name="aiThemeId"></param>
        public void Delete(int aiThemeId)
        {
            string sQuery = "UPDATE Xseed.ThemeMaster " +
                            "SET Is_Deleted=N'Y' " +
                            "WHERE ThemeId=" + aiThemeId +
                            " AND SchoolId=" + miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteTransaction(sQuery);
            }
        }

        public int GetCount()
        {
            string sSelectStatement = " SELECT COUNT(ThemeId) AS COUNT " +
                                      " FROM Xseed.ThemeMaster " +
                                      " WHERE Is_Deleted = N'"+ Constants.C_NO.ToString() +"' " +
                                      " AND SchoolId = " + miSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }
        #endregion
    }

 
}
