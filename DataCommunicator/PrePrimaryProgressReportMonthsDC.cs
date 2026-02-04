using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using Utility;
using ProgressReportEntities;

namespace DataCommunicator
{

    public class PrePrimaryProgressReportMonthsDC
    {

        #region DataMembers

        public PrePrimaryProgressReportMonth moPrePrimaryProgressReportMonthsEntity = null;
        public List<PrePrimaryConfiguredMonthDetails> olstclsPrePrimaryProgressReportMonths = new List<PrePrimaryConfiguredMonthDetails>();
       
        PrePrimaryConfiguredMonthDetails moPrePrimaryConfiguredMonthDetailsEntity;

        #endregion

        #region Properties

        public virtual PrePrimaryProgressReportMonth PrePrimaryProgressReportMonthsEntity
        {
            get
            {
                return moPrePrimaryProgressReportMonthsEntity;
            }
            set
            {
                moPrePrimaryProgressReportMonthsEntity = value;
            }
        }



        public PrePrimaryConfiguredMonthDetails PrePrimaryConfiguredMonthDetailsEntity
        {
            set { moPrePrimaryConfiguredMonthDetailsEntity = value; }
        }

        #endregion

        #region Overloaded Constructors

        public PrePrimaryProgressReportMonthsDC()
        {

        }
        #endregion

        #region Public Method

        /// <summary>
        /// This function is used to get students list according to the applied filter.
        /// </summary>
        /// <returns></returns>
        public static List<PrePrimaryProgressReportMonth> GetMonthsList(int aiSchoolId, int aiAcademicYearId, int iStandardId)
        {
            List<PrePrimaryProgressReportMonth> olstclsPrePrimaryProgressReportMonths = new List<PrePrimaryProgressReportMonth>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", iStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPrePrimaryProgressReportMonths"))
                {
                    PrePrimaryProgressReportMonth oPrePrimaryProgressReportMonths;
                    while (oSqlDataReader.Read())
                    {
                        oPrePrimaryProgressReportMonths = new PrePrimaryProgressReportMonth
                        {
                            MonthId = Convert.ToInt32(oSqlDataReader["MonthID"]),
                            Month = Convert.ToString(oSqlDataReader["Month"]),
                            PrePrimaryProgressReportMonthId = Convert.ToInt32(oSqlDataReader["PrePrimaryProgressReportMonthId"]),
                            MonthAbbreviation = Convert.ToString(oSqlDataReader["MonthAbbreviation"]),
                            IsCommentable = Convert.ToInt32(oSqlDataReader["IsCommentable"]),
                            CommentAbbreviation = Convert.ToString(oSqlDataReader["CommentAbbreviation"]),
                        };
                        olstclsPrePrimaryProgressReportMonths.Add(oPrePrimaryProgressReportMonths);
                    }
                }
            }
            return olstclsPrePrimaryProgressReportMonths;
        }


        #endregion

        public static void Save(string asMonthXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("MonthXML", asMonthXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdatePrePrimaryProgressReportMonths");
            }
        }

        public static List<PrePrimaryProgressReportMonth> GetSavedMonthsList(int aiSchoolId, int aiAcademicYearId, int iStandaradId)
        {
            List<PrePrimaryProgressReportMonth> olstclsPrePrimaryProgressReportMonths = new List<PrePrimaryProgressReportMonth>();


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", iStandaradId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSavedProgressReportMonths"))
                {
                    PrePrimaryProgressReportMonth oPrePrimaryProgressReportMonths;
                    while (oSqlDataReader.Read())
                    {
                        oPrePrimaryProgressReportMonths = new PrePrimaryProgressReportMonth
                        {
                            MonthId = Convert.ToInt32(oSqlDataReader["MonthID"]),
                            Month = Convert.ToString(oSqlDataReader["Month"]),
                            PrePrimaryProgressReportMonthId = Convert.ToInt32(oSqlDataReader["PrePrimaryProgressReportMonthId"]),
                            MonthAbbreviation = Convert.ToString(oSqlDataReader["MonthAbbreviation"]),
                            SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"])
                        };
                        olstclsPrePrimaryProgressReportMonths.Add(oPrePrimaryProgressReportMonths);
                    }
                }
            }
            return olstclsPrePrimaryProgressReportMonths;
        }

        public void GetClasswiseMonthsList(int aiSchoolId, int aiAcademicYearId, int aiStandDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandDivId", aiStandDivId, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClasswiseProgressReportMonths"))
                SetPrePrimaryConfiguredMonthDetails(oSqlDataReader);
            }
        }

        private void SetPrePrimaryConfiguredMonthDetails(SqlDataReader aoSqlDataReader)
        {
            PrePrimaryConfiguredMonthDetails oPrePrimaryProgressReportMonths;
            while (aoSqlDataReader.Read())
            {
                oPrePrimaryProgressReportMonths = new PrePrimaryConfiguredMonthDetails
                {
                    MonthAbbreviation = Convert.ToString(aoSqlDataReader["MonthAbbreviation"]),
                    PreprimaryExamConfigurationId = Convert.ToInt32(aoSqlDataReader["PreprimaryExamConfigurationId"]),
                    PreprimaryProgressReportMonthID = Convert.ToInt32(aoSqlDataReader["PrePrimaryProgressReportMonthId"]),
                    IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                    IsPublished = Convert.ToBoolean(aoSqlDataReader["IsPublished"]),
                    RollNos = Convert.ToString(aoSqlDataReader["Roll_No"]),
                };
                olstclsPrePrimaryProgressReportMonths.Add(oPrePrimaryProgressReportMonths);
            }
        }

        public static List<PrePrimaryConfiguredMonthDetails> GetStudentWiseMonthsList(int aiSchoolId, int aiAcademicYearId, int aiStandardId,int aiStudentId)
        {
            List<PrePrimaryConfiguredMonthDetails> olstPrePrimaryConfiguredMonthDetails = new List<PrePrimaryConfiguredMonthDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentWiseMonthsList"))
                {
                    PrePrimaryConfiguredMonthDetails oPrePrimaryProgressReportMonths;
                    while (oSqlDataReader.Read())
                    {
                        oPrePrimaryProgressReportMonths = new PrePrimaryConfiguredMonthDetails
                        {
                            MonthAbbreviation = Convert.ToString(oSqlDataReader["MonthAbbreviation"]),
                            PreprimaryProgressReportMonthID = Convert.ToInt32(oSqlDataReader["PrePrimaryProgressReportMonthId"]),
                            IsPublished = Convert.ToString(oSqlDataReader["IsPublished"]) == Constants.S_YES,
                        };
                        olstPrePrimaryConfiguredMonthDetails.Add(oPrePrimaryProgressReportMonths);
                    }
                }
            }
            return olstPrePrimaryConfiguredMonthDetails;
        }


        public static void UpdateSortOrder(string sXmlSortOrder)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SortOrderXML", sXmlSortOrder, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateMonthsSortOrder");
            }
        }

        public static string CheckDependencies(string sSavedMonthsXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("MonthXML", sSavedMonthsXML, SqlDbType.Xml);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Message", "", SqlDbType.NVarChar, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CheckDependancyForPrePrimaryMonth");
                return Convert.ToString(oSqlParameter.Value);
            }
        }

        public static void UpdateStatusClass(int aischoolid, int aiacademicid, string sStatusDetails, bool abIsSubmit)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aischoolid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiacademicid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StatusDetailsXML", sStatusDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("IsSubmit", abIsSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateClassStatus");
            }
        }

        public void UpdateStatusClass()
        {
            int iIsSubmited = moPrePrimaryConfiguredMonthDetailsEntity.IsSubmitted ? 1 : 0;
            int iIsPublished = moPrePrimaryConfiguredMonthDetailsEntity.IsPublished ? 1 : 0;

            string sUpdateStatement = "UPDATE " +
                                      "PreprimaryExamConfiguration" +
                                      " SET " +
                                      "IsSubmitted = " + iIsSubmited +
                                      ",IsPublished = " + iIsPublished +
                                      " WHERE " +
                                      "PreprimaryExamConfigurationId =  " + moPrePrimaryConfiguredMonthDetailsEntity.PreprimaryExamConfigurationId +
                                      " AND SchoolId = " + moPrePrimaryConfiguredMonthDetailsEntity.SchoolId +
                                      " AND AcademicYearId = " + moPrePrimaryConfiguredMonthDetailsEntity.AcademicYearId +
                                      " AND Is_Deleted = 0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void UnpublishExam()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moPrePrimaryConfiguredMonthDetailsEntity.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moPrePrimaryConfiguredMonthDetailsEntity.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PreprimaryExamConfigurationId", moPrePrimaryConfiguredMonthDetailsEntity.PreprimaryExamConfigurationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Reason", moPrePrimaryConfiguredMonthDetailsEntity.UnpublishReason, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", moPrePrimaryConfiguredMonthDetailsEntity.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UnpublishPreprimaryExam");
            }
        }
    }
}
