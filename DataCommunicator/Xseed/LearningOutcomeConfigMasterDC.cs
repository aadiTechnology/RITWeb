// Class Name       :- LearningOutcomeConfigMasterDC
// Purpose          :- This class is used to managelearnig outcomes.
// Date Of creation :- 5/24/2011
// Author Name      :- Vipul Jadhav
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Utility;
using XseedReportEntities;
using MasterEntities;

namespace DataCommunicator
{
    public class LearningOutcomeConfigMasterDC
    {
        #region "Data Members"

        public LearningOutcomeConfigMaster moLearningOutcomeConfigMaster;
        public LearningOutcomesSubmitStatus moLearningOutcomesSubmitStatus;
        public bool bIsSubmitted;
        public bool bGradeSubmitStatus;

        #endregion "Data Members"

        #region "Constructors"
        public LearningOutcomeConfigMasterDC()
        {
            moLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster();
            moLearningOutcomesSubmitStatus = new LearningOutcomesSubmitStatus();
        }

        #endregion "Constructors"

        #region "Public Methods"

        /// <summary>
        /// This method is used to insert the learning outcome details.
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            string sInsertStatement = "INSERT INTO [Xseed].[LearningOutcomeConfigMaster] " +
                                      "([StandardwiseAssessmentId]" +
                                      ",[SubjectSectionConfigId]" +
                                      ",[LearningOutCome]" +
                                      ",[IsSubmitted]" +
                                      ",[SortOrder]" +
                                      ",[SchoolId]" +
                                      ",[Academic_Year_Id]" +
                                      ",[InsertedById]" +
                                      ",[InsertDate]" +
                                      ")VALUES(" +
                                      " " + moLearningOutcomeConfigMaster.StandardwiseAssessmentId +
                                      " , " + moLearningOutcomeConfigMaster.SubjectSectionConfigId +
                                      " , N'" + StringUtility.ReplaceSingleQuoteInString(moLearningOutcomeConfigMaster.LearningOutCome, false) + "' " +
                                      " , N'" + moLearningOutcomeConfigMaster.IsConsidered + "'" +
                                      " , " + moLearningOutcomeConfigMaster.SortOrder +
                                      " , " + moLearningOutcomeConfigMaster.SchoolId +
                                      " , " + moLearningOutcomeConfigMaster.AcademicYearId +
                                      " , " + moLearningOutcomeConfigMaster.InsertedById +
                                      " , N'" + moLearningOutcomeConfigMaster.InsertDate + "')";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        /// <summary>
        /// This method is used to save the learning outcome submit status.
        /// </summary>
        public void SaveLearningOutcomesSubmitStatus()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moLearningOutcomesSubmitStatus.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moLearningOutcomesSubmitStatus.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moLearningOutcomesSubmitStatus.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", moLearningOutcomesSubmitStatus.SubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmitted", moLearningOutcomesSubmitStatus.IsSubmitted, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardwiseAssessmentId", moLearningOutcomesSubmitStatus.StandardwiseAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Xseed].[usp_InsertLearningOutcomesSubmitStatus]");
            }
        }

        /// <summary>
        /// This method is used to get teacher associated standards.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<StandardMaster> GetTeacherAssociatedStandards(int aiTeacherId, int aiAcademicYearId, int aiSchoolId)
        {
            List<StandardMaster> lstStandardMaster = new List<StandardMaster>();
            string sSelectStatement = "SELECT " +
                                      " Teacher_Standard_Details.Standard_Id" +
                                      " ,Standard_Master.Standard_Name" +
                                      " FROM Teacher_Standard_Details INNER JOIN SchoolWise_Teacher_Master " +
                                      " ON Teacher_Standard_Details.Teacher_Id = SchoolWise_Teacher_Master.Teacher_Id " +
                                      " INNER JOIN Standard_Master ON " + 
                                      " Teacher_Standard_Details.Standard_Id = Standard_Master.Standard_Id " +
                                      " WHERE SchoolWise_Teacher_Master.Teacher_Id = " + aiTeacherId +
                                      " AND SchoolWise_Teacher_Master.academic_year_id = " + aiAcademicYearId +
                                      " AND SchoolWise_Teacher_Master.School_Id = " + aiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            StandardMaster oStandardMaster = new StandardMaster();
                            if (oDR["Standard_Id"] != DBNull.Value)
                                oStandardMaster.StandardId = Convert.ToInt32(oDR["Standard_Id"]);
                            if (oDR["Standard_Name"] != DBNull.Value)
                                oStandardMaster.StandardName = Convert.ToString(oDR["Standard_Name"]);
                            lstStandardMaster.Add(oStandardMaster);
                        }
                    }
                }
            }
            return lstStandardMaster;
        }

        /// <summary>
        /// This method is used to copy learning outcomes.
        /// </summary>
        /// <param name="aiCopyToStandardwiseAssessmentId"></param>
        public void Copy(int aiTargetAssessmentId, int aiTargetSubjectSectionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("CopyToSubjectSectionId", aiTargetSubjectSectionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardwiseAssessmentId", moLearningOutcomeConfigMaster.StandardwiseAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CopyToStandardwiseAssessmentId", aiTargetAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectSectionConfigId", moLearningOutcomeConfigMaster.SubjectSectionConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moLearningOutcomeConfigMaster.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", moLearningOutcomeConfigMaster.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moLearningOutcomeConfigMaster.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Xseed].[Usp_CopyLearningOutcome]");
            }
        }
         
        /// <summary>
        /// This method is used to update the learning outcome details.
        /// </summary>
        public void Update()
        {
            string sUpdateStatement = "UPDATE Xseed.LearningOutcomeConfigMaster SET " +
                                      " StandardwiseAssessmentId= " + moLearningOutcomeConfigMaster.StandardwiseAssessmentId + " " +
                                      ",SubjectSectionConfigId= " + moLearningOutcomeConfigMaster.SubjectSectionConfigId + " " +
                                      ",LearningOutCome= N'" + StringUtility.ReplaceSingleQuoteInString(moLearningOutcomeConfigMaster.LearningOutCome, false) + "' " +
                                      ",IsSubmitted= N'" + moLearningOutcomeConfigMaster.IsConsidered + "'" +
                                      ",SortOrder= " + moLearningOutcomeConfigMaster.SortOrder +
                                      ",UpdatedById= " + moLearningOutcomeConfigMaster.UpdatedById +
                                      ",UpdateDate= N'" + moLearningOutcomeConfigMaster.UpdateDate + "' " +
                                      " WHERE LearningOutcomeConfigId=" + moLearningOutcomeConfigMaster.LearningOutcomeConfigId +
                                      " AND SchoolId = " + moLearningOutcomeConfigMaster.SchoolId +
                                      " AND Academic_Year_Id = " + moLearningOutcomeConfigMaster.AcademicYearId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This function is used to delete the learning outcome details.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        public void Delete(int aiLearningOutcomeConfigId, int aiUserId)
        {
            string sDeleteStatement = "UPDATE Xseed.LearningOutcomeConfigMaster SET Is_Deleted = N'Y' " +
                                       " ,UpdatedById =  " + aiUserId + 
                                       " , UpdateDate = GETDATE() " + 
                                      " WHERE LearningOutcomeConfigId=" + aiLearningOutcomeConfigId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to get learning outcome details.
        /// </summary>
        /// <param name="asSortOrder"></param>
        /// <returns></returns>
        public List<LearningOutcomeConfigMaster> GetAll(string asSortOrder)
        {
            List<LearningOutcomeConfigMaster> lstLearningOutcomeConfigMaster = new List<LearningOutcomeConfigMaster>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moLearningOutcomeConfigMaster.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moLearningOutcomeConfigMaster.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectSectionId", moLearningOutcomeConfigMaster.SubjectSectionConfigId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardwiseAssessmentId", moLearningOutcomeConfigMaster.StandardwiseAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortOrder", asSortOrder, SqlDbType.VarChar);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetLearningOutcomeDetails]"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            LearningOutcomeConfigMaster oLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster();
                            if (oDR["LearningOutcomeConfigId"] != DBNull.Value)
                                oLearningOutcomeConfigMaster.LearningOutcomeConfigId = Convert.ToInt32(oDR["LearningOutcomeConfigId"]);
                            if (oDR["LearningOutCome"] != DBNull.Value)
                                oLearningOutcomeConfigMaster.LearningOutCome = Convert.ToString(oDR["LearningOutCome"]);
                            if (oDR["SortOrder"] != DBNull.Value)
                                oLearningOutcomeConfigMaster.SortOrder = Convert.ToInt32(oDR["SortOrder"]);
                            if (oDR["IsSubmitted"] != DBNull.Value)
                                oLearningOutcomeConfigMaster.IsConsidered = Convert.ToBoolean(oDR["IsSubmitted"]);
                            lstLearningOutcomeConfigMaster.Add(oLearningOutcomeConfigMaster);
                        }

                        if (oDR.NextResult())
                        {
                            while (oDR.Read())
                            {
                                if (oDR["COUNT"] != DBNull.Value)
                                    bIsSubmitted = Convert.ToInt32(oDR["COUNT"]) > 0 ? true : false;
                            }
                        }

                        if (oDR.NextResult())
                        {
                            while (oDR.Read())
                            {
                                if (oDR["GradeSubmitStatus"] != DBNull.Value)
                                    bGradeSubmitStatus = Convert.ToInt32(oDR["GradeSubmitStatus"]) != Constants.I_ZERO ? true : false;
                            }
                        }
                    }
                }
            }
            return lstLearningOutcomeConfigMaster;
        }

        /// <summary>
        ///  This method is used to load learning outcome details.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        public void Load(int aiLearningOutcomeConfigId)
        {
            List<LearningOutcomeConfigMaster> lstLearningOutcomeConfigMaster = new List<LearningOutcomeConfigMaster>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchLearningOutcomeConfigMasterFromDatabase(aiLearningOutcomeConfigId, string.Empty);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["LearningOutcome"] != DBNull.Value)
                                moLearningOutcomeConfigMaster.LearningOutCome = Convert.ToString(oDR["LearningOutcome"]);
                            if (oDR["SortOrder"] != DBNull.Value)
                                moLearningOutcomeConfigMaster.SortOrder = Convert.ToInt32(oDR["SortOrder"]);
                            if (oDR["IsSubmitted"] != DBNull.Value)
                                moLearningOutcomeConfigMaster.IsSubmitted = Convert.ToBoolean(oDR["IsSubmitted"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to check dependency of learning outcome with grade.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <returns></returns>
        public static bool Dependent(int aiLearningOutcomeConfigId, int aiSchoolID, int aiAcademicYearID)
        {

            string sSelect = " select COUNT(*) FROM Xseed.LearningOutcomesGrade " +
                                      " WHERE  LearningOutcomeConfigId = " + aiLearningOutcomeConfigId +
                                      " AND SchoolId = " + aiSchoolID +
                                      " AND Academic_Year_Id = " + aiAcademicYearID +
                                      " AND Is_Deleted=N'" + Constants.C_NO + "'";
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelect);

            if (iCount == 0)
                return false;
            else
                return true;
        }

        #endregion "Public Methods"

        #region "Private Methods"
        /// <summary>
        /// This method is used to fetch the learning outcome details.
        /// </summary>
        /// <param name="aiLearningOutcomeConfigId"></param>
        /// <param name="asSortOrder"></param>
        /// <returns></returns>
        private String FetchLearningOutcomeConfigMasterFromDatabase(int aiLearningOutcomeConfigId, string asSortOrder)
        {
            string sSelectStatement = " SELECT " +
                                      " LearningOutcomeConfigId" +
                                      ", LearningOutCome" +
                                      ", StandardwiseAssessmentId" +
                                      ", SubjectSectionConfigId" +
                                      ", IsSubmitted " +
                                      ", SortOrder" +
                                      " FROM Xseed.LearningOutcomeConfigMaster " +
                                      " WHERE Is_Deleted = N'N' " +
                                      " AND SchoolId = " + moLearningOutcomeConfigMaster.SchoolId +
                                      " AND Academic_Year_Id = " + moLearningOutcomeConfigMaster.AcademicYearId +
                                      " AND StandardwiseAssessmentId = " + moLearningOutcomeConfigMaster.StandardwiseAssessmentId +
                                      " AND SubjectSectionConfigId = " + moLearningOutcomeConfigMaster.SubjectSectionConfigId;

            sSelectStatement += aiLearningOutcomeConfigId != 0 ? " AND LearningOutcomeConfigId=" + aiLearningOutcomeConfigId
                                        : string.Empty;
            if (asSortOrder != string.Empty)
                sSelectStatement += " ORDER BY LearningOutCome " + asSortOrder;
            else
                sSelectStatement += " ORDER BY LearningOutCome";

            return sSelectStatement;
        }

        #endregion "Private Methods"
        
    }
}
