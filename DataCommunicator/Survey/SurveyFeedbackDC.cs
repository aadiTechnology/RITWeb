using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities.Survey;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
    public class SurveyFeedbackDC
    {
        #region Data MEmber(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        private List<FeedbackGrade> mlstGrades = new List<FeedbackGrade>();
        private List<FeedbackCategory> mlstCategories = new List<FeedbackCategory>();
        private List<FeedbackParameter> mlstParameters = new List<FeedbackParameter>();
        private SchoolEntity moSchoolEntity = new SchoolEntity();
        private List<SurveyFeedbackDetails> mlstFeedbacks = new List<SurveyFeedbackDetails>();
        private bool mbIsFeedbackSubmitted;

        #endregion

        #region Constructor(s)

        public SurveyFeedbackDC()
        {
        }

        public SurveyFeedbackDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region Property(s)

        public List<FeedbackGrade> FeedbackGrades
        {
            get { return this.mlstGrades; }
        }

        public List<FeedbackCategory> FeedbackCategories
        {
            get { return this.mlstCategories; }
        }

        public List<FeedbackParameter> FeedbackParameters
        {
            get { return this.mlstParameters; }
        }

        public SchoolEntity SchoolInfo
        {
            get { return this.moSchoolEntity; }
        }

        public List<SurveyFeedbackDetails> SurveyFeedbacks
        {
            get { return this.mlstFeedbacks; }
        }

        public bool IsFeedbackSubmitted
        {
            get { return this.mbIsFeedbackSubmitted; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return feedback details.
        /// </summary>
        /// <param name="aiSurveyId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<SurveyFeedbackDetails> GetFeedbackDetails(int aiSurveyId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSurveyDetails"))
                {
                    this.FillSchoolInfo(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillFeedbackGrades(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillFeedbackParameters(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    this.FillFeedbackCategories(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    List<SurveyFeedbackDetails> lstFeedbacks = this.FillFeedbackGradesAssignment(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    if (oSqlDataReader.Read())
                        this.mbIsFeedbackSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]);

                    return lstFeedbacks;
                }
            }
        }

        /// <summary>
        /// This method is used to save feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, int aiSurveyId, string asXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeedbackXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSurveyFeedbackDetails");
            }
        }


        /// <summary>
        /// This method is used to submit feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="abIsSubmitted"></param>
        public void Submit(int aiUserId, int aiSurveyId, bool abIsSubmitted)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitted", abIsSubmitted, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitSurveyFeedbackDetails");
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to load feedback grade assignments.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<SurveyFeedbackDetails> FillFeedbackGradesAssignment(SqlDataReader aoSqlDataReader)
        {
            List<SurveyFeedbackDetails> lstFeedbacks = new List<SurveyFeedbackDetails>();
            while (aoSqlDataReader.Read())
            {
                lstFeedbacks.Add
                    (
                        new SurveyFeedbackDetails
                        {
                            FeedbackParameterId = Convert.ToInt32(aoSqlDataReader["FeedbackParameterId"]),
                            FeedbackGradeId = Convert.ToInt32(aoSqlDataReader["FeedbackGradeId"]),
                            Observation = Convert.ToString(aoSqlDataReader["Observation"]),
                            ParameterSubject = Convert.ToString(aoSqlDataReader["ParameterSubject"])
                        }
                    );
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// This method is used to load feedback categories.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillFeedbackCategories(SqlDataReader aoSqlDataReader)
        {
            FeedbackCategory oFeedbackCategory = null;
            while (aoSqlDataReader.Read())
            {
                oFeedbackCategory = new FeedbackCategory
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    OriginalCategoryId = Convert.ToInt32(aoSqlDataReader["OriginalCategoryId"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    InputTypeId = Convert.ToInt32(aoSqlDataReader["InputTypeId"]),
                    IsEditableToAll = Convert.ToBoolean(aoSqlDataReader["IsEditableToAll"]),
                    SurveyId = Convert.ToInt32(aoSqlDataReader["SurveyId"]),
                    ShowNameOnReport = Convert.ToBoolean(aoSqlDataReader["ShowNameOnReport"])
                };

                this.mlstCategories.Add(oFeedbackCategory);
            }
        }

        /// <summary>
        /// This method is used to load feedback parameters.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillFeedbackParameters(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                this.mlstParameters.Add(new FeedbackParameter
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Title = Convert.ToString(aoSqlDataReader["Title"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                    FeedbackCategoryId = Convert.ToInt32(aoSqlDataReader["FeedbackCategoryId"]),
                    SurveyId = Convert.ToInt32(aoSqlDataReader["SurveyId"]),
                    IsAnswerRequired = Convert.ToBoolean(aoSqlDataReader["IsAnswerRequired"]),
                    AllowParameterUpdation = Convert.ToBoolean(aoSqlDataReader["AllowParameterUpdation"]),
                    IsMandatory = Convert.ToBoolean(aoSqlDataReader["IsMandatory"])
                });
            }
        }

        /// <summary>
        /// This method is used to load feedback grades.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillFeedbackGrades(SqlDataReader aoSqlDataReader)
        {
            FeedbackGrade oFeedbackGrade = null;
            while (aoSqlDataReader.Read())
            {
                oFeedbackGrade = new FeedbackGrade
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    Description = Convert.ToString(aoSqlDataReader["Description"]),
                    OriginalGradeId = Convert.ToInt32(aoSqlDataReader["OriginalGradeId"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                };

                this.mlstGrades.Add(oFeedbackGrade);
            }
        }

        /// <summary>
        /// This method is used to set school info.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillSchoolInfo(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                this.moSchoolEntity = new SchoolEntity
                {
                    OrganizationName = aoSqlDataReader["School_Orgn_Name"].ToString(),
                    SchoolName = aoSqlDataReader["School_Name"].ToString(),
                    Address = aoSqlDataReader["Address"].ToString()
                };
            }
        } 

        #endregion
    }
}
