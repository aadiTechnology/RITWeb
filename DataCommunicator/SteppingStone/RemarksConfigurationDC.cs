// --------------------------------------------------------------------------
//	FileName	: RemarksConfigurationDC.cs
//	Modified by	: Pravin
//	Date		: 30 Mar 2012
//	Description	: This class is used to Adding,Removing Remarks and Template
// --------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Utility;
using SchoolEntities;
using ProgressReportEntities;

namespace DataCommunicator
{
    public class RemarksConfigurationDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;

        #endregion
        #region Constructor(s)

        /// <summary>
        /// constructor for school id and Academic year id
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public RemarksConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        }

        /// <summary>
        /// empty constructor.
        /// </summary>
        public RemarksConfigurationDC()
        {

        }

        #endregion

        public static List<RemarksConfig> GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            List<RemarksConfig> lstRemarksConfig = new List<RemarksConfig>();
            string sSqlStatement = "SELECT Id, Name, SortOrder FROM RemarksConfiguration" +
                                   " WHERE SchoolId = " + aiSchoolId +
                                   " AND AcademicYearId = " + aiAcademicYearId +
                                   " AND Is_Deleted = N'" + Constants.S_NO + "'" +
                                   " ORDER BY SortOrder";
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                GenericClass<RemarksConfig> oGenricClass = new GenericClass<RemarksConfig>();
                lstRemarksConfig = oGenricClass.GetFilledObjectList(oReader);
            }

            return lstRemarksConfig;
        }

        /// <summary>
        /// This function is used to get the remark template notes.
        /// </summary>
        /// <returns></returns>
        public static List<RemarkTemplateKeyword> GetTemplateNotes()
        {
            List<RemarkTemplateKeyword> olstRemarkTemplateNotes = new List<RemarkTemplateKeyword>();
            string sSqlSatement = " SELECT Id,Keyword,Male,Female,Description,Example" +
                                  " FROM RemarkTemplateKeywords" +
                                  " WHERE Is_Deleted=0";
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlSatement))
                {
                    GenericClass<RemarkTemplateKeyword> oGenericClass = new GenericClass<RemarkTemplateKeyword>();
                    olstRemarkTemplateNotes = oGenericClass.GetFilledObjectList(oSqlDataReader);
                }
                return olstRemarkTemplateNotes;
            }
        }

        public static void Save(RemarksConfig aoRemarksConfig)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aoRemarksConfig.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcadmicYearId", aoRemarksConfig.AcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("Remark", aoRemarksConfig.Name, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("SortOrder", aoRemarksConfig.SortOrder, SqlDbType.Int);
                oSqlDbUtility.AddParameter("InsertedById", aoRemarksConfig.InsertedById, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_InsertRemarksConfiguration");
            }
        }

        public static int Update(RemarksConfig aoRemarksConfig)
        {
            string sSqlStatement = "UPDATE RemarksConfiguration " +
                                   " SET Name = N'" + StringUtility.ReplaceSingleQuoteInString(aoRemarksConfig.Name, false) + "'" +
                                   ", SortOrder = " + aoRemarksConfig.SortOrder +
                                   ", UpdatedById = " + aoRemarksConfig.UpdatedById +
                                   ", UpdateDate = GETDATE()" +
                                   " WHERE SchoolId = " + aoRemarksConfig.SchoolId +
                                   " AND AcademicYearId = " + aoRemarksConfig.AcademicYearId +
                                   " AND Id = " + aoRemarksConfig.Id +
                                   " AND Is_Deleted = N'" + Constants.S_NO + "'";
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
                return oSqlDbUtility.PerformIntQueryOnSqlServer(sSqlStatement);
        }

        public static void Delete(RemarksConfig aoRemarksConfig)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aoRemarksConfig.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcadmicYearId", aoRemarksConfig.AcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("RemarksConfigurationId", aoRemarksConfig.Id, SqlDbType.Int);
                oSqlDbUtility.AddParameter("UpdatedById", aoRemarksConfig.UpdatedById, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteRemarksConfiguration");
            }
        }

        public static RemarksConfig GetRemarkDetails(int aiSchoolId, int aiAcademicYearId, int aiRemarksConfigId)
        {
            string sSqlStatement = "SELECT Name," +
                                   " SortOrder" +
                                   " FROM RemarksConfiguration" +
                                   " WHERE SchoolId = " + aiSchoolId +
                                   " AND AcademicYearId = " + aiAcademicYearId +
                                   " AND Id = " + aiRemarksConfigId +
                                   " AND Is_Deleted = N'" + Constants.S_NO + "'";
            RemarksConfig oRemarksConfig = new RemarksConfig();
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oSqlDataReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                while (oSqlDataReader.Read())
                {
                    oRemarksConfig.Name = Convert.ToString(oSqlDataReader["Name"]);
                    oRemarksConfig.SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"]);
                }
              
            }
            return oRemarksConfig;
        }

        /// <summary>
        /// This method is used to get Maximum remark length.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="bFlag"></param>
        /// <returns></returns>
        public int GetMaxRemarkLength(int aiStandardId, int aiTermId)
        {            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);             
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentsMaxRemarkLength");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get Maximum remark length.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiTermId"></param>
        /// <param name="bFlag"></param>
        /// <returns></returns>
        public int GetConfiguredMaxRemarkLength(int aiStandardId, int aiTermId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetConfiguredMaxRemarkLength");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This methode is used to get Standard wise Remark Length which returns list.
        /// </summary>
        /// <param name="aiConfigId"></param>
        /// <returns></returns>
        public List<StandardwiseRemarkLength> GetAllStandardwiseRemarkLengths()
        {
            List<StandardwiseRemarkLength> lstRemarkLength = new List<StandardwiseRemarkLength>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);              
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardwiseRemarkLength"))
                {
                    lstRemarkLength = ReadAllRemarkLength(oSqlDataReader);
                    return lstRemarkLength;
                }
            }
        }

        /// <summary>
        /// This Methode is used to get standard wise Remark length which return list of single record.
        /// </summary>
        /// <param name="aiConfigId"></param>
        /// <returns></returns>
        public StandardwiseRemarkLength GetRemarkConfiguration(int aiConfigId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aiConfigId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardwiseRemarkLength"))
                {
                    List<StandardwiseRemarkLength> lstRemarkLength = ReadAllRemarkLength(oSqlDataReader);
                    return lstRemarkLength[0];
                }

            }
        }

        /// <summary>
        /// This method is used to set Remarks Configuration values.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StandardwiseRemarkLength> ReadAllRemarkLength(SqlDataReader aoSqlDataReader)
        {
            List<StandardwiseRemarkLength> lstRemarkLength = new List<StandardwiseRemarkLength>();
            StandardwiseRemarkLength oStudentwiseRemarkConfigDetails;
            if (aoSqlDataReader != null && aoSqlDataReader.HasRows)
            {
                while (aoSqlDataReader.Read())
                {
                    oStudentwiseRemarkConfigDetails = new StandardwiseRemarkLength();
                    if ((aoSqlDataReader["StandardwiseRemarkLengthId"]) != DBNull.Value)
                        oStudentwiseRemarkConfigDetails.StandardwiseRemarkLengthId = Convert.ToInt32(aoSqlDataReader["StandardwiseRemarkLengthId"]);
                    if ((aoSqlDataReader["StandardId"]) != DBNull.Value)
                        oStudentwiseRemarkConfigDetails.StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]);
                    if ((aoSqlDataReader["StandardName"]) != DBNull.Value)
                        oStudentwiseRemarkConfigDetails.StandardName = Convert.ToString(aoSqlDataReader["StandardName"]);
                    if ((aoSqlDataReader["TermId"]) != DBNull.Value)
                        oStudentwiseRemarkConfigDetails.TermId = Convert.ToInt32(aoSqlDataReader["TermId"]);
                    if ((aoSqlDataReader["Term"]) != DBNull.Value)
                        oStudentwiseRemarkConfigDetails.Term = Convert.ToString(aoSqlDataReader["Term"]);
                    if ((aoSqlDataReader["MaxRemarkLength"]) != DBNull.Value)
                        oStudentwiseRemarkConfigDetails.MaxRemarkLength = Convert.ToInt32(aoSqlDataReader["MaxRemarkLength"]);

                    lstRemarkLength.Add(oStudentwiseRemarkConfigDetails);
                }
                
            }
            return lstRemarkLength;
        }

        /// <summary>
        /// This methode is used to Insert Standard and Term wise Remark Length configuration.
        /// </summary>
        /// <param name="aoStandardwiseRemarkLength"></param>
        /// <param name="aiConfigId"></param>
        public void InsertRemarkLengthDetails(StandardwiseRemarkLength aoStandardwiseRemarkLength)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aoStandardwiseRemarkLength.StandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aoStandardwiseRemarkLength.TermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MaxRemarkLength", aoStandardwiseRemarkLength.MaxRemarkLength, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", aoStandardwiseRemarkLength.StandardwiseRemarkLengthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertRemarkConfiguration");

            }
        }

        /// <summary>
        /// This methode is used to Delete Remark Length configuration.
        /// </summary>
        /// <param name="aiConfigId"></param>
        public void DeleteProgressRemarkLength(int aiConfigId)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcadmicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("StandardwiseRemarkLengthId", aiConfigId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteProgressRemarksLength");
            }
        }

        /// <summary>
        /// This method is used to return Maximum Lenngth of Remark of any student in class or Standard.
        /// </summary>
        /// <param name="aiStandardarId"></param>
        /// <param name="aiTermId"></param>
        /// <returns></returns>
        public int GetStudentsMaxRemarkLength(int aiStandardarId, int aiTermId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardarId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TermId", aiTermId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentsMaxRemarkLength");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        public int GetConfiguredMaxRemarkLength(int aiSubjectId, int aiTestId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetRemarkMaxLength");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }
    }

    public class RemarksCategoryDC
    {
        public static List<RemarksCategory> GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            List<RemarksCategory> lstRemarksCategory = new List<RemarksCategory>();
            string sSqlStatement = "SELECT Id, Name, SortOrder FROM RemarksCategory" +
                                   " WHERE SchoolId = " + aiSchoolId +
                                   " AND AcademicYearId = " + aiAcademicYearId +
                                   " AND IsDeleted =" + Constants.S_ZERO +
                                   "ORDER BY SortOrder";
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                GenericClass<RemarksCategory> oGenricClass = new GenericClass<RemarksCategory>();
                lstRemarksCategory = oGenricClass.GetFilledObjectList(oReader);
            }

            return lstRemarksCategory;
        }

        
        public static DataTable GetGrades(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCurrentYearGradeDetails");
            }
            
        }

        /// <summary>
        /// This function is used to get the remark template notes.
        /// </summary>
        /// <returns></returns>
        public static List<RemarkTemplateKeyword> GetTemplateNotes()
        {
            List<RemarkTemplateKeyword> olstRemarkTemplateNotes = new List<RemarkTemplateKeyword>();
            string sSqlSatement = " SELECT Id,Keyword,Male,Female,Description,Example" +
                                  " FROM RemarkTemplateKeywords" +
                                  " WHERE Is_Deleted=0";
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlSatement))
                {
                    GenericClass<RemarkTemplateKeyword> oGenericClass = new GenericClass<RemarkTemplateKeyword>();
                    olstRemarkTemplateNotes = oGenericClass.GetFilledObjectList(oSqlDataReader);
                }

                return olstRemarkTemplateNotes;
            }
        }
        public static void Save(RemarksConfig aoRemarksCategory, int aiRecordId)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aoRemarksCategory.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcadmicYearId", aoRemarksCategory.AcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("Name", aoRemarksCategory.Name, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("SortOrder", aoRemarksCategory.SortOrder, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("InsertedById", aoRemarksCategory.InsertedById, SqlDbType.Int);
                if (aiRecordId == 0)
                    oSqlDbUtility.AddParameter("RecordId", aoRemarksCategory.Id, SqlDbType.Int);
                else
                    oSqlDbUtility.AddParameter("RecordId", 0, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_InsertRemarksCategory");
            }
        }

        public static void Delete(RemarksConfig aoRemarksCategory)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aoRemarksCategory.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcadmicYearId", aoRemarksCategory.AcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("RemarksCategoryId", aoRemarksCategory.Id, SqlDbType.Int);
                oSqlDbUtility.AddParameter("UpdatedById", aoRemarksCategory.UpdatedById, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteRemarksCategory");
            }
        }

        public static RemarksConfig GetRemarkDetails(int aiSchoolId, int aiAcademicYearId, int aiRemarksCategoryId)
        {
            string sSqlStatement = "SELECT Name," +
                                   " SortOrder" +
                                   " FROM RemarksCategory" +
                                   " WHERE SchoolId = " + aiSchoolId +
                                   " AND AcademicYearId = " + aiAcademicYearId +
                                   " AND Id = " + aiRemarksCategoryId +
                                   " AND IsDeleted = 0";
            RemarksConfig oRemarksCategory = new RemarksConfig();
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oSqlDataReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                while (oSqlDataReader.Read())
                {
                    oRemarksCategory.Name = Convert.ToString(oSqlDataReader["Name"]);
                    oRemarksCategory.SortOrder = Convert.ToInt16(oSqlDataReader["SortOrder"]);
                }
            }
            return oRemarksCategory;
        }

    }

    public class RemarkTemplateDC
    {
        /// <summary>
        /// This method is used to Save Remark Template details
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public void Save(RemarkTemplateConfig oRemarkTemplateConfig)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", oRemarkTemplateConfig.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("RemarkId", oRemarkTemplateConfig.RemarkId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("Template", oRemarkTemplateConfig.Template, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("InsertedById", oRemarkTemplateConfig.InsertedById, SqlDbType.Int);
                oSqlDbUtility.AddParameter("TemplateId", oRemarkTemplateConfig.TemplateId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("OriginalConfigId", oRemarkTemplateConfig.OriginalConfigId, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_InsertRemarkTemplate");
            }
        }

        /// <summary>
        /// This method is used to Check Remark Template is duplicated or not
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public bool IsDuplicate(RemarkTemplateConfig oRemarkTemplateConfig)
        {
            SqlParameter oSqlParameter;
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", oRemarkTemplateConfig.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("RemarkId", oRemarkTemplateConfig.RemarkId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("Template", oRemarkTemplateConfig.Template, SqlDbType.VarChar);
                oSqlDbUtility.AddParameter("TemplateId", oRemarkTemplateConfig.TemplateId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("OriginalConfigId", oRemarkTemplateConfig.OriginalConfigId, SqlDbType.Int);
                oSqlParameter = oSqlDbUtility.AddParameter("Count", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_IsDuplicateRemarkTemplate");
            }
            return Convert.ToBoolean(oSqlParameter.Value);
        }

        /// <summary>
        /// This method is used to get Remark Template details
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public RemarkTemplateConfig Get(int aiSchoolId, int aiTemplateConfigId)
        {
            using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
            {
                string sSqlStatement = "SELECT Template," +
                                   "RemarkId," +
                                   "OriginalConfigId" +
                                   " FROM RemarkTemplateConfiguration" +
                                   " WHERE SchoolId = " + aiSchoolId +
                                   " AND TemplateId = " + aiTemplateConfigId +
                                   " AND Is_Deleted = 0";
                RemarkTemplateConfig oRemarkTemplateConfig = new RemarkTemplateConfig();
                using (SqlDataReader oSqlDataReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
                {
                    if (oSqlDataReader.Read())
                    {
                        oRemarkTemplateConfig.Template = Convert.ToString(oSqlDataReader["Template"]);
                        oRemarkTemplateConfig.RemarkId = Convert.ToInt32(oSqlDataReader["RemarkId"]);
                        oRemarkTemplateConfig.OriginalConfigId = Convert.ToInt32(oSqlDataReader["OriginalConfigId"]);
                    }
                }
                return oRemarkTemplateConfig;
            }
        }

        /// <summary>
        /// This method is used to get Remark Template details for selected remark id
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public List<RemarkTemplateConfig> GetAll(int aiSchoolId, int aiRemarkId, string asSortExpression, string asSortDirection, string asFilter, int aiAcademicYearId, int aiMarks_Grades_Configuration_DetailsId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RemarkId", aiRemarkId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MarksGradesConfigurationDetailsId", aiMarks_Grades_Configuration_DetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRemarkTemplateDetails"))
                {
                    GenericClass<RemarkTemplateConfig> oGeneric = new GenericClass<RemarkTemplateConfig>();
                    return oGeneric.GetFilledObjectList(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to Delete Remark Template details
        /// </summary>
        /// <param name="oRemarkTemplateConfig"></param>
        public void Delete(RemarkTemplateConfig oRemarkTemplateConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSqlStatement = "UPDATE RemarkTemplateConfiguration SET Is_Deleted=1," +
                                        "UpdatedById=N'" + oRemarkTemplateConfig.UpdatedById + "'," +
                                        "UpdateDate=N'" + DateTime.Now.ToString("yyyy/MM/dd") + "'" +
                                        "WHERE TemplateId=N'" + oRemarkTemplateConfig.TemplateId + "'";
                oSQLServerDbUtility.ExecuteTransaction(sSqlStatement);
            }
        }

        public List<RemarkTypeCategory> GetAllRemarkTypeCategories(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                List<RemarkTypeCategory> lstRemarkTypeCategoies = new List<RemarkTypeCategory>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRemarkTypeCategories"))
                {
                    while (oSqlDataReader.Read())
                        lstRemarkTypeCategoies.Add(new RemarkTypeCategory { CategoryId = Convert.ToInt32(oSqlDataReader["CategoryId"]), RemarkConfigId = Convert.ToInt32(oSqlDataReader["RemarkTypeId"]) });
                }

                return lstRemarkTypeCategoies;
            } 
        }
    }

}
