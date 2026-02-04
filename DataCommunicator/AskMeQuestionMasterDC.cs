using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class AskMeQuestionMasterDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById; 

        #endregion

        #region Constructor(s)
        
        public AskMeQuestionMasterDC()
        {
        }

        public AskMeQuestionMasterDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion

        #region Public Method(s)
       
        /// <summary>
        /// This method is used to return all available questions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStatusId"></param>
        /// <param name="aiLoginUserId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <param name="abOnlyShowPublishedQueries"></param>
        /// <returns></returns>
        public static List<AskMeQuestionMaster> GetAllQuestions(int aiSchoolId, int aiAcademicYearId, int aiStatusId, int aiLoginUserId, string asSortExpression, string asSortDirection, int aiStartRowIndex, int aiEndRowIndex, bool abOnlyShowPublishedQueries, string asFilter, string asCategories)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StatusId", aiStatusId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndRowIndex", aiEndRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowPublishedQueries", Convert.ToInt32(abOnlyShowPublishedQueries), SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Categories", asCategories, SqlDbType.NVarChar);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllQuestions"))
                    return LoadQuestionDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to return uestion count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStatusId"></param>
        /// <param name="aiLoginUserId"></param>
        /// <param name="abOnlyShowPublishedQueries"></param>
        /// <returns></returns>
        //public static int GetCountOfQuestionDetails(int aiSchoolId, int aiAcademicYearId, int aiStatusId, int aiLoginUserId, bool abOnlyShowPublishedQueries, string asFilter, string asCategories)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("StatusId", aiStatusId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("ShowPublishedQueries", Convert.ToInt32(abOnlyShowPublishedQueries), SqlDbType.Bit);
        //        oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
        //        oSQLServerDbUtility.AddParameter("Categories", asCategories, SqlDbType.NVarChar);
        //        SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
        //        oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetCountOfQuestionDetails");
        //        return Convert.ToInt32(oSqlParameter.Value);
        //    }
        //}

        /// <summary>
        /// This method is used to return all communications of given question id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="asSortDirection"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <param name="aiEndRowIndex"></param>
        /// <returns></returns>
        public static List<AskMeQuestionDetails> GetAllQuestionCommunications(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, string asSortExpression, string asSortDirection, int aiStartRowIndex, int aiEndRowIndex, int aiLoginUserId, bool abShowOnPublishedQuery)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndRowIndex", aiEndRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowOnPublishedQuery", abShowOnPublishedQuery, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllQuestionComminucations"))
                {
                    List<AskMeQuestionDetails> lstCommunications = new List<AskMeQuestionDetails>();
                    AskMeQuestionDetails oAskMeQuestionDetails;
                    while (oSqlDataReader.Read())
                    {
                        oAskMeQuestionDetails = new AskMeQuestionDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            AttachedFileName = (oSqlDataReader["AttachedFileName"] == DBNull.Value ? string.Empty : Convert.ToString(oSqlDataReader["AttachedFileName"])),
                            Comment = Convert.ToString(oSqlDataReader["Comment"]),
                            HasReadMessage = Convert.ToBoolean(oSqlDataReader["HasReadMessage"]),
                            Date = Convert.ToDateTime(oSqlDataReader["Date"]),
                            SenderName = Convert.ToString(oSqlDataReader["SenderName"]),
                            SenderUserId = Convert.ToInt32(oSqlDataReader["SenderUserId"]),
                            IsPublished = Convert.ToBoolean(oSqlDataReader["IsPublished"]),
                            IsEditable = Convert.ToBoolean(oSqlDataReader["IsEditable"]),
                            IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]),
                            IsInvalidQuery = Convert.ToBoolean(oSqlDataReader["IsInvalidQuery"])
                        };
                        lstCommunications.Add(oAskMeQuestionDetails);
                    }
                    return lstCommunications;
                }
            }
        }
        
        /// <summary>
        /// This method is used to return count of communications of given question id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public static int GetCountOfQuestionCommunications(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, int aiLoginUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetCountOfQuestionDetailCommunications");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to return all statuses.
        /// </summary>
        /// <returns></returns>
        public List<AskMeStatusMaster> GetAllStatuses()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllQuestionStatuses"))
                {
                    List<AskMeStatusMaster> lstStatuses = new List<AskMeStatusMaster>();
                    AskMeStatusMaster oAskMeStatusMaster;
                    while (oSqlDataReader.Read())
                    {
                        oAskMeStatusMaster = new AskMeStatusMaster
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            Name = Convert.ToString(oSqlDataReader["Name"])
                        };
                        lstStatuses.Add(oAskMeStatusMaster);
                    }
                    return lstStatuses;
                }
            }
        }

        /// <summary>
        /// This method is used to delete communication.
        /// </summary>
        /// <param name="aiQuestionDetailsId"></param>
        /// <param name="aiUpdatedById"></param>
        public static void DeleteQuestionDetails(int aiQuestionDetailsId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("QuestionDetailsId", aiQuestionDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteQuestionDetails");
            }
        }

        /// <summary>
        /// This method is used to return communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionDetailsId"></param>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public static AskMeQuestionMaster GetQuestionDetails(int aiSchoolId, int aiAcademicYearId, int aiQuestionDetailsId, int aiQuestionId, int aiLoginUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionDetailsId", aiQuestionDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetQuestionDetails"))
                {
                    AskMeQuestionMaster oAskMeQuestionMaster = new AskMeQuestionMaster();

                    if (oSqlDataReader.Read())
                    {
                        oAskMeQuestionMaster.OwnerUserId = 0;
                        if (oSqlDataReader["OwnerUserId"] != DBNull.Value)
                            oAskMeQuestionMaster.OwnerUserId = Convert.ToInt32(oSqlDataReader["OwnerUserId"]);

                        oAskMeQuestionMaster.IsCommunicationStarted = Convert.ToBoolean(oSqlDataReader["IsCommunicationStarted"]);
                        oAskMeQuestionMaster.StatusId = Convert.ToInt32(oSqlDataReader["StatusId"]);
                        oAskMeQuestionMaster.Title = Convert.ToString(oSqlDataReader["Title"]);
                        oAskMeQuestionMaster.UserRoleId = Convert.ToInt32(oSqlDataReader["User_Role_Id"]);
                        oAskMeQuestionMaster.StudentUserId = Convert.ToInt32(oSqlDataReader["StudentUserId"]);
                        oAskMeQuestionMaster.CommunicationStartDate = Convert.ToDateTime(oSqlDataReader["CommunicationStartDate"]);
                        oAskMeQuestionMaster.AssociatedCategories = Convert.ToString(oSqlDataReader["AssociatedCategories"]);
                        oAskMeQuestionMaster.CategoryNames = Convert.ToString(oSqlDataReader["CategoryNames"]);

                        oAskMeQuestionMaster.AskMeQuestionDetails = new AskMeQuestionDetails();

                        oAskMeQuestionMaster.AskMeQuestionDetails.Id = Convert.ToInt32(oSqlDataReader["Id"]);
                        oAskMeQuestionMaster.AskMeQuestionDetails.SenderUserId = Convert.ToInt32(oSqlDataReader["SenderUserId"]);
                        oAskMeQuestionMaster.AskMeQuestionDetails.AttachedFileName = string.Empty;
                        oAskMeQuestionMaster.AskMeQuestionDetails.IsSubmitted = Convert.ToBoolean(oSqlDataReader["IsSubmitted"]);
                        if (oSqlDataReader["AttachedFileName"] != DBNull.Value)
                            oAskMeQuestionMaster.AskMeQuestionDetails.AttachedFileName = Convert.ToString(oSqlDataReader["AttachedFileName"]);

                        if (oSqlDataReader["LastDescription"] != DBNull.Value)
                            oAskMeQuestionMaster.AskMeQuestionDetails.LastDescription = Convert.ToString(oSqlDataReader["LastDescription"]);

                        oAskMeQuestionMaster.AskMeQuestionDetails.Comment = Convert.ToString(oSqlDataReader["Comment"]);
                        oAskMeQuestionMaster.AskMeQuestionDetails.Date = Convert.ToDateTime(oSqlDataReader["Date"]);
                        oAskMeQuestionMaster.IsOwnerAssignmentSubmitted = Convert.ToBoolean(oSqlDataReader["IsOwnerAssignmentSubmitted"]);
                        oAskMeQuestionMaster.IsPublishBtnEnabled = Convert.ToBoolean(oSqlDataReader["PublishedEnable"]);
                        oAskMeQuestionMaster.IsQueryPublished = Convert.ToBoolean(oSqlDataReader["QueryPublished"]);
                        oAskMeQuestionMaster.IsCategoryEnabled = Convert.ToBoolean(oSqlDataReader["IsCategoryEnabled"]);
                    }

                    return oAskMeQuestionMaster;
                }
            }
        }

        /// <summary>
        /// This method is used to save communication details.
        /// </summary>
        /// <param name="aoAskMeQuestionMaster"></param>
        public void SaveCommunicationDetails(AskMeQuestionMaster aoAskMeQuestionMaster)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LastUpdatedDate", aoAskMeQuestionMaster.LastUpdatedDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("OwnerUserId", aoAskMeQuestionMaster.OwnerUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsClosedStatus", Convert.ToInt32(aoAskMeQuestionMaster.IsClosedStatus), SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Title", aoAskMeQuestionMaster.Title, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserRoleId", aoAskMeQuestionMaster.UserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aoAskMeQuestionMaster.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SenderUserId", aoAskMeQuestionMaster.AskMeQuestionDetails.SenderUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AttachedFileName", aoAskMeQuestionMaster.AskMeQuestionDetails.AttachedFileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Comment", aoAskMeQuestionMaster.AskMeQuestionDetails.Comment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("QuestionDetailsId", aoAskMeQuestionMaster.AskMeQuestionDetails.Id, SqlDbType.Int);                
                oSQLServerDbUtility.AddParameter("AssociatedCategories", aoAskMeQuestionMaster.AssociatedCategories, SqlDbType.NVarChar);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveCommunicationDetails");
            }
        }

        /// <summary>
        /// This method is used to publish communication details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsPublish"></param>
        public static void PublishCommunication(int aiSchoolId, int aiAcademicYearId, int aiQuestionId, int aiUpdatedById, bool abIsPublish)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublish", abIsPublish, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PublishCommunicationDetails");
            }
        } 

        #endregion

        #region Private Method(s)
        
        /// <summary>
        /// This method is sued to load question details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private static List<AskMeQuestionMaster> LoadQuestionDetails(SqlDataReader aoSqlDataReader)
        {
            List<AskMeQuestionMaster> lstQuestions = new List<AskMeQuestionMaster>();
            AskMeQuestionMaster oAskMeQuestionMaster;
            while (aoSqlDataReader.Read())
            {
                oAskMeQuestionMaster = new AskMeQuestionMaster
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    OwnerUserId = (aoSqlDataReader["OwnerUserId"] == DBNull.Value ? 0 : Convert.ToInt32(aoSqlDataReader["OwnerUserId"])),
                    Status = Convert.ToString(aoSqlDataReader["Status"]),
                    StatusId = Convert.ToInt32(aoSqlDataReader["StatusId"]),
                    Title = Convert.ToString(aoSqlDataReader["Title"]),
                    LastUpdatedDate = Convert.ToDateTime(aoSqlDataReader["LastUpdatedDate"]),
                    IsPublished = Convert.ToBoolean(aoSqlDataReader["IsPublished"]),
                    QueryNo = 1000 + Convert.ToInt32(aoSqlDataReader["Id"]),
                    ShowOwnerButton = Convert.ToBoolean(aoSqlDataReader["ShowOwnerButton"]),
                    IsQueryInUnsubmitState = Convert.ToBoolean(aoSqlDataReader["IsQueryInUnsubmitState"]),
                    AllowReply = Convert.ToBoolean(aoSqlDataReader["AllowReply"]),
                    AllowForward = Convert.ToBoolean(aoSqlDataReader["AllowForward"]),
                    AllowBackward = Convert.ToBoolean(aoSqlDataReader["AllowBackward"]),
                    IsInvalidQuestion = Convert.ToBoolean(aoSqlDataReader["IsInvalidQuestion"]),
                    ShowInvalidButton = Convert.ToBoolean(aoSqlDataReader["ShowInvalidButton"]),
                    ShowPublishButton = Convert.ToBoolean(aoSqlDataReader["ShowPublishButton"]),
                    TotalRowCount = Convert.ToInt32(aoSqlDataReader["TotalRowCount"])
                };
                lstQuestions.Add(oAskMeQuestionMaster);
            }
            return lstQuestions;
        } 

        /// <summary>
        /// This method is used to submit communication.
        /// </summary>
        /// <param name="aiQuestionDetailsId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsSubmitted"></param>
        public static void SubmitCommunication(int aiSchoolId, int aiQuestionDetailsId, int aiUpdatedById, bool abIsSubmitted)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("QuestionDetailsId", aiQuestionDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitted", abIsSubmitted, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitComunication");
            }
        }

        /// <summary>
        /// This method is used to mark query as invalid.
        /// </summary>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsInvalid"></param>
        public static void MarkValidityStatus(int aiQuestionId, int aiSchoolId, int aiAcademicYearId, int aiUpdatedById, bool abIsInvalidAction)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInvalidQuery", Convert.ToInt32(abIsInvalidAction), SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkQueryAsInvalid");
            }
        }

        /// <summary>
        /// This method is used to return all categories.
        /// </summary>
        /// <returns></returns>
        public List<AskMeCategory> GetAllCategories()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<AskMeCategory> lstCategories = new List<AskMeCategory>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAskMeCategories"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstCategories.Add
                            (
                                new AskMeCategory
                                {
                                    Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                    Name = Convert.ToString(oSqlDataReader["Name"])
                                }
                            );
                    }
                    return lstCategories;
                }
}
        }

        public static List<AskMeCommunicationDetails> GetAskMeCommunication(int aiSchoolId, int aiAcademicYearId, int aiQuestionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAskMeCommunication"))
                    return LoadAskMeCommunication(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to add values into entity class.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private static List<AskMeCommunicationDetails> LoadAskMeCommunication(SqlDataReader aoSqlDataReader)
        {
            List<AskMeCommunicationDetails> lstStudentDetails = new List<AskMeCommunicationDetails>();
            AskMeCommunicationDetails oAcademicYearwiseLeftStudentDetails;
            while (aoSqlDataReader.Read())
            {
                oAcademicYearwiseLeftStudentDetails = new AskMeCommunicationDetails()
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    CommunicationDate = Convert.ToDateTime(aoSqlDataReader["CommunicationDate"]),
                    SenderUserName = Convert.ToString(aoSqlDataReader["SenderUserName"]),
                    Communication = Convert.ToString(aoSqlDataReader["Communication"]),
                    MainQuestion = Convert.ToString(aoSqlDataReader["MainQuestion"]),
                    IsDisplayCommunication = Convert.ToBoolean(aoSqlDataReader["IsDisplayCommunication"]),
                    IsPublished = Convert.ToBoolean(aoSqlDataReader["IsPublished"])                    
                };
                lstStudentDetails.Add(oAcademicYearwiseLeftStudentDetails);
            }
            return lstStudentDetails;
        }


        /// <summary>
        /// This method is used to set owner assignment.
        /// </summary>
        /// <param name="aoAskMeQuestionMaster"></param>
        public void SetOwnerAssignment(AskMeOwnerAssignment aoAskMeOwnerAssignment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aoAskMeOwnerAssignment.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OwnerId", aoAskMeOwnerAssignment.OwnerId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aoAskMeOwnerAssignment.QuestionId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AssignQueryOwner");
            }
        }

        /// <summary>
        /// This method is used to assign communication.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiQuestionId"></param>
        /// <param name="aiUpdatedById"></param>
        /// <param name="abIsForward"></param>
        /// <param name="aiAcademicYearId"></param>
        public static void AssignCommunication(int aiSchoolId, int aiQuestionId, int aiUpdatedById, bool abIsForward, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsForward", abIsForward, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AssignCommunication");
            }
        }

        /// <summary>
        /// This method is used to return subject experts.
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<SubjectExperts> GetSubjectExperts(int aiSubjectId)
        {
            List<SubjectExperts> lstSubjectExperts = new List<SubjectExperts>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAskMeSubjectTeachers"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSubjectExperts.Add(new SubjectExperts
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UserName = Convert.ToString(oSqlDataReader["UserName"]),
                            IsAssignExpert = Convert.ToBoolean(oSqlDataReader["IsExpert"])
                        });
                    }
                }
            }
            return lstSubjectExperts;
        }

        /// <summary>
        /// This method is used to save subject experts.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="asTeacherId"></param>
        public void SaveSubjectExperts(int aiSchoolId, int aiAcademicYearId, int aiSubjectId, string asTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", asTeacherId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConfigId", Constants.SchoolConfigMenuId.Ask_Me_Related.ToInt(), SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSubjectExpertsForAskMe");
            }
        }

        /// <summary>
        /// This method is used to return all owners.
        /// </summary>
        /// <param name="aiQuestionId"></param>
        /// <returns></returns>
        public List<AskMeOwnerAssignment> GetAllOwners(int aiQuestionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllAskMeOwners"))
                {
                    List<AskMeOwnerAssignment> lstOwners = new List<AskMeOwnerAssignment>();
                    while (oSqlDataReader.Read())
                    {
                        AskMeOwnerAssignment oAskMeOwnerAssignment = new AskMeOwnerAssignment
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            OwnerId = Convert.ToInt32(oSqlDataReader["OwnerId"]),
                            UserRoleId = Convert.ToInt32(oSqlDataReader["User_Role_Id"]),
                            OwnerName = Convert.ToString(oSqlDataReader["UserName"]),
                            UserRole = Convert.ToString(oSqlDataReader["User_Role_Name"])
                        };
                        lstOwners.Add(oAskMeOwnerAssignment);
                    }

                    return lstOwners;
                }
            }
        }

        /// <summary>
        /// This method is used to return subject teachers.
        /// </summary>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public List<AskMeOwnerAssignment> GetAllSubjectTeachers(int aiUserRoleId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllSubjectTeachers"))
                {
                    List<AskMeOwnerAssignment> lstOwners = new List<AskMeOwnerAssignment>();
                    while (oSqlDataReader.Read())
                    {
                        AskMeOwnerAssignment oAskMeOwnerAssignment = new AskMeOwnerAssignment
                        {
                            OwnerId = Convert.ToInt32(oSqlDataReader["OwnerId"]),
                            OwnerName = Convert.ToString(oSqlDataReader["OwnerName"])
                        };
                        lstOwners.Add(oAskMeOwnerAssignment);
                    }

                    return lstOwners;
                }
            }
        }

        /// <summary>
        /// This method is used to submit owner assignment.
        /// </summary>
        /// <param name="aiQuestionId"></param>
        /// <param name="abIsSubmit"></param>
        public void SubmitOwnerAssignment(int aiQuestionId, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitOwnerAssignment");
            }
        }

        /// <summary>
        /// This method is used to delete owner assignment.
        /// </summary>
        /// <param name="aiId"></param>
        public void DeleteOwnerAssignment(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteOwnerAssignment");
            }
        }

        /// <summary>
        /// This method will used for get the count of unread Questions.
        /// </summary>
        /// <param name="aiUserId"></param>
        public int GetCountOfUnreadQuestion(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                DataTable oDataTable = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCntOfAskMeUnreadQuestion");
                return Convert.ToInt32(oDataTable.Rows[0][0]);
            }
        }

        /// <summary>
        /// This method is used to get the Read Receipt Details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiQuestionId"></param>
        /// <pparam name="aiAcademicYearId"></param>
        public List<AskMeReadReceiptDetails> GetReadReceiptDetails(int aiSchoolId, int aiQuestionId, int aiAcademicYearId, int aiLoginUserId)
        {
            List<AskMeReadReceiptDetails> lstAskMeReadReceiptDetails = new List<AskMeReadReceiptDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                AskMeReadReceiptDetails oAskMeReadReceiptDetails = new AskMeReadReceiptDetails();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionId", aiQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAskMeReadReceiptDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstAskMeReadReceiptDetails.Add(new AskMeReadReceiptDetails
                        {
                            Name = Convert.ToString(oSqlDataReader["Name"]),
                        });
                    }
                }
            }
            return lstAskMeReadReceiptDetails;
        }

        /// <summary>
        /// This method is used to save selectd communicatin details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSelectedQuestionIds"></param>
        /// <param name="aiMasterQuestionId"></param>
        /// <param name="aiUserId"></param>
        public static void SaveSelection(int aiSchoolId, string asSelectedQuestionIds, int aiMasterQuestionId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QuestionDetailIds", asSelectedQuestionIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MasterQuestionId", aiMasterQuestionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSelectedCommunicationDetails");
            }
        }

        #endregion
    }
}
