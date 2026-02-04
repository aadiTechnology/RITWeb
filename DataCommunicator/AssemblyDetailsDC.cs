// Class Name       :- AssemblyDetailsDC
// Purpose          :- This class is used to manage Assembly Details.
// Date Of creation :- 13/02/2016
// Author Name      :- Dnyaneshwar Shinde.


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using StaffPerformanceEntity;

namespace DataCommunicator
{
    public class AssemblyDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private List<AssemblyQuestions> mlstQuestions;
        private List<AssemblyAnswers> mlstAnswers;
        private List<StandardDetails> mlstStandards;
        private ButtonState moButtonState;

        #endregion

        #region Constructor(s)

        public AssemblyDetailsDC() { }

        public AssemblyDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        #endregion

        #region Property(s)

        public List<AssemblyQuestions> AssemblyQuestions
        {
            get { return this.mlstQuestions; }
        }

        public List<AssemblyAnswers> AssemblyAnswers
        {
            get { return this.mlstAnswers; }
        }

        public List<StandardDetails> StandardDetails
        {
            get { return this.mlstStandards; }
        }

        public ButtonState ButtonStates
        {
            get { return this.moButtonState; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return Assembly details.
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public List<AssemblyDetails> GetAllAssemblyDetails(DateTime asDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", asDate, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAssemblyDetails"))
                {
                    List<AssemblyDetails> lstAssembly = FillAssemblyDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillAssemblyQuestions(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillAssemblyAnswers(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStandards(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetButtonState(oSqlDataReader);
                    return lstAssembly;
                }
            }
        }

        /// <summary>
        /// This method will fetch all the Assembly Details from the database. It will simply convert the result into a List object and return it.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name ="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<AssemblyDetails> GetAllAssemblyList()
        {
            List<AssemblyDetails> lstAssemblyList = new List<AssemblyDetails>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAssemblyDetailsList"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstAssemblyList.Add(new AssemblyDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            Date = Convert.ToDateTime(oSqlDataReader["SubmitDate"]),
                            IsSubmit = Convert.ToBoolean(oSqlDataReader["IsSubmit"]),
                            IsPublish = Convert.ToBoolean(oSqlDataReader["IsPublish"])
                        });
                    }
                }
            }
            return lstAssemblyList;
        }

        /// <summary>
        /// This method is used to Delete Assembly details.
        /// </summary>        
        /// <param name="aiAssemblyId"></param>
        public void DeleteAssembly(int aiAssemblyId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AssemblyId", aiAssemblyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteAssemblyDetails");
            }
        }

        /// <summary>
        /// This method is used to save Assembly details.
        /// </summary>        
        /// <param name="asXml"></param>
        /// <param name="dtDate"></param>
        /// <param name="aiAssemblyId"></param>
        public void Save(string asXml, DateTime asDate, int aiAssemblyId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertDate", asDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("AssemblyXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("AssemblyId", aiAssemblyId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveAssemblyDetails");
            }
        }

        /// <summary>
        /// This method is used to submit Assembly details.
        /// </summary>
        /// <param name="dtDate"></param>
        /// <param name="IsSubmited"></param>
        /// <param name="aiAssemblyId"></param>
        public void Submit(DateTime asDate, bool abIsSubmited, int aiAssemblyId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubmitDate", asDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmited", abIsSubmited, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AssemblyId", aiAssemblyId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitAssemblyDetails");
            }
        }

        /// <summary>
        /// This method is used to publish Assembly details.
        /// </summary>
        /// <param name="dtTime"></param>
        /// <param name="abIsPublish"></param>
        /// <param name="aiAssemblyId"></param>
        public void Publish(DateTime asDate, bool abIsPublish, int aiAssemblyId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublish", abIsPublish, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AssemblyId", aiAssemblyId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", asDate, SqlDbType.DateTime);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishAssemblyDetails");
            }
        }

        public DataTable GetAllAssemblyQuestionsForConfiguration()
        {
            string sQuery = " SELECT " +
                                 " Id, " +
                                 " Name" +
                             " FROM " +
                                 " AssemblyAnswerGroupMaster " +
                             " WHERE " +
                                 " IsDeleted = 0 " +
                                 " AND " +
                                 " SchoolId = " + this.miSchoolId +
                                 " AND " +
                                 " AcademicYearId = " + this.miAcademicYearId;                                 

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
           
        }

        public DataTable GetAllAssemblyParentQuestions()
        {
            string sQuery = " SELECT " +
                                 " Id AS ParentQueId, " +
                                 " Name AS ParentQueName" +
                             " FROM " +
                                 " AssemblyQuestions " +
                             " WHERE " +
                                 " SchoolId = " + this.miSchoolId +
                                 " AND " +
                                 " AcademicYearId = " + this.miAcademicYearId +
                                 "AND " +
                                 "ParentQuestionId = " + 0;


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

        public void Save(AssemblyQuestionConfiguration aoAssemblyQuestionConfiguration)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aoAssemblyQuestionConfiguration.AssemblyId, SqlDbType.Int);
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
                this.moButtonState.EnableSaveButton = Convert.ToBoolean(aoSqlDataReader["IsSaved"]);
                this.moButtonState.EnableSubmitButton = Convert.ToBoolean(aoSqlDataReader["IsSubmited"]);
                this.moButtonState.EnablePublishButton = Convert.ToBoolean(aoSqlDataReader["IsPublished"]);
                this.moButtonState.IsApprover = Convert.ToBoolean(aoSqlDataReader["IsApprover"]);
            }
        }

        /// <summary>
        /// This method is used to get assembly details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<AssemblyDetails> FillAssemblyDetails(SqlDataReader aoSqlDataReader)
        {
            List<AssemblyDetails> lstAssembly = new List<AssemblyDetails>();
            while (aoSqlDataReader.Read())
            {
                lstAssembly.Add
                  (
                    new AssemblyDetails
                    {
                        Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                        AssemblyId = Convert.ToInt32(aoSqlDataReader["AssemblyId"]),
                        QuestionId = Convert.ToInt32(aoSqlDataReader["QuestionId"]),
                        AnswerId = Convert.ToInt32(aoSqlDataReader["AnswerId"]),                        
                        FreeTextValue = Convert.ToString(aoSqlDataReader["FreeTextValue"]),
                        PhotoFilePath = Convert.ToString(aoSqlDataReader["PhotoFilePath"]),
                        AssemblyPhoto = Convert.ToString(aoSqlDataReader["AssemblyPhoto"])
                    }

                  );
            }
            return lstAssembly;
        }

        /// <summary>
        /// This method is used to fill Assembly questions.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillAssemblyQuestions(SqlDataReader aoSqlDataReader)
        {
            this.mlstQuestions = new List<AssemblyQuestions>();
            while (aoSqlDataReader.Read())
            {
                this.mlstQuestions.Add
                    (
                        new AssemblyQuestions
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"]),
                            GroupId = Convert.ToInt32(aoSqlDataReader["GroupId"]),
                            ParentQuestionId = Convert.ToInt32(aoSqlDataReader["ParentQuestionId"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"])
                        }

                    );
            }
        }

        /// <summary>
        /// This method is used to fill Assembly answers.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillAssemblyAnswers(SqlDataReader aoSqlDataReader)
        {
            this.mlstAnswers = new List<AssemblyAnswers>();
            while (aoSqlDataReader.Read())
            {
                this.mlstAnswers.Add
                    (
                        new AssemblyAnswers
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            AnswerId = Convert.ToInt32(aoSqlDataReader["AnswerId"]),
                            Answer = Convert.ToString(aoSqlDataReader["Answer"]),
                            AnswerGroupId = Convert.ToInt32(aoSqlDataReader["AnswerGroupId"]),
                            InputControlId = Convert.ToInt32(aoSqlDataReader["InputcontrolId"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill Standards.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillStandards(SqlDataReader aoSqlDataReader)
        {
            this.mlstStandards = new List<StandardDetails>();
            while (aoSqlDataReader.Read())
            {
                this.mlstStandards.Add
                    (
                        new StandardDetails
                        {
                            StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]),
                            StandardName = Convert.ToString(aoSqlDataReader["StandardName"])
                        }
                    );
            }
        }

        #endregion
    }
}
