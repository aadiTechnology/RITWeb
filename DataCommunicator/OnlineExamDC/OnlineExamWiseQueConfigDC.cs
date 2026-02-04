using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using DataCommunicator;
using Utility;
using SchoolEntities.Admin;
namespace DataCommunicator
{
    public class OnlineExamWiseQueConfigDC
    {
        #region " Data Members "

        public int miSchoolId;
        public int miAcademicYearId;
        List<AnswerDetails> mlstAnswerDetails = new List<AnswerDetails>();
        OnlineExamConfiguration mOnlineExamConfiguration;

        #endregion
        #region " Constructor "

        public OnlineExamWiseQueConfigDC() { }

        public OnlineExamWiseQueConfigDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public List<AnswerDetails> AnswerDetails
        {
            get { return mlstAnswerDetails; }
        }

        public OnlineExamConfiguration OnlineExamConfiguration
        {
            get { return mOnlineExamConfiguration; }
        }

        public DataTable GetAllTestsForClass(int aiStandardDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetClasswiseTestForCombobox");
            }
        }


        public static DataTable GetAllQuestions(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllExamQuestions");
            }
        }

        public DataTable GetAssociatedStandards(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT " +
                                    "School_Id" +
                                    ",Standard_Id " +
                                    ",className as class_name " +
                                    ",SchoolWise_Standard_Division_Id" +
                                    " FROM " +
                                    "vw_standard_division " +
                                    " WHERE " +
                                    "School_Id =" + miSchoolId +
                                    "AND academic_year_id =" + miAcademicYearId +
                                    "ORDER BY " +
                                    "Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
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

        //public void GetAllClassesCopy(int aiSchoolId, int aiAcademicYearId, int aiclass, int aisubject, string sids)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Class", aiclass, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Subject", aisubject, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("StandardDivisionId", sids, SqlDbType.Int);
        //        oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllExamClassSubject");
        //    }
        //}

        public struct VehicleDetailsStruct
        {

            public int Id;

            public int ExamId;
            public int QuestionId;

            public int SubjectId;
            public int StandardDivisionId;
            public DateTime StartDateAndTime;
            public DateTime EndDateAndTime;
            public bool ShuffleForCount;
            public int NoOfQuestions;
            public bool ShuffleForSequence;
            public int miSchoolId;

            public int miAcademicYearId;

            public bool mblnIsDeleted;

            public System.DateTime mdtInsertDate;

            public int miInsertedById;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;
            public int ExamConfigurationId;

        }



        public static DataTable GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId)
        {
            DataTable dtt = new DataTable();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllExamQuestionDetails");
            }

        }

        public List<OnlineExamWiseQueConfig> GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId, String sSortExpression, int iEndIndex, int startRowIndex)
        {
            if (sSortExpression == string.Empty)
                sSortExpression = "Class";
            List<OnlineExamWiseQueConfig> lst = new List<OnlineExamWiseQueConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExpression == "" ? "" : " ORDER BY " + sSortExpression, SqlDbType.NVarChar);

                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllExamQuestionDetails"))
                {
                    if (oReader.HasRows)
                    {
                        while (oReader.Read())
                            lst.Add(ReadObjectFromReader(oReader));
                    }
                }

            }
            return lst;

        }
        private OnlineExamWiseQueConfig ReadObjectFromReader(SqlDataReader aoReader)
        {
            return new OnlineExamWiseQueConfig()
            {
                Class = aoReader["ClassName"].ToString(),
                Exam = aoReader["SchoolWise_Test_Name"].ToString(),
                Subject = aoReader["Subject_Name"].ToString(),
                Question = aoReader["Question"].ToString(),
                Id = aoReader["Id"].ToInt()
            };
        }

        public void Save(string asStaffXML, OnlineExamWiseQueConfig oExamConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", oExamConfig.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", oExamConfig.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", oExamConfig.ExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", oExamConfig.SubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", oExamConfig.StandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamXML", asStaffXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", oExamConfig.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDateAndTime", oExamConfig.StartDateAndTime, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("EndDateAndTime", oExamConfig.EndDateAndTime, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("NoOfQuestions", oExamConfig.NoOfQuestions, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShuffleForCount", oExamConfig.ShuffleForCount, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ShuffleForSequence", oExamConfig.ShuffleForSequence, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StartTime", oExamConfig.StartTime, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EndTime", oExamConfig.EndTime, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Id", oExamConfig.Id, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveOnlineExamWiseQueConfig");

            }
        }

        public void SaveExamQuestion(string asQuestionXML, OnlineExamQuestConfig oExamConfig)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", oExamConfig.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", oExamConfig.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", oExamConfig.StandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", oExamConfig.SubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Question", oExamConfig.Question, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("OutOfMarks", oExamConfig.OutOfMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", oExamConfig.QuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AnswerTypeId", oExamConfig.AnswerTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AnswerDetailsXML", asQuestionXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", oExamConfig.InsertedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveOnlineExamQuestionConfig");

            }
        }

        public List<OnlineExamQuestConfig> GetAllQuestionConfiguration(int aiSchoolId, int aiAcademicYearId, String sSortExpression, int iEndIndex, int startRowIndex, int aiStandardDivisionId, int aiSubjectId)
        {
           
            //if (sSortExpression == string.Empty || sSortExpression=="Question" || sSortExpression=="Question ASC")
            //    sSortExpression = "Question";
            if (sSortExpression != "")
                sSortExpression = "ORDER BY" + sSortExpression;
            else
                sSortExpression = "ORDER BY Question desc";

            List<OnlineExamQuestConfig> lstOnlineExamQuestConfig = new List<OnlineExamQuestConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sSortExpression ,SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardDivisionId",aiStandardDivisionId , SqlDbType.Int);
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

        public List<OnlineExamQuestConfig> GetAll(int aiSchoolId)
        {
            List<OnlineExamQuestConfig> lstOnlineExamQuestConfig = new List<OnlineExamQuestConfig>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
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

        private OnlineExamQuestConfig ReadObjectFromRead(SqlDataReader aoReader)
        {
            return new OnlineExamQuestConfig()
            {
                Class = aoReader["className"].ToString(),
                Subject = aoReader["Subject_Name"].ToString(),
                Question = aoReader["Question"].ToString(),
                OutOfMarks = aoReader["OutOfMarks"].ToInt(),
                Id = aoReader["Id"].ToInt()
            };
        }

       
        public List<OnlineExamQuestConfig> Get(int aiId, int aiSchoolId)
        {
            List<OnlineExamQuestConfig> lstRemarkLength = new List<OnlineExamQuestConfig>();
            OnlineExamQuestConfig oOnlineExamQuestConfig;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                using (SqlDataReader aoSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOnlineQuestionDetails"))
                {
                    while (aoSqlDataReader.Read())
                    {
                        oOnlineExamQuestConfig = new OnlineExamQuestConfig();
                        //   oOnlineExamQuestConfig.Id = Convert.ToInt32(aoSqlDataReader["Id"]);
                        oOnlineExamQuestConfig.StandardDivisionId = Convert.ToInt32(aoSqlDataReader["StandardDivisionId"]);
                        oOnlineExamQuestConfig.SubjectId = Convert.ToInt32(aoSqlDataReader["SubjectId"]);
                        oOnlineExamQuestConfig.Question = Convert.ToString(aoSqlDataReader["Question"]);
                        oOnlineExamQuestConfig.OutOfMarks = Convert.ToInt32(aoSqlDataReader["OutOfMarks"]);
                        oOnlineExamQuestConfig.Answer = Convert.ToString(aoSqlDataReader["Answer"]);
                        oOnlineExamQuestConfig.AnswerTypeId = Convert.ToInt32(aoSqlDataReader["AnswerTypeId"]);
                        oOnlineExamQuestConfig.IsCorrectAnswer = Convert.ToBoolean(aoSqlDataReader["IsCorrectAnswer"]);
                        oOnlineExamQuestConfig.DisplayOrder = Convert.ToInt32(aoSqlDataReader["DisplayOrder"]);
                        oOnlineExamQuestConfig.QuestionId = Convert.ToInt32(aoSqlDataReader["QuestionId"]);
                        lstRemarkLength.Add(oOnlineExamQuestConfig);
                    }
                    return lstRemarkLength;
                }
            }
        }


        //public static int CountTotalExamQuestionConfiguration(Int32 aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

        //        oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountAllExamQuestionDetails");
        //        return Convert.ToInt32(oSqlParameter.Value);
        //    }
        //}
        //public static int CountTotalQuestionConfiguration(Int32 aiSchoolId, int aiAcademicYearId, String sortExpression, int maximumRows, int startRowIndex, int aiStandardDivisionId, int aiSubjectId)
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

        public static DataSet GetDetailsForUpdateQuestions(int aiVehicleId, int aiSchoolID, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiVehicleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetDetailsForEditQuestions");

            }
        }


        public static DataTable GetAllExamQuestionConfigurationl(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiExamId, int aiSubjectId)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllExamQuestionDetails");

            }

        }
        public List<OnlineExamWiseQueConfig> GetAllExamQuestionConfiguration(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiExamId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllExamQuestionDetails"))
                    return FillVendorDetails(oSqlDataReader);
            }
        }
        private List<OnlineExamWiseQueConfig> FillVendorDetails(SqlDataReader oSqlDataReader)
        {
            List<OnlineExamWiseQueConfig> lstVendorDetails = new List<OnlineExamWiseQueConfig>();
            while (oSqlDataReader.Read())
            {
                OnlineExamWiseQueConfig oVendorDetails = new OnlineExamWiseQueConfig();
                oVendorDetails.Class = Convert.ToString(oSqlDataReader["ClassName"]);
                oVendorDetails.Exam = Convert.ToString(oSqlDataReader["SchoolWise_Test_Name"]);
                oVendorDetails.Question = Convert.ToString(oSqlDataReader["Question"]);

                lstVendorDetails.Add(oVendorDetails);
            }
            return lstVendorDetails;
        }


        public DataTable DeleteExamDetails(int aiVehicleId, int aiSchoolID, int aiAcademicYearId, out int aiRowCount)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiVehicleId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("RowCount", 0, SqlDbType.Int, ParameterDirection.Output);
                DataTable oDTMsg = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteOnlineQuestionDetails", true);
                aiRowCount = Convert.ToInt32(oSqlParameter.Value);
                return oDTMsg;
            }
        }

        /// <summary>
        /// Thie method is used to get all questions for Online Exam.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<QuestionDetails> GetQuestionsForOnlineExam(int aiStandardId, int aiStandardDivisionId, int aiSubjectId, int aiExamId, int aiStudentId)
        {
            List<QuestionDetails> lstQuestionDetails = new List<QuestionDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseTestId", aiExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExamwiseQuestionDetailsForOnlineExam"))
                {
                    while (oSqlDataReader.Read())
                    {
                        QuestionDetails oQuestionDetails = new QuestionDetails();

                        oQuestionDetails.QuestionId = oSqlDataReader["QuestionId"].ToInt();
                        oQuestionDetails.Question = oSqlDataReader["Question"].ToString();
                        oQuestionDetails.SerialNo = oSqlDataReader["SerialNo"].ToInt();
                        oQuestionDetails.Marks = oSqlDataReader["OutOfMarks"].ToInt();
                        oQuestionDetails.IsExamSaved = oSqlDataReader["IsOnlineExamSaved"].ToBool();
                        oQuestionDetails.IsExamSubmited = oSqlDataReader["IsOnlineExamSubmited"].ToBool();
                        oQuestionDetails.AttachmentPath = oSqlDataReader["FileUploadPath"].ToString();
                        oQuestionDetails.AnswerTypeId = oSqlDataReader["AnswerTypeId"].ToInt();
                        lstQuestionDetails.Add(oQuestionDetails);
                    }

                    mlstAnswerDetails = new List<SchoolEntities.AnswerDetails>();
                    oSqlDataReader.NextResult();
                    while (oSqlDataReader.Read())
                    {
                        AnswerDetails oAnswerDetails = new AnswerDetails();

                        oAnswerDetails.AnswerId = oSqlDataReader["AnswerId"].ToInt();
                        oAnswerDetails.Answer = oSqlDataReader["Answer"].ToString();
                        oAnswerDetails.QuestionID = oSqlDataReader["QuestionId"].ToInt();
                        oAnswerDetails.DisplayOrder = oSqlDataReader["DisplayOrder"].ToInt();
                        oAnswerDetails.IsCorrectAnswer = oSqlDataReader["IsCorrectAnswer"].ToBool();
                        oAnswerDetails.UserSelectedAnswer = oSqlDataReader["UserSelectedAnswer"].ToInt();
                        oAnswerDetails.QuestionID = oSqlDataReader["QuestionID"].ToInt();
                        oAnswerDetails.AttachmentPath = oSqlDataReader["AnswerFilePath"].ToString();
                        oAnswerDetails.DescriptionFileName = oSqlDataReader["DescriptionFileName"].ToString();

                        mlstAnswerDetails.Add(oAnswerDetails);
                    }

                    oSqlDataReader.NextResult();
                    mOnlineExamConfiguration = new OnlineExamConfiguration();
                    if (oSqlDataReader.Read())
                    {
                        mOnlineExamConfiguration.Subject = oSqlDataReader["Subject_Name"].ToString();
                        mOnlineExamConfiguration.Exam = oSqlDataReader["TestName"].ToString();
                        mOnlineExamConfiguration.StartDateAndTime = oSqlDataReader["StartDateAndTime"].ToDateTime();
                        mOnlineExamConfiguration.EndDateAndTime = oSqlDataReader["EndDateAndTime"].ToDateTime();
                    }
                }
                return lstQuestionDetails;
            }
        }       

        /// <summary>
        /// This method is used to get questionwise answer details for online exam.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        //public List<AnswerDetails> GetQuestionWiseAnswersForOnlineExam(int aiStandardDivisionId, int aiSubjectId, int aiExamId, int aiQuestionId, int aiStudentId)
        //{
        //    List<AnswerDetails> lstAnswerDetails = new List<AnswerDetails>();
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("SchoolwiseTestId", aiExamId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

        //        using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetQuestionWiseAnswerDetails"))
        //        {
        //            while (oSqlDataReader.Read())
        //            {
        //                AnswerDetails oAnswerDetails = new AnswerDetails();

        //                oAnswerDetails.AnswerId = oSqlDataReader["AnswerId"].ToInt();
        //                oAnswerDetails.Answer = oSqlDataReader["Answer"].ToString();
        //                oAnswerDetails.QuestionID = oSqlDataReader["QuestionId"].ToInt();
        //                oAnswerDetails.DisplayOrder = oSqlDataReader["DisplayOrder"].ToInt();
        //                oAnswerDetails.IsCorrectAnswer = oSqlDataReader["IsCorrectAnswer"].ToBool();
        //                oAnswerDetails.UserSelectedAnswer = oSqlDataReader["UserSelectedAnswer"].ToInt();

        //                lstAnswerDetails.Add(oAnswerDetails);
        //            }
        //        }
        //        return lstAnswerDetails;
        //    }
        //}

        /// <summary>
        /// This method is used to save student online exam details.
        /// </summary>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asQuestAnswerDetails"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="aiTotalMarks"></param>
        /// <param name="aiOutOfMarks"></param>
        public void SaveStudentQuestionAnswerDetails(int iStandardId, int aiStandardDivisionId, int aiSubjectId, int aiExamId, int aiStudentId, string asQuestAnswerDetails, int aiInsertedById, int aiTotalMarks, int aiOutOfMarks)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestAnswerDetails", asQuestAnswerDetails, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TotalMarks", aiTotalMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OutOfMarks", aiOutOfMarks, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);                

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentQuestionAnswerDetails");
            }
        }

        /// <summary>
        /// This method is used to submit the online exam.
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiExamId"></param>
        /// <param name="aiStudentId"></param>
        public void SubmitStudentOnlineExam(int aiStdId, int aiStdDivId, int aiSubjectId, int aiExamId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentOnlineExam");
            }
        }

        public static DataTable GetAllSubjectsForExam(int aiSchoolId, int aiAcademicYearId, int aiExamId, int aiStudentIUd)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExamId", aiExamId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentIUd, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllSubjectsForExam");
            }
        }

        public static DataTable GetAllSubjectDetailsOfExam(int aiSchoolId, int aiAcademicYearId, int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllSubjectDetailsOfExam");
            }
        }

        public DataTable GetAllStudentList(int aiStdDivId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("StandardDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStudentList");
            }

        }

        public DataTable GetAllStudentQuestionList(int aistudentid, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aistudentid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStudentQuestionList");
            }
        }

        public void SaveStudentsQuestionMarks(string asStudentMarkDetails, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentMarkDetails", asStudentMarkDetails, SqlDbType.NVarChar);                
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentsQuestionMarks");
            }
        }      

        #endregion
    }
}
