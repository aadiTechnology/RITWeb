using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Sql;
using Utility;
using PreCondionEntities;

namespace DataCommunicator
{

    public class ReferenceDC
    {
    
        #region Overloaded Constructor

        public ReferenceDC()
        {
        }

        #endregion

        ///// <summary>
        ///// This method is used to delete the teacher dependency details.
        ///// </summary>
        ///// <param name="aiUserId"></param>
        ///// <param name="aiSchoolId"></param>
        ///// <param name="aiAcademicYearId"></param>
        ///// <param name="aiTeacherId"></param>
        //public static void DeleteTeacherDependencyDetails(int aiUserId, int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.NVarChar);
        //        oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
        //        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteTeacherDependencyDetails");              
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiParentId"></param>
        /// <param name="aiParentIdValue"></param>
        /// <param name="aiName"></param>
        /// <returns></returns>
        public static string CheckDependenciesAndGetErrorMessages(int aiParentId, int aiParentIdValue, string aiName, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_intReference_Id", aiParentId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intRecord_Id", aiParentIdValue,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_strRecord_Name", aiName,SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYearId,SqlDbType.Int);
                DataSet oDs = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetReferencesAcademicYear");
                if (oDs.Tables[oDs.Tables.Count - 1].Rows.Count > 0)
                    return oDs.Tables[oDs.Tables.Count - 1].Rows[0]["Reference"].ToString();
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiParentId"></param>
        /// <param name="aiParentIdValue"></param>
        /// <param name="aiName"></param>
        /// <returns></returns>
        public static DataTable CheckDependenciesAndGetErrorMessages(Constants.ReferenceId aParentId, string asXml, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_intReference_Id", aParentId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intRecord_Id", asXml,SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYearId,SqlDbType.Int);
                DataSet oDs = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetReferences");
                return oDs.Tables[oDs.Tables.Count - 1];
            }
        }

        public static DataSet GetPreConditionMsg(int aiSchoolId, int aiAcademicYearId, int aiConfigId)
        {
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                 oSQLServerDbUtility.AddParameter("prm_intConfig_Id", aiConfigId,SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYearId,SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("prm_intSchoolId", aiSchoolId,SqlDbType.Int);
                 return  oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_CheckPreCondition");
             }
        }
        
        /// <summary>
        /// This method is added to check that is user have doe with requred configuration for standard 
        /// for which student is being added.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandard"></param>
        /// <returns></returns>
        public static DataSet GetStudentUIPreConditionMsg(int aiSchoolId, int aiAcademicYearId, int aiStandard)
        {
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                 oSQLServerDbUtility.AddParameter("prm_intStandard_Id", aiStandard,SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYearId,SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("prm_intSchoolId", aiSchoolId,SqlDbType.Int);
                 return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_CheckStudentUIPreCondition");
             }
        }

		public static List<PreCondition> GetPreConditionMsgForStudentWiseProgressReport(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            List<PreCondition> olstPreCondition = new List<PreCondition>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_CheckPreConditionStudentWiseProgressReport"))
                {
                    while (oSqlDataReader.Read())
                    {
                        PreCondition oPreCondition = new PreCondition()
                        {
                            ConfigureName = Convert.ToString(oSqlDataReader["Configure_Name"]),
                            NavigateUrl = Convert.ToString(oSqlDataReader["NavigateURL"]),
                        };
                        olstPreCondition.Add(oPreCondition);
                    }
                }
            }
            return olstPreCondition;
        }
        
        /// <summary>
        /// This is DAL function to generalise Prcondition checking Methode.
        /// </summary>
        /// <param name="aiParentId"></param>
        /// <param name="aiParentIdValue"></param>
        /// <param name="aiName"></param>
        /// <returns>Dataset</returns>
        
        public static DataSet CheckPrecondition(int aiSchoolId, int aiAcademicYearId, int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_intConfig_Id", aiConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intSchoolId", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_CheckPreCondition");
            }
        }
        
        public static bool IsPrePrimaryStdConfigured(int aiShcoolId) {
			string sSqlStatement = String.Format("SELECT Standard_Id FROM Standard_Master WHERE School_Id = {0} AND Is_PrePrimary = 'Y'"
												 ,aiShcoolId);
												 
			DataTable oDT;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility()) {
				oDT =  oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSqlStatement);
				return !(oDT == null || oDT.Rows.Count == 0);
			}
        }
    }
}
