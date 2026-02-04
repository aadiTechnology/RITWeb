using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using StaffPerformanceEntity;

namespace DataCommunicator
{
    public class UserSurveyDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private List<SurveyQuestion> mlstQuestions;
        private List<SurveyAnswer> mlstAnswers;
        private SurveyUserDetails moSurveyUserDetails;
        private List<SurveyHeader> mlstSurveyHeaders;
        private ButtonState moButtonState;

        #endregion

        #region Constructor(s)

        public UserSurveyDC()
        {
        }

        public UserSurveyDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region Property(s)

        public List<SurveyQuestion> SurveyQuestions
        {
            get { return this.mlstQuestions; }
        }

        public List<SurveyAnswer> SurveyAnswers
        {
            get { return this.mlstAnswers; }
        }

        public SurveyUserDetails UserDetails
        {
            get { return this.moSurveyUserDetails; }
        }

        public List<SurveyHeader> SurveyHeaders
        {
            get { return this.mlstSurveyHeaders; }
        }

        public ButtonState ButtonStates
        {
            get { return this.moButtonState; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return survey details.
        /// </summary>
        /// <param name="aiSurveyId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<UserSurveyDetails> GetUserSurveyDetails(int aiSurveyId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserSurveyDetails");
                List<UserSurveyDetails> lstSurveys = FillUserSurveyDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                this.FillSurveyQuestions(oSqlDataReader);

                oSqlDataReader.NextResult();
                this.FillSurveyAnswers(oSqlDataReader);

                oSqlDataReader.NextResult();
                this.FillUserDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                this.FillSurveyHeaders(oSqlDataReader);

                oSqlDataReader.NextResult();
                this.SetButtonState(oSqlDataReader);

                return lstSurveys;
            }
        }

        /// <summary>
        /// This method is used to save survey details.
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
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUserSurveyDetails");
            }
        }

        /// <summary>
        /// This method is used to submit survey details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSurveyId"></param>
        public void Submit(int aiUserId, int aiSurveyId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitUserSurveyDetails");
            }
        }

        /// <summary>
        /// This method is used to return all survey users.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSurveyId"></param>
        /// <param name="asFilter"></param>
        /// <param name="aiUserRoleId"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public static List<SurveyUserDetails> GetAllUsers(int aiSchoolId, int aiAcademicYearId, int aiSurveyId,  string asFilter, int aiUserRoleId, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SurveyId", aiSurveyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUsersForSurvey");

                List<SurveyUserDetails> lstUsers = new List<SurveyUserDetails>();
                while (oSqlDataReader.Read())
                {
                    SurveyUserDetails oSurveyUserDetails = new SurveyUserDetails
                    {
                        RegNo = Convert.ToString(oSqlDataReader["Enrolment_Number"]),
                        ClassName = Convert.ToString(oSqlDataReader["ClassName"]),
                        UserName = Convert.ToString(oSqlDataReader["UserName"]),
                        TotalRecordCount = Convert.ToInt32(oSqlDataReader["TotalRecordCount"]),
                        UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                        IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]),
                        AllowSubmission = Convert.ToBoolean(oSqlDataReader["AllowSubmission"])
                    };
                    lstUsers.Add(oSurveyUserDetails);
                }
                return lstUsers;
            }
        }

        /// <summary>
        /// This method is used to return all surveys.
        /// </summary>
        /// <returns></returns>
        public List<SurveyConfig> GetAllSurveys()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSurveyConfigs");
                List<SurveyConfig> lstSurveys = new List<SurveyConfig>();
                while (oSqlDataReader.Read())
                {
                    lstSurveys.Add
                        (
                            new SurveyConfig
                            {
                                Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                SurveyName = Convert.ToString(oSqlDataReader["Title"])
                            }
                        );
                }
                return lstSurveys;
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to set button state.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetButtonState(SqlDataReader aoSqlDataReader)
        {
            this.moButtonState = new ButtonState();
            if (aoSqlDataReader.Read())
            {
                this.moButtonState.EnableSubmitButton = Convert.ToBoolean(aoSqlDataReader["EnableSubmitButton"]);
                this.moButtonState.EnableSaveButton = Convert.ToBoolean(aoSqlDataReader["EnableSaveButton"]);
            }
        }

        /// <summary>
        /// This method is used to fill survey headers.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillSurveyHeaders(SqlDataReader aoSqlDataReader)
        {
            this.mlstSurveyHeaders = new List<SurveyHeader>();
            while (aoSqlDataReader.Read())
            {
                this.mlstSurveyHeaders.Add
                    (
                        new SurveyHeader
                        {
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                            Header = Convert.ToString(aoSqlDataReader["Header"])
                        }

                    );
            }
        }

        /// <summary>
        /// This method is used to fill user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            this.moSurveyUserDetails = new SurveyUserDetails();
            if (aoSqlDataReader.Read())
            {
                this.moSurveyUserDetails.ClassName = Convert.ToString(aoSqlDataReader["ClassName"]);
                this.moSurveyUserDetails.EmailAddress = Convert.ToString(aoSqlDataReader["EmailAddress"]);
                this.moSurveyUserDetails.MobileNumber = Convert.ToString(aoSqlDataReader["Mobile_Number"]);
                this.moSurveyUserDetails.ParentName = Convert.ToString(aoSqlDataReader["Parent_Name"]);
                this.moSurveyUserDetails.UserName = Convert.ToString(aoSqlDataReader["StudentName"]);
                this.moSurveyUserDetails.SchoolName = Convert.ToString(aoSqlDataReader["School_Name"]);
                this.moSurveyUserDetails.SchoolAddress = Convert.ToString(aoSqlDataReader["Address"]);
            }
        }

        /// <summary>
        /// This method is used to fill survey answers.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillSurveyAnswers(SqlDataReader aoSqlDataReader)
        {
            this.mlstAnswers = new List<SurveyAnswer>();
            while (aoSqlDataReader.Read())
            {
                this.mlstAnswers.Add
                    (
                        new SurveyAnswer
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            SurveyGroupId = Convert.ToInt32(aoSqlDataReader["SurveyGroupId"]),
                            InputControlId = Convert.ToInt32(aoSqlDataReader["InputControlId"]),
                            SurveyAnswerId = Convert.ToInt32(aoSqlDataReader["SurveyAnswerId"]),
                            Answer = Convert.ToString(aoSqlDataReader["Answer"])
                        }

                    );
            }
        }

        /// <summary>
        /// This method is used to fill survey questions.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillSurveyQuestions(SqlDataReader aoSqlDataReader)
        {
            this.mlstQuestions = new List<SurveyQuestion>();
            while (aoSqlDataReader.Read())
            {
                this.mlstQuestions.Add
                    (
                        new SurveyQuestion
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            AllowFreeText = Convert.ToBoolean(aoSqlDataReader["AllowFreeText"]),
                            ParentQuestionId = Convert.ToInt32(aoSqlDataReader["ParentQuestionId"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                            SurveyGroupId = Convert.ToInt32(aoSqlDataReader["SurveyGroupId"]),
                            Title = Convert.ToString(aoSqlDataReader["Title"])
                        }

                    );
            }
        }

        /// <summary>
        /// This method is used to survey details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<UserSurveyDetails> FillUserSurveyDetails(SqlDataReader aoSqlDataReader)
        {
            List<UserSurveyDetails> lstSurveys = new List<UserSurveyDetails>();
            while (aoSqlDataReader.Read())
            {
                lstSurveys.Add
                  (
                    new UserSurveyDetails
                    {
                        AnswerId = Convert.ToInt32(aoSqlDataReader["AnswerId"]),
                        FreeTextValue = Convert.ToString(aoSqlDataReader["FreeTextValue"]),
                        Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                        QuestionId = Convert.ToInt32(aoSqlDataReader["QuestionId"])
                    }

                  );
            }

            return lstSurveys;
        }

        #endregion
    }
}
