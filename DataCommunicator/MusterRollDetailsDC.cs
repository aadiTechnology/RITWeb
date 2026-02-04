using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.MusterRollDetails;
using Utility;

namespace DataCommunicator.MusterRollDetails
{     
    public class MusterRollDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private List<StudentDetails> mlstStudentDetails;
        private List<HolidayDetails> mlstHolidayDetails;
        private List<AttendanceSummaryDetails> mlstAttendanceSummaryDetails;
        private SchoolDetails moSchoolDetails;
        private List<int> mlstWeekends;
        private List<GenderwiseAttendanceSummary> mlstGenderwiseAttendanceSummary; 

        #endregion

        #region Constructor(s)
        
        public MusterRollDetailsDC()
        {
        }

        public MusterRollDetailsDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        } 

        #endregion

        #region Property(s)

        public List<StudentDetails> StudentDetails
        {
            get { return this.mlstStudentDetails; }
        }

        public List<HolidayDetails> HolidayDetails
        {
            get { return this.mlstHolidayDetails; }
        }

        public List<AttendanceSummaryDetails> AttendanceSummaryDetails
        {
            get { return this.mlstAttendanceSummaryDetails; }
        }

        public SchoolDetails SchoolDetails
        {
            get { return this.moSchoolDetails; }
        }

        public List<int> Weekends
        {
            get { return this.mlstWeekends; }
        }

        public List<GenderwiseAttendanceSummary> GenderwiseAttendanceSummary
        {
            get { return this.mlstGenderwiseAttendanceSummary; }
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return muster roll report related details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public List<AttendanceDetails> GetAttendanceDetailsForMusterRoll(int aiStandardId, int aiDivisionId, int aiYear, int aiMonthId)
        {
            List<AttendanceDetails> lstAttendanceDetails;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonthId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAttendanceDetailsforMusterRoll"))
                {
                    lstAttendanceDetails = LoadAttendanceDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadStudentDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadHolidayDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadAttendanceSummary(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadWeekendDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadGenderwiseAttendanceSummary(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadSchoolDetails(oSqlDataReader);
                }
            }
            return lstAttendanceDetails;
        } 

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to load genderwise attendance summary details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadGenderwiseAttendanceSummary(SqlDataReader aoSqlDataReader)
        {
            this.mlstGenderwiseAttendanceSummary = new List<GenderwiseAttendanceSummary>();
            while (aoSqlDataReader.Read())
            {
                mlstGenderwiseAttendanceSummary.Add(
                    new GenderwiseAttendanceSummary
                    {
                        CategoryId = aoSqlDataReader["CategoryId"].ToInt(),
                        IsPresent = (aoSqlDataReader["Is_Present"].ToString() == Constants.S_YES ? true : false),
                        Sex = Convert.ToChar(aoSqlDataReader["Sex"]),
                        TotalCount = aoSqlDataReader["TotalCount"].ToInt()
                    }
                    );
            }
        }

        /// <summary>
        /// This method is used to load weekend details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadWeekendDetails(SqlDataReader aoSqlDataReader)
        {
            mlstWeekends = new List<int>();
            while (aoSqlDataReader.Read())
            {
                mlstWeekends.Add(aoSqlDataReader["Original_WeekDays_Id"].ToInt());
            }
        }

        /// <summary>
        /// This method is used to load school details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadSchoolDetails(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                moSchoolDetails = new SchoolDetails
                {
                    EndDate = aoSqlDataReader["EndDate"].ToDateTime(),
                    StartDate = aoSqlDataReader["Startdate"].ToDateTime(),
                    OrgName = aoSqlDataReader["School_Orgn_Name"].ToString(),
                    SchoolName = aoSqlDataReader["School_Name"].ToString(),
                    AcademicYear = aoSqlDataReader["AcademicYear"].ToString()
                };
            }
        }

        /// <summary>
        /// This method is used to load attendance summary details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadAttendanceSummary(SqlDataReader aoSqlDataReader)
        {
            mlstAttendanceSummaryDetails = new List<AttendanceSummaryDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstAttendanceSummaryDetails.Add(
                    new AttendanceSummaryDetails
                    {
                        CurrentMonthCount = aoSqlDataReader["CurrentMonthCount"].ToInt(),
                        LastMonthCount = aoSqlDataReader["LastMonthCount"].ToInt(),
                        StudentId = aoSqlDataReader["Student_Id"].ToInt(),
                        TotalCount = aoSqlDataReader["TotalCount"].ToInt(),
                        TotalPercentage = aoSqlDataReader["TotalPercentage"].ToDecimal()
                    }
                    );
            }
        }

        /// <summary>
        /// This method is used to load holiday details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadHolidayDetails(SqlDataReader aoSqlDataReader)
        {
            mlstHolidayDetails = new List<HolidayDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstHolidayDetails.Add(
                    new HolidayDetails
                    {
                        StartDate = aoSqlDataReader["Holiday_Start_Date"].ToDateTime(),
                        EndDate = aoSqlDataReader["Holiday_End_Date"].ToDateTime(),
                        HolidayId = aoSqlDataReader["Holiday_Id"].ToInt()
                    }
                    );
            }
        }

        /// <summary>
        /// This method is used to load student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadStudentDetails(SqlDataReader aoSqlDataReader)
        {
            mlstStudentDetails = new List<StudentDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstStudentDetails.Add(
                    new StudentDetails
                    {
                        StudentId = aoSqlDataReader["YearWise_Student_Id"].ToInt(),
                        DOB = aoSqlDataReader["DOB"].ToDateTime(),
                        EnrolmentNumber = aoSqlDataReader["Enrolment_Number"].ToString(),
                        RollNo = aoSqlDataReader["Roll_No"].ToInt(),
                        SchoolLeftDate = (aoSqlDataReader["SchoolLeft_Date"] == DBNull.Value ? DateTime.MinValue : aoSqlDataReader["SchoolLeft_Date"].ToDateTime()),
                        StudentName = aoSqlDataReader["StudentName"].ToString(),
                        Sex = Convert.ToChar(aoSqlDataReader["Sex"]),
                        JoiningDate = aoSqlDataReader["Joining_Date"].ToDateTime()
                    }
                    );
            }
        }

        /// <summary>
        /// This method is used to load attendance details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<AttendanceDetails> LoadAttendanceDetails(SqlDataReader aoSqlDataReader)
        {
            List<AttendanceDetails> lstAttendanceDetails = new List<AttendanceDetails>();
            while (aoSqlDataReader.Read())
            {
                lstAttendanceDetails.Add(
                    new AttendanceDetails
                    {
                        AttendanceDate = aoSqlDataReader["Attendance_Date"].ToDateTime(),
                        IsHalfDayPresent = (aoSqlDataReader["Is_HalfDayPresent"].ToString() == Constants.S_YES ? true : false),
                        IsPresent = (aoSqlDataReader["Is_Present"].ToString() == Constants.S_YES ? true : false),
                        StudentId = aoSqlDataReader["Student_Id"].ToInt()
                    }
                    );
            }
            return lstAttendanceDetails;
        } 

        #endregion
    }
}
