// Class Name       :- StaffLeaveDetailsDC
// Purpose          :- This class is used to manage StaffLeaveDetails details.
// Date Of creation :- 15-1-2010
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PayrollEntities;
using System.Data;
using Utility;

namespace DataCommunicator
{
    public class StaffLeaveDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;

        private List<ConfiguredLeaves> mlstConfiguredLeaves = new List<ConfiguredLeaves>();
        private List<UserLateMarkLeave> mlstUserLateMarkLeaves = new List<UserLateMarkLeave>();
        private List<LateMarkConfiguration> mlstLateMarkConfigurations = new List<LateMarkConfiguration>();
        private List<StaffLeaveDetails> mlstStaffLeaveDetails = new List<StaffLeaveDetails>();
        private List<UsersSalaryDeduction> mlstUsersSalaryDeductions = new List<UsersSalaryDeduction>();
        private List<int> mlstWeekends = new List<int>();
        private List<DaywiseStaffAttendance> mlstDaywiseStaffAttendance = new List<DaywiseStaffAttendance>();
        private List<HolidayMaster> mlstHolidays = new List<HolidayMaster>();
        private List<DatewiseStaffLeave> mlstDatewiseStaffLeaves = new List<DatewiseStaffLeave>();

        private List<UserBasicDetails> mlstUserDetails = new List<UserBasicDetails>();
        private List<LeaveBalanceDetails> mlstLeaveBalanceDetails = new List<LeaveBalanceDetails>();
        private List<UserLateMarkLeave> mlstUserLateMarks = new List<UserLateMarkLeave>();
        private List<LeaveYear> mlstLeaveYears = new List<LeaveYear>();
        private string msSchoolName;
        private bool mbIsAttendanceMarked;
        
        #endregion

        #region Constructor(s)

        public StaffLeaveDetailsDC()
        {
        }

        public StaffLeaveDetailsDC(int aiSchoolId)
        {
            this.miSchoolId = aiSchoolId;
        }

        #endregion

        #region Property(s)

        public List<ConfiguredLeaves> ConfiguredLeaves
        {
            get { return this.mlstConfiguredLeaves; }
            set { this.mlstConfiguredLeaves = value; }
        }

        public List<UserLateMarkLeave> UserLateMarkLeaves
        {
            get { return this.mlstUserLateMarkLeaves; }
            set { this.mlstUserLateMarkLeaves = value; }
        }

        public List<LateMarkConfiguration> LateMarkConfigurations
        {
            get { return this.mlstLateMarkConfigurations; }
            set { this.mlstLateMarkConfigurations = value; }
        }

        public List<StaffLeaveDetails> StaffLeaveDetails
        {
            get { return this.mlstStaffLeaveDetails; }
            set { this.mlstStaffLeaveDetails = value; }
        }

        public List<UsersSalaryDeduction> UsersSalaryDeductions
        {
            get { return this.mlstUsersSalaryDeductions; }
            set { this.mlstUsersSalaryDeductions = value; }
        }

        public List<UserLateMarkLeave> UserLateMarks
        {
            get { return this.mlstUserLateMarks; }
        }

        public List<LeaveYear> LeaveYears
        {
            get { return mlstLeaveYears; }
        }

        public List<int> WeekendDays
        {
            get { return mlstWeekends; }
        }

        public List<DaywiseStaffAttendance> DaywiseStaffAttendances
        {
            get { return mlstDaywiseStaffAttendance; }
        }

        public List<HolidayMaster> Holidays
        {
            get { return mlstHolidays; }
        }

        public List<DatewiseStaffLeave> DatewiseStaffLeaves
        {
            get { return mlstDatewiseStaffLeaves; }
        }

        public List<UserBasicDetails> UserDetails
        {
            get { return this.mlstUserDetails; }
        }

        public List<LeaveBalanceDetails> LeaveBalanceDetails
        {
            get { return this.mlstLeaveBalanceDetails; }
        }

        public string SchoolName
        {
            get { return msSchoolName; }
        }

        public bool IsAttendanceMarked
        {
            get { return mbIsAttendanceMarked; }
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to populate leave entity.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetLeaves(SqlDataReader oSqlDataReader)
        {
            ConfiguredLeaves oConfiguredLeavesDC;
            while (oSqlDataReader.Read())
            {
                oConfiguredLeavesDC = new ConfiguredLeaves
                {
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    IsUnpaidLeave = Convert.ToBoolean(oSqlDataReader["IsUnpaidLeave"]),
                };
                this.mlstConfiguredLeaves.Add(oConfiguredLeavesDC);
            }
        }

        /// <summary>
        /// This method is used to fill late mark entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void SetLateMarkDetails(SqlDataReader aoSqlDataReader)
        {
            UserLateMarkLeave oUserLateMark;
            while (aoSqlDataReader.Read())
            {
                oUserLateMark = new UserLateMarkLeave
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    Days = Convert.ToDecimal(aoSqlDataReader["Days"]),
                    IsUnPaidLeave = Convert.ToBoolean(aoSqlDataReader["IsUnPaidLeave"])
                };
                this.mlstUserLateMarkLeaves.Add(oUserLateMark);
            }
        }

        /// <summary>
        /// This method is used to fill late mark configuration entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void SetLateMarkConfig(SqlDataReader aoSqlDataReader)
        {
            LateMarkConfiguration oLateMarkConfiguration = null;
            while (aoSqlDataReader.Read())
            {
                oLateMarkConfiguration = new LateMarkConfiguration
                {
                    LateMarkCount = Convert.ToInt32(aoSqlDataReader["LateMarkCount"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    ConsideredLeaves = Convert.ToDecimal(aoSqlDataReader["ConsideredLeaves"]),
                };
                this.mlstLateMarkConfigurations.Add(oLateMarkConfiguration);
            }
        }

        /// <summary>
        /// This method is used to fill staff leave entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetStaffLeavesDetails(SqlDataReader oSqlDataReader)
        {
            StaffLeaveDetails oStaffLeaveDetailsDC;
            while (oSqlDataReader.Read())
            {
                oStaffLeaveDetailsDC = new StaffLeaveDetails
                {
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    Days = Convert.ToDecimal(oSqlDataReader["Days"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"])
                };
                this.mlstStaffLeaveDetails.Add(oStaffLeaveDetailsDC);
            }
        }

        /// <summary>
        /// This method is used to fill staff holidayk entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        public void SetStaffHolidayConfiguration(SqlDataReader aoSqlDataReader)
        {
            UsersSalaryDeduction oUsersSalaryDeduction = null;
            while (aoSqlDataReader.Read())
            {
                oUsersSalaryDeduction = new UsersSalaryDeduction
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    Days = Convert.ToInt32(aoSqlDataReader["Days"]),
                    PercentageToDeduct = Convert.ToDecimal(aoSqlDataReader["PercentageToDeduct"]),
                };
                this.mlstUsersSalaryDeductions.Add(oUsersSalaryDeduction);
            }
        }

        #endregion

        #region Public MEthod(s)

        /// <summary>
        /// This method is used to return all user details.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcadmicYearId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public List<UserBasicDetails> GetAllUsers(int aiStaffGroupId, int aiSchoolId, int aiAcadmicYearId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcadmicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUsersDetails");
                List<UserBasicDetails> lstUserBasicDetails = new List<UserBasicDetails>();
                while (oSqlDataReader.Read())
                {
                    lstUserBasicDetails.Add
                        (
                            new UserBasicDetails
                            {
                                UserId = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                                StaffName = Convert.ToString(oSqlDataReader["Display_Member"])
                            }
                        );
                }
                return lstUserBasicDetails;
            }
        }

        /// <summary>
        /// This method is used to return all user details For OD Screen.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcadmicYearId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public List<UserBasicDetails> GetAllUsersForODDetails(int aiStaffGroupId, int aiSchoolId, int aiAcadmicYearId, int aiYear, bool abIsForInOutDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcadmicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsForInOutDetails", abIsForInOutDetails, SqlDbType.Bit);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForODDetails");
                List<UserBasicDetails> lstUserBasicDetails = new List<UserBasicDetails>();
                while (oSqlDataReader.Read())
                {
                    if (!abIsForInOutDetails)
                    {
                        lstUserBasicDetails.Add
                            (
                                new UserBasicDetails
                                {
                                    UserId = Convert.ToInt32(oSqlDataReader["Value_Member"]),
                                    StaffName = Convert.ToString(oSqlDataReader["Display_Member"])
                                }
                            );
                    }
                    else
                    {
                        lstUserBasicDetails.Add
                            (
                                new UserBasicDetails
                                {
                                    EmployeeNo = oSqlDataReader["Value_Member"].ToString(),
                                    StaffName = Convert.ToString(oSqlDataReader["Display_Member"])
                                }
                            );
                    }
                }
                return lstUserBasicDetails;
            }
        }

        /// <summary>
        /// This method is used to return all leave details.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="aiMonthId"></param>
        /// <returns></returns>
        public List<UserLeaveDetails> GetLeaveDetailsToExport(int aiStaffGroupId, int aiSchoolId, int aiUserId, int aiYear, int aiMonthId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                if (aiMonthId != 0)
                    oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);

                if (aiStaffGroupId != 0)
                    oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);

                if (aiUserId != 0)
                    oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllLeaveDetailsToExport");

                List<UserLeaveDetails> lstUserLeaveDetails = GetUserLeaveDetails(oSqlDataReader);
                oSqlDataReader.NextResult();
                FillConfiguredLeaves(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillWeekends(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillHolidays(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillUserIds(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillLateMarkDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                if (oSqlDataReader.Read())
                {
                    msSchoolName = oSqlDataReader["School_Name"].ToString();
                    mbIsAttendanceMarked = oSqlDataReader["IsAttendanceMarked"].ToBool();
                }
                else
                    msSchoolName = string.Empty;

                return lstUserLeaveDetails;
            }
        }

        /// <summary>
        /// THis method is sued to return leave balance details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<LeaveBalanceDetails> GetLeaveBalanceToExport(int aiSchoolId, int aiStaffGroupId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeaveBalanceToExport");

                List<LeaveBalanceDetails> lstLeaveBalanceDetails = new List<LeaveBalanceDetails>();

                while (oSqlDataReader.Read())
                {
                    lstLeaveBalanceDetails.Add
                        (
                            new LeaveBalanceDetails
                            {
                                RowNo = Convert.ToInt32(oSqlDataReader["RowNo"]),
                                LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                UserName = Convert.ToString(oSqlDataReader["UserName"]),
                                LeaveName = Convert.ToString(oSqlDataReader["LeaveName"]),
                                LeaveBalance = Convert.ToDecimal(oSqlDataReader["LeaveBalance"])
                            }
                        );
                }

                return lstLeaveBalanceDetails;
            }
        }

        /// <summary>
        /// This method is used to return staff leaves.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <returns></returns>
        public List<DateWiseStaffLeaves> GetStaffwiseLeaves(int aiUserId, int aiStaffGroupId, DateTime adtStartDate, DateTime adtEndDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStaffwiseLeaveDetails"))
                {
                    FillUserDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillLeaveTypes(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillLeaveBalance(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillLateMarkCount(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillLeaveYears(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    return FillDatewiseLeaves(oSqlDataReader);
                }
            }
        }

        private void FillLeaveYears(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstLeaveYears.Add(
                    new LeaveYear
                    {
                        Id = aoSqlDataReader["Id"].ToInt(),
                        StartDate = aoSqlDataReader["StartDate"].ToDateTime(),
                        EndDate = aoSqlDataReader["EndDate"].ToDateTime()
                    }
                 );
            }
        }

        private void FillLateMarkCount(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstUserLateMarks.Add(
                        new UserLateMarkLeave
                        {
                            UserId = aoSqlDataReader["UserId"].ToInt(),
                            Year = aoSqlDataReader["Year"].ToInt(),
                            LeaveId = aoSqlDataReader["LeaveId"].ToInt(),                            
                            Days = aoSqlDataReader["TotalLeaves"].ToDecimal()
                        }
                    );
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill leave balance.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillLeaveBalance(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstLeaveBalanceDetails.Add
                    (
                        new LeaveBalanceDetails
                        {
                            LeaveYear = Convert.ToInt32(aoSqlDataReader["LeaveYear"]),
                            UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                            LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                            LeaveBalance = Convert.ToDecimal(aoSqlDataReader["LeaveBalance"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill leave type.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillLeaveTypes(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstConfiguredLeaves.Add
                    (
                        new ConfiguredLeaves
                        {
                            ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                            OriginalLeaveId = Convert.ToInt32(aoSqlDataReader["OriginalLeaveId"]),
                            LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                            CanAccumulate = Convert.ToBoolean(aoSqlDataReader["CanAccumulate"]),
                            IsUnpaidLeave = Convert.ToBoolean(aoSqlDataReader["IsUnpaidLeave"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill date wise staff leaves.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<DateWiseStaffLeaves> FillDatewiseLeaves(SqlDataReader aoSqlDataReader)
        {
            List<DateWiseStaffLeaves> lstDateWiseStaffLeaves = new List<DateWiseStaffLeaves>();
            while (aoSqlDataReader.Read())
            {
                lstDateWiseStaffLeaves.Add
                    (
                        new DateWiseStaffLeaves
                        {
                            DateWiseStaffUserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                            LeaveDate = Convert.ToDateTime(aoSqlDataReader["Date"]),
                            LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                            IsHalfLeave = Convert.ToBoolean(aoSqlDataReader["IsHalfLeave"]),
                            IsPartialLeave = Convert.ToBoolean(aoSqlDataReader["IsPartialLeave"])
                        }
                    );
            }
            return lstDateWiseStaffLeaves;
        }

        /// <summary>
        /// This method is used to fill user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstUserDetails.Add
                    (
                        new UserBasicDetails
                        {
                            UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                            StaffName = Convert.ToString(aoSqlDataReader["UserName"]),
                            SrNo = Convert.ToInt32(aoSqlDataReader["SrNo"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill configured leave types.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillConfiguredLeaves(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstConfiguredLeaves.Add(
                    new ConfiguredLeaves
                    {
                        LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                        LeaveName = Convert.ToString(aoSqlDataReader["LeaveName"]),
                        ShortName = Convert.ToString(aoSqlDataReader["ShortName"])
                    }
                );
            }
        }

        /// <summary>
        /// This method is used to return leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private static List<UserLeaveDetails> GetUserLeaveDetails(SqlDataReader aoSqlDataReader)
        {
            List<UserLeaveDetails> lstUserLeaveDetails = new List<UserLeaveDetails>();
            while (aoSqlDataReader.Read())
            {
                lstUserLeaveDetails.Add
                    (
                        new UserLeaveDetails
                        {
                            RowNo = Convert.ToInt32(aoSqlDataReader["RowNo"]),
                            Day = Convert.ToInt32(aoSqlDataReader["Day"]),
                            IsHalfLeave = Convert.ToBoolean(aoSqlDataReader["IsHalfLeave"]),
                            IsLateMark = Convert.ToBoolean(aoSqlDataReader["IsLateMark"]),
                            PartialLeaveId = Convert.ToInt32(aoSqlDataReader["PartialLeaveId"]),
                            LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                            MonthId = Convert.ToInt32(aoSqlDataReader["MonthId"]),
                            UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                            UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                            Year = Convert.ToInt32(aoSqlDataReader["Year"]),
                            LeaveName = Convert.ToString(aoSqlDataReader["LeaveName"]),
                            LeaveColor = Convert.ToString(aoSqlDataReader["LeaveColor"])                           
                        }
                    );
            }
            return lstUserLeaveDetails;
        }

        /// <summary>
        /// This method is used to fill late mark details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillLateMarkDetails(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstDatewiseStaffLeaves.Add
                    (
                        new DatewiseStaffLeave
                        {
                            UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                            Date = Convert.ToDateTime(aoSqlDataReader["Date"]),
                            LateMarkLeaveCount = Convert.ToDecimal(aoSqlDataReader["LateMarkLeaveCount"]),
                        }
                    );
            }
        }

        /// <summary>
        /// THis method is used to fill user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillUserIds(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstDaywiseStaffAttendance.Add(
                    new DaywiseStaffAttendance
                    {
                        SrNo = Convert.ToInt32(aoSqlDataReader["RowNo"]),
                        UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                        Name = Convert.ToString(aoSqlDataReader["UserName"]),
                        EmployeeNo = Convert.ToString(aoSqlDataReader["EmployeeNo"])
                    }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill holidays.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillHolidays(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstHolidays.Add(
                    new HolidayMaster
                    {
                        Id = Convert.ToInt32(aoSqlDataReader["Holiday_Id"]),
                        StatDate = Convert.ToDateTime(aoSqlDataReader["Holiday_Start_Date"]),
                        EndDate = Convert.ToDateTime(aoSqlDataReader["Holiday_End_Date"]),
                        HolidayName = Convert.ToString(aoSqlDataReader["Holiday_Name"])
                    }
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillWeekends(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
            {
                mlstWeekends.Add(Convert.ToInt32(aoSqlDataReader["Day"]));
            }
        }

        #endregion
    }
}
