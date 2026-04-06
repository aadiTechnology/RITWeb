using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class StudentAssessmentDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;
        private ButtonStatesForStudentAssessment moButtonStatesForStudentAssessment;
        private List<CategorywiseComment> mlstCategorywiseComment;

        #endregion

        #region Constructor(s)

        public StudentAssessmentDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public StudentAssessmentDC()
        {

        }

        #endregion

        #region Property(s)
        
        public ButtonStatesForStudentAssessment ButtonStates
        {
            get { return moButtonStatesForStudentAssessment; }
        }

        public List<CategorywiseComment> CategorywiseComments
        {
            get { return mlstCategorywiseComment; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to get students to fill student dropdown.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiCategoryId"></param>
        /// <returns></returns>
        public DataTable GetStudents(int aiStudentId, int aiCategoryId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentsForAssessmentDetails");
            }
        }

        /// <summary>
        /// This method is used to get test names to fill test name dropdown.
        /// </summary>
        /// <returns></returns>
        public DataTable GetTestNames(int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTestsForStudent");
            }
        }

        public DataTable GetAcademicYear(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicYearForStudentAssessment");
            }
        }

        /// <summary>
        /// This method is used to fill listview.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public List<StudentAssessmentDetails> GetStudentAssessmentDetails(int aiAcademiYearId, int aiStandardId, int aiCategoryId, int aiStudentId, int aiTestId)
        {
            List<StudentAssessmentDetails> lstStudentAssessmentDetails = new List<StudentAssessmentDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademiYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDetailsOfStudentAssessment"))
                {
                    moButtonStatesForStudentAssessment = new ButtonStatesForStudentAssessment();
                    mlstCategorywiseComment = new List<CategorywiseComment>();
                    
                    while (oSqlDataReader.Read())
                    {
                        lstStudentAssessmentDetails.Add(new StudentAssessmentDetails
                        {
                            ParameterId = Convert.ToInt32(oSqlDataReader["ParameterId"]),
                            Aspect = Convert.ToString(oSqlDataReader["Aspect"]),
                            GradeId = Convert.ToInt32(oSqlDataReader["GradeId"]),
                        });
                    }

                    oSqlDataReader.NextResult();
                    if (oSqlDataReader.Read())
                    {
                        moButtonStatesForStudentAssessment.IsSaved = oSqlDataReader["IsSaved"].ToBool();
                        moButtonStatesForStudentAssessment.IsSubmitted = oSqlDataReader["IsSubmitted"].ToBool();
                    }

                    oSqlDataReader.NextResult();
                    while (oSqlDataReader.Read())
                    {
                        CategorywiseComment oCategorywiseComment = new CategorywiseComment();
                        oCategorywiseComment.ParameterId = oSqlDataReader["ParameterId"].ToInt();
                        oCategorywiseComment.Parameter = oSqlDataReader["Parameter"].ToString();
                        oCategorywiseComment.CommentForCategory = oSqlDataReader["Comment"].ToString();

                        mlstCategorywiseComment.Add(oCategorywiseComment);
                    }
                }
                return lstStudentAssessmentDetails;
            }
        }


       /// <summary>
        /// This method is used to get list of student fav details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiTestId"></param>
        /// <returns></returns>
        public List<StudentFavouriteListDetails> GetListOfStudentFavDetails(int aiAcademicYearId, int aiStandardId, int aiStudentId, int aiTestId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetListOfStudentFavDetails"))
                {
                    List<StudentFavouriteListDetails> lstStudentFavouriteListDetails = new List<StudentFavouriteListDetails>();

                    while (oSqlDataReader.Read())
                    {
                        lstStudentFavouriteListDetails.Add(new StudentFavouriteListDetails
                        {
                            ParameterId = Convert.ToInt32(oSqlDataReader["ParameterId"]),
                            Parameter = Convert.ToString(oSqlDataReader["Parameter"]),
                            Comment = Convert.ToString(oSqlDataReader["Comment"]),
                        });
                    }
                    return lstStudentFavouriteListDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to return saved favourite related details.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public StudentFavouriteDetails GetAll(int aiStudentId, int aiTestId, int aiAcdemicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentFavouriteDetails oStudentFavouriteDetails = new StudentFavouriteDetails();
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcdemicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllDetailsOfStudentAssessment"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oStudentFavouriteDetails.FavouriteColour = oSqlDataReader["FavouriteColour"].ToString();
                        oStudentFavouriteDetails.FavouriteFood = oSqlDataReader["FavouriteFood"].ToString();
                        oStudentFavouriteDetails.FavouriteSport = oSqlDataReader["FavouriteSport"].ToString();
                        oStudentFavouriteDetails.FavouriteSubject = oSqlDataReader["FavouriteSubject"].ToString();
                    }
                }
                return oStudentFavouriteDetails;
            }
        }

        /// <summary>
        /// This method is used to fill listview dropdown.
        /// </summary>
        /// <returns></returns>
        public DataTable GetGrades(int aiAcemicYearId)
        {
            if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            {
                string s1SelectStatement = " SELECT " +
                                        " Id " +
                                        " ,NewGradeName as Name " +
                                        " FROM " +
                                        " vw_AlternateObservationGrades ";
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(s1SelectStatement);
            }
            else
            {
                string sSelectStatement = " SELECT " +
                                            " Id " +
                                            " ,Name " +
                                            " FROM " +
                                            " ObservationGrades " +
                                            " WHERE " +
                                            "IsDeleted =" + Constants.S_ZERO +
                                            "AND AcademicYearId=" + aiAcemicYearId +
                                            "AND SchoolId=" + miSchoolId +
                                            "AND Id<=28" +
                                            " ORDER BY SortOrder";
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
            }
        }

        /// <summary>
        /// This method is used to save student assessment details.
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="aiTestId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aoStudentFavouriteDetails"></param>
        public void Save(string asXml, string asFavListXml, string asCategorywiseCommentXml, int aiAcademicYearId, int aiTestId, int aiStudentId, StudentFavouriteDetails aoStudentFavouriteDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentAssessmentDetailxml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("FavListDetailxml", asFavListXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CategorywiseCommentXml", asCategorywiseCommentXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FavouriteColour", aoStudentFavouriteDetails.FavouriteColour, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FavouriteFood", aoStudentFavouriteDetails.FavouriteFood, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FavouriteSport", aoStudentFavouriteDetails.FavouriteSport, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FavouriteSubject", aoStudentFavouriteDetails.FavouriteSubject, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentAssessmentDetails");
            }
        }

        /// <summary>
        /// This method is used to submit student assessment details.
        /// </summary>
        /// <param name="aiCategoryId"></param>
        /// <param name="aiTestId"></param>
        /// <param name="abIsSubmitted"></param>
        /// <param name="aiStudentId"></param>
        public void SubmitStudentAssessmentDetails(int aiAcademicYearId, int aiCategoryId, int aiTestId, bool abIsSubmitted, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitted", abIsSubmitted, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentAssessmentDetails");
            }
        }

        public bool AllowSelfAssessmentscreen()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", miUpdatedById, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("AllowSelfAssessmentscreen", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AllowSelfAssessmentscreen");
                return oSqlParameter.Value.ToBool();
            }
        }

        #endregion
    }
}
