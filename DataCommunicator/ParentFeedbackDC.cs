using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class ParentFeedbackDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private List<ParentFeedbackGrade> mlstGrades = new List<ParentFeedbackGrade>();
        private List<ParentFeedbackDetails> mlstFeedbackDetails = new List<ParentFeedbackDetails>();

        #endregion

        #region Constructor(s)

        public ParentFeedbackDC()
        {
        }

        public ParentFeedbackDC(int aiSchoolId, int aiUpdatedByid)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedByid;
        }

        #endregion

        #region Property(s)

        public List<ParentFeedbackGrade> ParentFeedbackGrades
        {
            get { return mlstGrades; }
        }

        public List<ParentFeedbackDetails> ParentFeedbacks
        {
            get { return mlstFeedbackDetails; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is sued to check feedback submit status.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFeedbackId"></param>
        /// <returns></returns>
        public bool CheckIsFeedbackSubmit(int aiUserId, int aiSchoolId, int aiFeedbackId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeedbackId", aiFeedbackId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsSubmited", 0, SqlDbType.Bit, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CheckIsFeedbackSubmitted");
                return oSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// This method is sued to return feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiFeedbackId"></param>
        /// <returns></returns>
        public List<ParentFeedbackQuestion> GetAll(int aiUserId, int aiFeedbackId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<ParentFeedbackQuestion> lstQuestions = new List<ParentFeedbackQuestion>();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeedbackId", aiFeedbackId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetParentFeedbackDetails"))
                {
                    lstQuestions = FillFeedbackQuestions(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillGradeDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillParentFeedbackDetails(oSqlDataReader);

                    return lstQuestions;
                }
            }
        }

        /// <summary>
        /// This method is sued to save feedback details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asFeedbackXml"></param>
        /// <param name="aiFeedbackId"></param>
        public void Save(int aiUserId, string asFeedbackXml, int aiFeedbackId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeedbackId", aiFeedbackId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeedbackXml", asFeedbackXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveParentFeedback");
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to load feedback details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillParentFeedbackDetails(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstFeedbackDetails.Add
                    (
                          new ParentFeedbackDetails
                          {
                              QuestionId = aoSqlDataReader["QuestionId"].ToInt(),
                              GradeId = aoSqlDataReader["GradeId"].ToInt(),
                              Remark = aoSqlDataReader["Remark"].ToString()
                          }
                    );
            }
        }

        /// <summary>
        /// This method is used to load grade details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillGradeDetails(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstGrades.Add
                (
                      new ParentFeedbackGrade
                      {
                          Id = aoSqlDataReader["Id"].ToInt(),
                          Name = aoSqlDataReader["Name"].ToString(),
                          SortOrder = aoSqlDataReader["SortOrder"].ToInt()
                      }
                );
            }
        }

        /// <summary>
        /// This method is used to load user feedback details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<ParentFeedbackQuestion> FillFeedbackQuestions(SqlDataReader aoSqlDataReader)
        {
            List<ParentFeedbackQuestion> lstQuestions = new List<ParentFeedbackQuestion>();
            while (aoSqlDataReader.Read())
            {
                lstQuestions.Add
                    (
                          new ParentFeedbackQuestion
                          {
                              Id = aoSqlDataReader["Id"].ToInt(),
                              ControlId = aoSqlDataReader["ControlId"].ToInt(),
                              ParentQuestionId = aoSqlDataReader["ParentQuestionId"].ToInt(),
                              Title = aoSqlDataReader["Title"].ToString()
                          }
                    );
            }
            return lstQuestions;
        }

        #endregion
    }
}
