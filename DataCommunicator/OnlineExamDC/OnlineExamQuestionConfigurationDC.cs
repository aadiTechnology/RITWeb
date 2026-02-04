using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using SchoolEntities.Admin;
using Utility;
namespace DataCommunicator
{
    public class OnlineExamQuestionConfigurationDC
    {
        #region " Data Members "

        public int miSchoolId;
        public int miAcademicYearId;
        private int miUpdatedById;

        #endregion

        #region " Constructor "

        public OnlineExamQuestionConfigurationDC() { }

        public OnlineExamQuestionConfigurationDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUpdatedById = aiUpdatedById;
        }

         #endregion

        #region " Method(s) "

        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetYearwiseSubjectList"))
                    return this.ReadAllSubjects(oSqlDataReader);
            }
        }
        public List<YearWiseSubjectsDetails> ReadAllSubjects(SqlDataReader aoSqlDataReader)
        {
            List<YearWiseSubjectsDetails> lstSubjectsDetails = new List<YearWiseSubjectsDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    YearWiseSubjectsDetails oYearWiseSubjectsDetails = new YearWiseSubjectsDetails();
                    if (aoSqlDataReader["Subject_Id"] != DBNull.Value)
                        oYearWiseSubjectsDetails.SubjectId = Convert.ToInt32(aoSqlDataReader["Subject_Id"]);
                    if (aoSqlDataReader["Subject_Name"] != DBNull.Value)
                        oYearWiseSubjectsDetails.SubjectName = aoSqlDataReader["Subject_Name"].ToString();

                    lstSubjectsDetails.Add(oYearWiseSubjectsDetails);
                }
                aoSqlDataReader.Close();
            }
            return lstSubjectsDetails;
        }
        //public DataTable CopySubjectConfiguration(int aitargetstddivid, int aisubject, string sids)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SourceStandardDivisionId", aitargetstddivid, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SubjectId", aisubject, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("TargetStandardDivisionIds", sids, SqlDbType.NVarChar);
        //        return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CopystandarddivisiontoSubject");
        //    }
        //}

        /// <summary>
        /// This method is used to delete the parameter from the given list view.
        /// </summary>
        /// <param name="aiParameterId"></param>
        public void Delete(int aiQuestionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteOnlineExamQuestion");
            }
        }


        public DataTable GetAllStandards()
        {
            string sSelectStatement = " SELECT " +
                                    "School_Id" +
                                    ",Standard_Id " +
                                    ",Standard_Name " +

                                    " FROM " +
                                    "Standard_Master " +
                                    " WHERE " +
                                    "School_Id =" + this.miSchoolId +
                                    "AND academic_year_id =" + this.miAcademicYearId +
                                    "ORDER BY " +
                                    "Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        public DataTable GetAssociatedStandards(int aiStandardId)
        {
            string sSelectStatement = " SELECT " +
                                    "School_Id" +
                                    ",Standard_Id " +
                                    ",className as class_name " +
                                    ",SchoolWise_Standard_Division_Id" +
                                     ",Division_Name" +
                                    " FROM " +
                                    "vw_standard_division " +
                                    " WHERE " +
                                    "School_Id =" + this.miSchoolId +
                                    "AND academic_year_id =" + this.miAcademicYearId +
                                     "AND Standard_Id =" + aiStandardId +
                                    "ORDER BY " +
                                    "Original_Division_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public void Save(string asQuestionXML, OnlineExamQuestionConfig oExamConfig, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", oExamConfig.StandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", oExamConfig.SubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Question", oExamConfig.Question, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("OutOfMarks", oExamConfig.OutOfMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionFilePath", oExamConfig.QuestionFilePath, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("QuestionId", oExamConfig.QuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AnswerTypeId", oExamConfig.AnswerTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AnswerDetailsXML", asQuestionXML, SqlDbType.Xml);                
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
              //  oSQLServerDbUtility.AddParameter("StandardDivisionId", aidivision, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveOnlineExamQuestionConfig");

            }
        }
        public List<OnlineExamQuestionConfig> GetAll(int aiSchoolId, int aiAcademicYearId, String sSortExpression,string sortDirection, int iEndIndex, int startRowIndex,int aiStandardId, int aiStandardDivisionId, int aiSubjectId)
        {
            if (string.IsNullOrEmpty(sSortExpression))
                sSortExpression = "Question";

            sSortExpression = sSortExpression.ToLower().Replace(Constants.S_ASCENDING, string.Empty).Replace(Constants.S_DESCENDING, string.Empty);
            sSortExpression = "Order By "+ sSortExpression + " " + sortDirection;

            List<OnlineExamQuestionConfig> lstOnlineExamQuestConfig = new List<OnlineExamQuestionConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllOnlineExamQuestions"))
                {
                    if (oReader.HasRows)
                    {
                        while (oReader.Read())
                            lstOnlineExamQuestConfig.Add(ReadObjectFromRead(oReader));
                    }
                }

            }
            return lstOnlineExamQuestConfig;
        }

        public void DeleteQuestionAnswerImage(int aiQuestionId, int aiAnswerId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AnswerId", aiAnswerId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteQuestionAnswerImage");
            }
        }

        private OnlineExamQuestionConfig ReadObjectFromRead(SqlDataReader aoReader)
        {
            return new OnlineExamQuestionConfig()
            {             
                Question = aoReader["Question"].ToString(),
                OutOfMarks = aoReader["OutOfMarks"].ToInt(),
                Id = aoReader["Id"].ToInt(),
                CorrectAnswer = aoReader["CorrectAnswer"].ToString(),
                IsSubmitted = aoReader["IsSubmitted"].ToBool(),
                TotalRows = aoReader["TotalRows"].ToInt(),
                AnswerTypeId = aoReader["AnswerTypeId"].ToInt(),
                AnswerFilePath = aoReader["AnswerFilePath"].ToString()
            };
        }
        //public static int Count(Int32 aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, int aiStandardDivisionId, int aiSubjectId)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
        //        SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

        //        oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountAllQuestionDetails");
        //        return Convert.ToInt32(oSqlParameter.Value);
        //    }
        //}

        public List<OnlineExamQuestionConfig> Get(int aiVehicleId)
        {
            List<OnlineExamQuestionConfig> lstRemarkLength = new List<OnlineExamQuestionConfig>();
            OnlineExamQuestionConfig oOnlineExamQuestConfig;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);                
                using (SqlDataReader aoSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_FillQuestionDetailstoControl"))
                {
                    while (aoSqlDataReader.Read())
                    {
                        oOnlineExamQuestConfig = new OnlineExamQuestionConfig();
                        oOnlineExamQuestConfig.StandardDivisionId = Convert.ToInt32(aoSqlDataReader["StandardDivisionId"]);
                        oOnlineExamQuestConfig.SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]);
                        oOnlineExamQuestConfig.Question = Convert.ToString(aoSqlDataReader["Question"]);
                        oOnlineExamQuestConfig.QuestionFilePath = Convert.ToString(aoSqlDataReader["FileUploadPath"]);
                        oOnlineExamQuestConfig.OutOfMarks = Convert.ToInt32(aoSqlDataReader["OutOfMarks"]);
                        oOnlineExamQuestConfig.Answer = Convert.ToString(aoSqlDataReader["Answer"]);
                        oOnlineExamQuestConfig.AnswerFilePath = Convert.ToString(aoSqlDataReader["AnswerFilePath"]);
                        oOnlineExamQuestConfig.AnswerTypeId = Convert.ToInt32(aoSqlDataReader["AnswerTypeId"]);
                        oOnlineExamQuestConfig.IsCorrectAnswer = Convert.ToBoolean(aoSqlDataReader["IsCorrectAnswer"]);
                        oOnlineExamQuestConfig.DisplayOrder = Convert.ToInt32(aoSqlDataReader["DisplayOrder"]);
                        oOnlineExamQuestConfig.QuestionId = Convert.ToInt32(aoSqlDataReader["QuestionId"]);
                        oOnlineExamQuestConfig.Id = Convert.ToInt32(aoSqlDataReader["AnswerId"]);
                        lstRemarkLength.Add(oOnlineExamQuestConfig);
                    }
                    return lstRemarkLength;
                }
            }
        }
        #endregion

        public ButtonStateDetails GetButtonState(int aiStdId, int aiStdDivId, int aiSubjectId)
        {
            ButtonStateDetails oButtonStateDetails = new ButtonStateDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOnlineExamQuestionSubmitStatus"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oButtonStateDetails.EnableSubmitButtton = oSqlDataReader["EnableSubmitButtton"].ToBool();
                        oButtonStateDetails.EnableUnSubmitButtton = oSqlDataReader["EnableUnSubmitButtton"].ToBool();
                    }
                }
            }
            return oButtonStateDetails;
        }

        public void Submit(int aiStdId, int aiStdDivId, int aiSubjectId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitOnlineExamQuestions");                
            }
        }
    }
}