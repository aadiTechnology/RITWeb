// Class Name       :- ExternalLecturesDC
// Purpose          :- This class is used to external lectures.
// Date Of creation :- 6/23/2011
// Author Name      :- Vipul Jadhav

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Utility;
using ExternalLectures;
using WeekDayNameDetails;

namespace DataCommunicator
{
    public class ExternalLecturesDC
    {
        #region " Constants "

        const string S_DEFAULT_WEEK_DAY_ID = "-999";

        #endregion " Constants "

        #region " Data Members "

        public List<StandardDivisions> mlstStandardDivisions;
        public List<WeekDays> mlstWeekDays;
        public List<StayBackLectureDetails> mlstStayBackLectureDetails;
        public StandardWeekDaywsieStayBackLectureDetails moStandardWeekDaywsieStayBackLectureDetails;

        #endregion " Data Members "

        #region " Public Methods "

        /// <summary>
        /// This method is used to get paged teacher details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="endRowIndex"></param>
        /// <param name="asCriteria"></param>
        /// <returns></returns>
        public List<TeacherExternalLecturesDetails> GetPagedTeacherExternalLectureDetails(int aiSchoolId, int aiAcademicYearId, int aiStartRowIndex, int aiEndRowIndex, string asCriteria)
        {
            List<TeacherExternalLecturesDetails> lstTeacherDetails = new List<TeacherExternalLecturesDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Criteria", StringUtility.ReplaceSingleQuoteInString(asCriteria,true), SqlDbType.NVarChar);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetPagedTeacherDetails]"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                        {
                            TeacherExternalLecturesDetails oTeacherDetails = new TeacherExternalLecturesDetails
                            {
                                TeacherId = Convert.ToInt32(oSqlDataReader["Teacher_Id"]),
                                TeacherName = Convert.ToString(oSqlDataReader["TeacherName"]),
                                IsAssembly = (Convert.ToString(oSqlDataReader["Assembly_Applicable"]) != Constants.C_NO.ToString()) ? true : false,
                                IsMPT = (Convert.ToString(oSqlDataReader["MPT_Applicable"]) != Constants.C_NO.ToString()) ? true : false,
                                IsStayBack = (Convert.ToInt32(oSqlDataReader["Stayback_Applicable"]) != Constants.I_ZERO) ? true : false,
                                WeeklyTestApplicable = (Convert.ToString(oSqlDataReader["WeeklyTestApplicable"]) != Constants.C_NO.ToString()) ? true:false,
                            };
                            lstTeacherDetails.Add(oTeacherDetails);
                        }
                    }
                }
            }
            return lstTeacherDetails;
        }

        /// <summary>
        /// This method is used to get stay back lecture details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetStayBackLectureDetails(int aiSchoolId, int aiAcademicYearId,string asLectureType)
        {
            mlstStandardDivisions = new List<StandardDivisions>();
            mlstWeekDays = new List<WeekDays>();
            mlstStayBackLectureDetails = new List<StayBackLectureDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExternalLectureType", asLectureType, SqlDbType.NVarChar);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetStandardDivisionwiseStayBackLectures]"))
                {
                    if (oSqlDataReader != null)
                    {
                        FillStandardDivisionsList(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillWeekDaysList(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillStayBackLectures(oSqlDataReader);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to get week day name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<WeekdaysName> GetWeedDaysName(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetWeekDaysName"))
                    return this.GetWeekDayName(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get week day name.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<WeekdaysName> GetWeekDayName(SqlDataReader aoSqlDataReader)
        {
            List<WeekdaysName> lstWeekdaysName = new List<WeekdaysName>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    WeekdaysName oWeekdaysName = new WeekdaysName();
                    if (aoSqlDataReader["Original_WeekDays_Id"] != DBNull.Value)
                        oWeekdaysName.Id = Convert.ToInt32(aoSqlDataReader["Original_WeekDays_Id"]);
                    if (aoSqlDataReader["WeekDay_Name"] != DBNull.Value)
                        oWeekdaysName.WeekDayName = aoSqlDataReader["WeekDay_Name"].ToString();
                    lstWeekdaysName.Add(oWeekdaysName);                   
                }
                aoSqlDataReader.Close();
            }
            return lstWeekdaysName;
        }

        /// <summary>
        /// This method is used to fill stay back lecture list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FillStayBackLectures(SqlDataReader oSqlDataReader)
        {
            while (oSqlDataReader.Read())
            {
                StayBackLectureDetails oStayBackLectureDetails = new StayBackLectureDetails
                {
                    LectureNo = Convert.ToInt32(oSqlDataReader["Lecture_Number"]),
                    StandardwiseDivisionId = Convert.ToInt32(oSqlDataReader["SchoolWise_Standard_Division_Id"]),
                    WeekDayId = Convert.ToInt32(oSqlDataReader["WeekDay_Id"]),
                };
                mlstStayBackLectureDetails.Add(oStayBackLectureDetails);
            }
        }

        /// <summary>
        /// This method is used to fill week days list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FillWeekDaysList(SqlDataReader oSqlDataReader)
        {
            while (oSqlDataReader.Read())
            {
                WeekDays oWeekDays = new WeekDays
                {
                    WeekDayId = Convert.ToInt32(oSqlDataReader["WeekDays_Id"]),
                    WeekDay = Convert.ToString(oSqlDataReader["WeekDay_Name"]),
                    IsConfigured = Convert.ToString(oSqlDataReader["WeekDays_Id"]) != S_DEFAULT_WEEK_DAY_ID,
                };
                mlstWeekDays.Add(oWeekDays);
            }
        }

        /// <summary>
        /// This method is used to fill standard division list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FillStandardDivisionsList(SqlDataReader oSqlDataReader)
        {
            while (oSqlDataReader.Read())
            {
                StandardDivisions oStandardDivisions = new StandardDivisions
                {
                    StandardwiseDivisionId = Convert.ToInt32(oSqlDataReader["SchoolWise_Standard_Division_id"]),
                    StandardDivision = Convert.ToString(oSqlDataReader["StandardDivision"]),
                    StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]),
                };
                mlstStandardDivisions.Add(oStandardDivisions);
            }
        }

        /// <summary>
        /// This method is used to get teacher count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asCriteria"></param>
        /// <returns></returns>
        public int CountPagedTeacherExternalLectureDetails(int aiSchoolId, int aiAcademicYearId, string asCriteria)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Criteria", StringUtility.ReplaceSingleQuoteInString(asCriteria,true), SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_CountTeacherDetails]");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to save external lecture details.
        /// </summary>
        /// <param name="asXmlExternalLectureDetails"></param>
        public void SaveTeacherExternalLectureDetails(string asXmlExternalLectureDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("XML", asXmlExternalLectureDetails, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_SaveTeacherExternalLectureDetails]");
            }
        }

        /// <summary>
        /// This method is used to save stay back lecture details.
        /// </summary>
        /// <param name="asXmlStayBackLectureDetails"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asWeekDay"></param>
        /// <param name="aiStandardDivsionId"></param>
        public void SaveStayBackLectureDetails(string asXmlStayBackLectureDetails, int aiSchoolId, int aiAcademicYearId, int aiUserId, string asWeekDay, int aiStandardDivsionId,string asLectureType)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("XML", asXmlStayBackLectureDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("StandardDivsionId", aiStandardDivsionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekDay", asWeekDay, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LectureType", asLectureType, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_SaveStandardDivisionwiseStayBackLectures]");
            }
        }

        /// <summary>
        /// This method is used to get standard week daywise stay back lecture details.
        /// </summary>
        /// <param name="aiStandardDivisonId"></param>
        /// <param name="aiWeekDayId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void GetStandardWeekDaywiseStayBackLectureDetails(int aiStandardDivisonId, int aiWeekDayId, int aiSchoolId, int aiAcademicYearId,string asLectureType)
        {
            List<int> lstLectureNo = new List<int>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisonId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekDaysId", aiWeekDayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LectureType", asLectureType, SqlDbType.NVarChar);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetStandardWeekDaywiseStayBackLectures]"))
                {
                    if (oSqlDataReader != null)
                    {
                        FiilStandardWeekDaywsieStayBackLectureDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FiilStayBackLectureDetailsList(oSqlDataReader);
                    }
                }
            }
        }

        public List<StayBackLectureDetails> GetStayBackLecturesForStandardsAssociatedToTeachers(int aiTeacherId, string asWeekDay, int aiSchoolId, int aiAcademicYearId, string asLectureType)
        {
            List<StayBackLectureDetails> lstStayBackLectureDetails = new List<StayBackLectureDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WeekDay", asWeekDay, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LectureType", asLectureType, SqlDbType.NChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetStayBackLecturesForStandardsAssociatedToTeachers]"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                        {
                            StayBackLectureDetails oStayBackLectureDetails = new StayBackLectureDetails
                            {
                                LectureNo = Convert.ToInt32(oSqlDataReader["Lecture_Number"]),
                                WeekDay = Convert.ToString(oSqlDataReader["WeekDay_Name"]),
                                StandardwiseDivisionId = Convert.ToInt32(oSqlDataReader["Standard_Division_Id"]),
                            };
                            lstStayBackLectureDetails.Add(oStayBackLectureDetails);
                        }
                    }
                }
            }
            return lstStayBackLectureDetails;
        }

        /// <summary>
        /// This method is used to fill stay back lectures list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FiilStayBackLectureDetailsList(SqlDataReader oSqlDataReader)
        {
            mlstStayBackLectureDetails = new List<StayBackLectureDetails>();
            while (oSqlDataReader.Read())
            {
                StayBackLectureDetails oStayBackLectureDetails = new StayBackLectureDetails
                {
                    LectureNo = Convert.ToInt32(oSqlDataReader["Lecture_Number"]),
                    StayBackDetailsId = Convert.ToInt32(oSqlDataReader["StaybackDetail_Id"]),
                };
                mlstStayBackLectureDetails.Add(oStayBackLectureDetails);
            }
        }

        /// <summary>
        /// This method is used to fill standard and week daywise stay back lectures.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FiilStandardWeekDaywsieStayBackLectureDetails(SqlDataReader oSqlDataReader)
        {
            while (oSqlDataReader.Read())
            {
                moStandardWeekDaywsieStayBackLectureDetails = new StandardWeekDaywsieStayBackLectureDetails();
                moStandardWeekDaywsieStayBackLectureDetails.StandardName = Convert.ToString(oSqlDataReader["Standard_Name"]);
                moStandardWeekDaywsieStayBackLectureDetails.DivisionName = Convert.ToString(oSqlDataReader["Division_Name"]);
                moStandardWeekDaywsieStayBackLectureDetails.WeekDay = Convert.ToString(oSqlDataReader["WeekDay_Name"]);
                moStandardWeekDaywsieStayBackLectureDetails.MaxNoOfLecturesForStandard = Convert.ToInt32(oSqlDataReader["Max_lectures_per_standard"]);
                moStandardWeekDaywsieStayBackLectureDetails.WeekdayShortName = Convert.ToString(oSqlDataReader["WeekDayShortName"]);
            }
        }

        #endregion " Public Methods "
    }
}
