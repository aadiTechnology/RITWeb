// File Name    : SchoolwiseAttendanceDetailsDC.cs
// Created By   : Ketan
// Crested Date : 6/12/2007  

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using System.Collections;
using SchoolEntities.Admin;
using SchoolEntities.Dashboard;
using System.Linq;
using SchoolEntities;
using SchoolEntities.Teacher;
namespace DataCommunicator
{
    /// <summary>
    /// This class is used to handle all the database related operations on SchoolWise_Attendance_Details. 
    /// </summary>
    //public class SchoolWiseAttendanceDetailsDC : DataCommunicatorBaseDC
    public class AttendanceDetailsDC : DataCommunicatorBaseDC
    {
        #region structure

        public struct SchoolWiseAttendanceDetailsStruct
        {
            public int miSchoolWiseAttendanceId;
            public int miSchoolId;
            public DateTime mdtAttendanceDate;
            public int miStudentId;
            public string msIsPresent;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public int miStandardDivisionId;
            public int miAcademicYearId;
        }

        #endregion

        #region Data members

        private SchoolWiseAttendanceDetailsStruct moSchoolWiseAttendanceDetailsStruct;
        private DayDetails moDayDetails;

        #endregion

        #region Properties

        public SchoolWiseAttendanceDetailsStruct SchoolWiseAttendanceDetailsStructDetails
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct = value;
            }
        }

        public DayDetails DayDetails
        {
            get
            {
                return moDayDetails;
            }
        }
        #endregion

        #region Constructors

        public AttendanceDetailsDC()
        {
        }
        public AttendanceDetailsDC(int aiId)
        {
            LoadSchoolWiseAttendanceDetailsDetails(aiId);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiId"></param>
        public void LoadSchoolWiseAttendanceDetailsDetails(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolWiseAttendanceDetailsDataFromDatabase(aiId);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {

                            if (oDR["SchoolWise_Attendance_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miSchoolWiseAttendanceId = Convert.ToInt32(oDR["SchoolWise_Attendance_Id"].ToString());
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                            if (oDR["Attendance_Date"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate = Convert.ToDateTime(oDR["Attendance_Date"].ToString());
                            if (oDR["Student_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miStudentId = Convert.ToInt32(oDR["Student_Id"].ToString());
                            if (oDR["Is_Present"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.msIsPresent = oDR["Is_Present"].ToString();
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"].ToString());
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolWiseAttendanceDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public string FetchSchoolWiseAttendanceDetailsDataFromDatabase(int aiId)
        {
            string sSelectStatement = " SELECT  " +
                "schoolwise_attendance_id" +
                " , school_id" +
                " , attendance_date" +
                " , student_id" +
                " , is_present" +
                " , is_deleted" +
                " , insert_date" +
                " , inserted_by_id" +
                " , update_date" +
                " , updated_by_id" +

            " FROM  " +
                "SchoolWise_Attendance_Details " +
            " WHERE " +
                 " schoolwise_attendance_id = " + aiId +
                 " AND is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="adtTodaysDate"></param>
        /// <returns></returns>
        public DataSet FetchAttendenceDetails(int aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardDivisionId, DateTime adtTodaysDate, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("dateOfAttendence", adtTodaysDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAttendance");
            }
        }

        /// <summary>
        /// This method is used to fetch attendance of student.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet FetchStudentAttendanceForCalender(Int32 aiSchoolID, Int32 aiStudentID,
                                    Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionid, Int32 aiMonthId, Int32 aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_id", aiDivisionid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_MusterReport");
            }
        }

        public DataTable GetTeachersForLecturewiseAttendance(int aiSchoolId, int aiAcademicYearId, int aiLoginUSerId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Login_User_Id", aiLoginUSerId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTeachersForLecturewiseAttendance");
            }
        }
        
        public DataSet FetchStudentMonthlyAttendance(int aiSchoolID, int aiStudentID, int aiAcademicYearId, int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStudentMonthlyAttendace");
            }
        }


        /// <summary>
        /// This method is used to check for non working day.
        /// </summary>
        /// <param name="adtTodaysDate"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet CheckTodaysDay(DateTime adtTodaysDate, Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("dDate", adtTodaysDate, SqlDbType.DateTime);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_CheckIsDayValid");
            }
        }



        /// <summary>
        /// This method is used to check for non working day.
        /// </summary>
        /// <param name="adtTodaysDate"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public Boolean CheckWeekEnd(DateTime adtTodaysDate, Int32 aiSchoolId, Int32 aiAcademicYearId)
        {
            string sSelectStatement = " SELECT TOP 1 WeekDay_Name" +
                                      " FROM WeekDays_Master " +
                                      " WHERE WeekDay_Name = " +
                                             " DATENAME(weekday,'" + adtTodaysDate + "') " +
                                             " AND School_Id= " + aiSchoolId +
                                             " AND is_deleted = N'" + Constants.C_NO + "'" +
                                             " AND Academic_Year_Id= " + aiAcademicYearId;
            DataTable oDataTable;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oDataTable = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
            if ((oDataTable != null) && (oDataTable.Rows.Count > 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method is used to insert student attendance.
        /// </summary>
        /// <returns></returns>
        public string InsertSchoolWiseAttendanceDetails()
        {

            string sInsertStatement = "INSERT INTO SchoolWise_Attendance_Details ( " +
                "  school_id" +
                " , attendance_date" +
                " , student_id" +
                " , is_present" +
                " , Standard_Division_Id" +
                " , Academic_Year_Id" +
             ") VALUES (" +
                  moSchoolWiseAttendanceDetailsStruct.miSchoolId +
                 " , N'" + moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate + "' " +
                 " , " + moSchoolWiseAttendanceDetailsStruct.miStudentId +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAttendanceDetailsStruct.msIsPresent, false) + "' " +
                 " , " + moSchoolWiseAttendanceDetailsStruct.miStandardDivisionId + " " +
                 " , " + moSchoolWiseAttendanceDetailsStruct.miAcademicYearId + " " +
            " ) ";

            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to update student attendance.
        /// </summary>
        /// <returns></returns>
        public string UpdateSchoolWiseAttendanceDetails()
        {
            string sUpdateStatement = " UPDATE SchoolWise_Attendance_Details SET " +
                "  is_present =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAttendanceDetailsStruct.msIsPresent, false) + "' " +
             " WHERE " +
                " is_deleted =  N'" + Constants.C_NO + "'" +
                //" AND schoolwise_attendance_id =  " + moSchoolWiseAttendanceDetailsStruct.miSchoolWiseAttendanceId +
                " AND SchoolWise_Attendance_Id = " + moSchoolWiseAttendanceDetailsStruct.miSchoolWiseAttendanceId +
                "AND Student_Id =" + moSchoolWiseAttendanceDetailsStruct.miStudentId;

            return sUpdateStatement;
            // using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            //   oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public static void DeleteSchoolWiseAttendanceDetails(int aiSchoolId, int aiAcademicYearId, string asAttendanceDate, int aiStdDivId)
        {
            string sDeleteStatement = " DELETE FROM SchoolWise_Attendance_Details " +
                      " WHERE " +
                      " School_Id = " + aiSchoolId +
                      " AND Attendance_Date = N'" + asAttendanceDate + "' " +
                      " AND Academic_Year_Id = " + aiAcademicYearId +
                      " AND Standard_Division_Id = " + aiStdDivId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to check if attendance is marked before the given date or not.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool CheckIfAttendanceMarked(DateTime dateTime, int iStandardId, int iDivisionId)
        {
            string sSelectStatement = " SELECT     COUNT(*) AS AttenCount" +
                                    " FROM SchoolWise_Attendance_Details INNER JOIN " +
                                    " SchoolWise_Standard_Division_Master ON " +
                                    " SchoolWise_Attendance_Details.Standard_Division_Id = " +
                                    " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id AND " +
                                    " SchoolWise_Attendance_Details.School_Id = SchoolWise_Standard_Division_Master.School_Id " +
                                    " AND SchoolWise_Attendance_Details.Academic_Year_Id = " +
                                    " SchoolWise_Standard_Division_Master.academic_year_id " +
                                    " WHERE     (SchoolWise_Attendance_Details.Attendance_Date <= '" + dateTime.ToString("yyyy-MM-dd") + "') " +
                                    " AND (SchoolWise_Standard_Division_Master.Division_Id = " + iDivisionId + ") " +
                                    " AND (SchoolWise_Standard_Division_Master.Standard_Id = " + iStandardId + ")";

            int iAttendCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iAttendCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iAttendCount > 0)
                return true;

            return false;
        }

        /// <summary>
        /// This method is used to mark the student's attendance.
        /// </summary>
        /// <param name="sAttendanceXML"></param>
        public void MarkStudentAttendence(string sAttendanceXML, bool abSendMessage)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moSchoolWiseAttendanceDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentsAttendance", sAttendanceXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Attendance_Date", moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", moSchoolWiseAttendanceDetailsStruct.miStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SendMessage", (abSendMessage ? Constants.S_YES : Constants.S_NO), SqlDbType.Char);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkStudentsAttendance");
            }
        }

        public void MarkStudentMonthlyAttendence(string sAttendanceXML, int aiYear, int aiMonth)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moSchoolWiseAttendanceDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentsAttendance", sAttendanceXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Student_Id", moSchoolWiseAttendanceDetailsStruct.miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonth, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkStudentMonthlyAttendace");
            }
        }

        /// <summary>
        /// This method is used to get the attendance details for attendance status
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aSelectedDate"></param>
        /// <returns></returns>
        public List<ClasswiseAttendanceStatus> Get(int aiSchoolId, int aiAcademicYearId, string aSelectedDate)
        {
            List<ClasswiseAttendanceStatus> olstAttendanceDetails = new List<ClasswiseAttendanceStatus>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SelectedDate", aSelectedDate, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardDivisionAttendanceDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        if (oSqlDataReader.Read())
                        {
                            moDayDetails = new DayDetails
                            {
                                HolidayName = oSqlDataReader["Holiday_Name"].ToString(),
                                IsWeekDay = oSqlDataReader["Is_Weekend"].ToString(),
                                OutSideAcademicYear = oSqlDataReader["OutSide_AcademicYear"].ToString()
                            };
                        }
                        if (oSqlDataReader.NextResult())
                        {
                            while (oSqlDataReader.Read())
                            {
                                ClasswiseAttendanceStatus oAttendanceDetais = new ClasswiseAttendanceStatus
                                                                         {
                                                                             SchoolWiseStandardDivisionId = oSqlDataReader["SchoolWiseStandardDivisionId"].ToInt(),
                                                                             StandardId = oSqlDataReader["StandardId"].ToInt(),
                                                                             StandardName = oSqlDataReader["StandardName"].ToString(),
                                                                             DivisionId = oSqlDataReader["DivisionId"].ToInt(),
                                                                             DivisionName = oSqlDataReader["DivisionName"].ToString(),
                                                                             AttendanceTaken = Convert.ToInt16(oSqlDataReader["AttendanceTaken"]),
                                                                             HolidayName = oSqlDataReader["AssociatedHoliday"].ToString(),
                                                                             PresentStudentWithTotal = oSqlDataReader["PresentStudentWithTotal"].ToString()
                                                                         };
                                olstAttendanceDetails.Add(oAttendanceDetais);
                            }
                        }
                    }
                    return olstAttendanceDetails;
                }
            }
        }

        /// <summary>
        /// this method is used to get attendance related data to show on attendance widget data.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asDate"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static AttendanceSummary GetAttendanceSummary(int aiSchoolId, int aiAcademicYearId, string asDate, int aiUserId, bool abIsServiceCall = false)
        {
            AttendanceSummary oAttendanceSummary = new AttendanceSummary();
            List<MissingAttendance> lstMissingAttendanceClasses = new List<MissingAttendance>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId, aiUserId, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DateOfAttendance", asDate, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_AttendanceSummaryDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oAttendanceSummary.TotalClasses = Convert.ToInt32(oSqlDataReader["TotalDivisionsCount"]);
                        oAttendanceSummary.TotalStudent = Convert.ToInt32(oSqlDataReader["TotalStudentsCount"]);
                        oAttendanceSummary.AttendanceMarkedClassCount = Convert.ToInt32(oSqlDataReader["ClassAttendanceCount"]);
                        oAttendanceSummary.AttendanceMarkedStudentCount = Convert.ToInt32(oSqlDataReader["StudentAttendanceCount"]);
                        oAttendanceSummary.Students = Convert.ToString(oSqlDataReader["Students"]);
                        oAttendanceSummary.Classes = Convert.ToString(oSqlDataReader["Classes"]);
                    }

                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            oAttendanceSummary.MissingAttendance.Add(new MissingAttendance()
                            {
                                ClassNames = Convert.ToString(oSqlDataReader["ClassName"]),
                                MissingPercentage = Convert.ToInt32(oSqlDataReader["MissingPercentage"])
                            });
                        }
                    }
                }
            }

            return oAttendanceSummary;

        }

        /// <summary>
        /// This method is used for to set Static details to Attendance Summery widgets on dashboards for teacher.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="abIsServiceCall"></param>
        /// <returns></returns>
        public static ClasswiseAttendanceSummary GetClasswiseAttendanceSummary(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiStandardId, bool abIsServiceCall = false)
        {
            List<string> lstAttendanceDays = new List<string>();
            //This is the test data for upcoming widget
            int[] iStudentCount = new int[] { 40, 32, 34, 36, 30, 33, 34, 38, 36, 23, 40, 32, 34, 36, 30, 33, 34, 38, 36, 23, 40, 32, 34, 36, 30, 33, 34, 38, 36, 23 };
            DateTime day = DateTime.Now.AddDays(-30);

            for (var iIndex = 1; iIndex <= 30; iIndex++)
            {
                lstAttendanceDays.Add(day.ToString(Constants.S_DATE_FORMAT_DD_MMM));
                day = day.AddDays(1);
            }

            ClasswiseAttendanceSummary oClasswiseAttendanceSummary = new ClasswiseAttendanceSummary()
            {
                AttendanceDays = lstAttendanceDays.ToArray(),
                StudentCount = iStudentCount,
                MaxCountOfStudent = iStudentCount.Max() + 10,
            };

            // returen test data for attendance summary
            return oClasswiseAttendanceSummary;
        }

        /// <summary>
        /// This method is used to get absent student ids.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="adSelectDate"></param>
        /// <param name="aiMaxAbsentDyasLimit"></param>
        /// <returns></returns>
        public List<int> GetAbsentStudentIds(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, DateTime adSelectDate, int aiMaxAbsentDyasLimit,out List<int> aolstHalfDayPresentStudentId)
        {
            List<int> olstInt = new List<int>();
            List<int> olstHalfDayAttendanceStudentId = new List<int>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearid", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SelectDate", adSelectDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("MaxDaysLimit", aiMaxAbsentDyasLimit, SqlDbType.Int);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAbsentStudentId"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["StudentId"] != DBNull.Value)
                                olstInt.Add(Convert.ToInt32(oDR["StudentId"]));
                        }
                        oDR.NextResult();
                        while (oDR.Read())
                        {
                            if (oDR["Student_Id"] != DBNull.Value)
                                olstHalfDayAttendanceStudentId.Add(Convert.ToInt32(oDR["Student_Id"]));
                        }
                    }
                }
            }
            aolstHalfDayPresentStudentId = olstHalfDayAttendanceStudentId;
            return olstInt;
        }

        /// <summary>
        /// This method is used to get absent student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aistudentIds"></param>
        /// <returns></returns>
        public List<AttendanceDetails> GetAbsentStudentDetails(int aiSchoolId, int aiAcademicYearId, string aistudentIds, int iStandardDivisionId, DateTime adtSelectedDate, string asHalfDayAttendanceStudentIds, out List<AttendanceDetails> lstHalfDayStudentAttendanceDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentIds", aistudentIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardDivId", iStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SelectedDate", adtSelectedDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("HalfDayAttendanceStudentIds", asHalfDayAttendanceStudentIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAbsentStudentDetails"))
                    return this.ReadAbsentStudentDetails(oSqlDataReader, out lstHalfDayStudentAttendanceDetails);
            }
        }

        /// <summary>
        /// This method is used to read absent student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<AttendanceDetails> ReadAbsentStudentDetails(SqlDataReader aoSqlDataReader , out List<AttendanceDetails> lstHalfDayStudentAttendanceDetails)
        {
            List<AttendanceDetails> lstAttendanceDetails = new List<AttendanceDetails>();
            List<AttendanceDetails> lstHalfDayAttendanceDetails = new List<AttendanceDetails>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    AttendanceDetails oAttendanceDetails = new AttendanceDetails();
                    if (aoSqlDataReader["StudentName"] != DBNull.Value && aoSqlDataReader["StudentName"].ToString() != string.Empty)
                        oAttendanceDetails.StudentName = aoSqlDataReader["StudentName"].ToString();
                    if (aoSqlDataReader["Mobile_Number"] != DBNull.Value && aoSqlDataReader["Mobile_Number"].ToString() != string.Empty)
                        oAttendanceDetails.Mobile_Number = Convert.ToDecimal(aoSqlDataReader["Mobile_Number"]);
                    if (aoSqlDataReader["Mobile_Number2"] != DBNull.Value && aoSqlDataReader["Mobile_Number2"].ToString() != string.Empty)
                        oAttendanceDetails.Mobile_Number2 = Convert.ToDecimal(aoSqlDataReader["Mobile_Number2"]);
                    if (aoSqlDataReader["User_Id"] != DBNull.Value && aoSqlDataReader["User_Id"].ToString() != string.Empty)
                        oAttendanceDetails.User_Id = Convert.ToInt32(aoSqlDataReader["User_Id"]);
                    if (aoSqlDataReader["FromAbsentDate"] != DBNull.Value && aoSqlDataReader["FromAbsentDate"].ToString() != string.Empty)
                        oAttendanceDetails.FromAbsentDate = Convert.ToDateTime(aoSqlDataReader["FromAbsentDate"]);
                    lstAttendanceDetails.Add(oAttendanceDetails);
                }
                aoSqlDataReader.NextResult();
                while (aoSqlDataReader.Read())
                {
                    AttendanceDetails oAttendanceDetails = new AttendanceDetails();
                    if (aoSqlDataReader["StudentName"] != DBNull.Value && aoSqlDataReader["StudentName"].ToString() != string.Empty)
                        oAttendanceDetails.StudentName = aoSqlDataReader["StudentName"].ToString();
                    if (aoSqlDataReader["Mobile_Number"] != DBNull.Value && aoSqlDataReader["Mobile_Number"].ToString() != string.Empty)
                        oAttendanceDetails.Mobile_Number = Convert.ToDecimal(aoSqlDataReader["Mobile_Number"]);
                    if (aoSqlDataReader["Mobile_Number2"] != DBNull.Value && aoSqlDataReader["Mobile_Number2"].ToString() != string.Empty)
                        oAttendanceDetails.Mobile_Number2 = Convert.ToDecimal(aoSqlDataReader["Mobile_Number2"]);
                    if (aoSqlDataReader["User_Id"] != DBNull.Value && aoSqlDataReader["User_Id"].ToString() != string.Empty)
                        oAttendanceDetails.User_Id = Convert.ToInt32(aoSqlDataReader["User_Id"]);
                    if (aoSqlDataReader["FromAbsentDate"] != DBNull.Value && aoSqlDataReader["FromAbsentDate"].ToString() != string.Empty)
                        oAttendanceDetails.FromAbsentDate = Convert.ToDateTime(aoSqlDataReader["FromAbsentDate"]);
                    lstHalfDayAttendanceDetails.Add(oAttendanceDetails);
                }
                aoSqlDataReader.Close();
            }
            lstHalfDayStudentAttendanceDetails = lstHalfDayAttendanceDetails;
            return lstAttendanceDetails;
        }

        /// <summary>
        /// This method is used to get class name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public string GetClassName(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sClassName = string.Empty;
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClassName");
                if (oSqlDataReader != null)
                {
                    if (oSqlDataReader.Read())
                    {

                        if (oSqlDataReader["className"] != DBNull.Value)
                            sClassName = oSqlDataReader["className"].ToString();
                    }
                    oSqlDataReader.Close();
                }
                return sClassName;
            }
        }

        /// <summary>
        /// This Method is used to mark monthwise attendance for all classes one time.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="dtAttendanceDate"></param>
        /// <param name="aiUpdatedById"></param>
        public void MarkClassMothwiseAttendance(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, DateTime dtAttendanceDate, int aiUpdatedById, bool abOverriteAttendance)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AttendanceDate", dtAttendanceDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsoverwriteAttendance", abOverriteAttendance, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkMonthwiseStudentAttendance");
            }
        }

      public List<CoordinateDetails> GetCoordinatorDetails(int aiSchoolId, int aiAcademicYearId)
      {
          List<CoordinateDetails> lstCoordinateDetails = new List<CoordinateDetails>();
          using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
          {
              oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
              oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
              using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetCoordinatorDetials"))
              {
                  while (oSqlDataReader.Read())
                  {
                      lstCoordinateDetails.Add(new CoordinateDetails
                      {
                          StandardId = Convert.ToInt32(oSqlDataReader["StandardId"]),
                          UserId = Convert.ToInt32(oSqlDataReader["UserId"]),


                      });
                  }
                  return lstCoordinateDetails;
              }

          }
      }
    }
        #endregion
    /// <summary>
    /// This class is used to execute attendance configuration transaction on SchoolWise_Attendance_Details table.
    /// </summary>
    public class StudentAttendanceCollectionDC
    {
        #region PublicMethod
        /// <summary>
        /// This method update all Attendance Configuration into SchoolWise_Attendance_Details table.
        /// </summary>
        /// <param name="aoArrayListWeekDays"></param>
        public void UpdateAttendanceConfiguration(StringBuilder aoSB)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(aoSB.ToString());
        }

        public static void MarkAttendanceForTestDdate(int aiSchoolId, int aiAcademicYearId, int aiStandatdDivisionId, int aiTestId, int aiSubjectId, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandatdDivisionId", aiStandatdDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkAttendanceForTestDate");
            }
        }
        #endregion
    }
}
