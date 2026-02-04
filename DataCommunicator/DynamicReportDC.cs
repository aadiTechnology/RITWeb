/* Class - DynamicReportDC.cs
 * Author - Yogesh Karne
 * Date - 10 Jun 2016.
 * Description - This business logic class used to handle business logics related to dynamic field export.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
namespace DataCommunicator
{
    public class DynamicReportDC
    {
        #region Member(s)

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private int miUserId = 0;

        #endregion

        #region Constructor

        public  DynamicReportDC(int miSchoolId, int miAcademicYearId, int miUserId)
        {
            this.miSchoolId = miSchoolId;
            this.miAcademicYearId = miAcademicYearId;
            this.miUserId = miUserId;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to save Dynamic Report Details details.
        /// </summary>
        /// <param name="asXml"></param>
        public void Save(string asXml, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DynamicReportDetailsXML", asXml, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertDynamicReportFieldDetails");
            };
        }

        /// <summary>
        /// This method is used to get dataset about dynamic student export.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public DataSet GetStudentDataForExport(int aiStandardId, int aiDivisionId, bool abIncludeWithLeft)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeWithLeft", abIncludeWithLeft, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetExportDynamicFields");
            }
        }
       
        /// <summary>
        /// This method is used to get Dynamic Report Field list.
        /// </summary>
        /// <returns></returns>
        public List<DynamicFieldDetails> GetDynamicReportFieldMasterDetails(bool abIsAdditional)
        {
            List<DynamicFieldDetails> lstDynamicReportFieldMasterDetails = new List<DynamicFieldDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsAdditionalField", abIsAdditional, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFieldDetails"))
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstDynamicReportFieldMasterDetails.Add(
                                new DynamicFieldDetails()
                                {
                                    DynamicReportFieldMasterId = Convert.ToInt32(oSqlDataReader["DynamicReportFieldMasterId"]),
                                    FieldText = Convert.ToString(oSqlDataReader["FieldText"]),
                                    IsSelected = Convert.ToBoolean(oSqlDataReader["IsSelected"]),
                                }
                                );
                        }
                    }
                    return lstDynamicReportFieldMasterDetails;
            }
        }
        #endregion
    }
}
