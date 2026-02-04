
// Class Name       :- DatewiseStaffLeavesDC
// Purpose          :- This class is used to manage DatewiseStaffLeaves details.
// Date Of creation :- 28-Aug-10
// Created By       :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using SchoolEntities;

namespace DataCommunicator
{
    public class DatewiseStaffLeavesDC
    {
        #region Data Members

        private DatewiseStaffLeave moDatewiseStaffLeaves;

        public List<DaywiseLeaves> StaffLeaves = new List<DaywiseLeaves>();
        public List<DaywiseLeaves> DatewiseLeaves = new List<DaywiseLeaves>();
        public List<DaywiseLeaves> StaffLeaveDetails = new List<DaywiseLeaves>();
        public List<DaywiseLeaves> UserLeavesYearwiseConfigurations = new List<DaywiseLeaves>();
        public List<LateMarkConfiguration> LateMarkConfigurations = new List<LateMarkConfiguration>();
        public List<LateMarkLeave> LateMarkLeaves = new List<LateMarkLeave>();
        public List<string> StaffLeaveSortOrders = new List<string>();
        public List<UsersSalaryDeduction> UsersSalaryDeductions = new List<UsersSalaryDeduction>();
        public List<StaffHolidaysSalaryDeduction> StaffHolidayAndSalaryDeductionConfigurations = new List<StaffHolidaysSalaryDeduction>();
        public List<WeekDay> weekDays = new List<WeekDay>();
        public List<SalaryCommonUtility> SalaryCommonUtilityList = new List<SalaryCommonUtility>();
        public StaffBaseDetails moStaffBaseDetails;
        public List<PartialLeaveDetails> PartialLeaveDetailsList = new List<PartialLeaveDetails>();
        public List<UsersLeaveBalance> UsersLeaveBalanceList = new List<UsersLeaveBalance>();
        public List<ConfiguredLeaves> ConfiguredLeavesList = new List<ConfiguredLeaves>();
        public SalaryCommonUtility SalaryCommonUtility;
        public List<MonthwiseStaffLeaveDetails> MonthwiseStaffLeaveDetailsList = new List<MonthwiseStaffLeaveDetails>();
        public List<MonthDetails> MonthDetailsList = new List<MonthDetails>();
        public List<DateWiseStaffLeaves> DateWiseStaffLeavesList = new List<DateWiseStaffLeaves>();
        public List<WeekDayDetails> WeekDayDetailsList = new List<WeekDayDetails>();
        public List<HolidayDetails> HolidayDetailsList = new List<HolidayDetails>();

        private int miPreAttachedHolidayId;
        private int miIsPostAttachedHolidayId;
        private int miSchoolId;
        private int miAcademicYearId;

        #endregion

        #region Constructors

        public DatewiseStaffLeavesDC()
        {
        }

        public DatewiseStaffLeavesDC(int miDatewiseStaffLeavesId)
        {
        }

        public DatewiseStaffLeavesDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }
        #endregion

        #region Property

        public DatewiseStaffLeave DatewiseStaffLeaves
        {
            get { return moDatewiseStaffLeaves; }
            set { moDatewiseStaffLeaves = value; }
        }

        public int PreAttachedHolidayId
        {
            get { return miPreAttachedHolidayId; }
        }

        public int PostAttachedHolidayId
        {
            get { return miIsPostAttachedHolidayId; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to save datewise leave details.
        /// </summary>
        public void Insert()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moDatewiseStaffLeaves.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moDatewiseStaffLeaves.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", moDatewiseStaffLeaves.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moDatewiseStaffLeaves.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveXml", moDatewiseStaffLeaves.LeaveXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("LateMarkLeaveXml", moDatewiseStaffLeaves.LateMarkLeaveXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("MonthId", moDatewiseStaffLeaves.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", moDatewiseStaffLeaves.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExcludeFromSalaryDeduction", moDatewiseStaffLeaves.ExcludeFromSalaryDeduction, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StaffHolidayLeaveConfigIds", moDatewiseStaffLeaves.StaffHolidayLeaveConfigIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("HolidayLeave", moDatewiseStaffLeaves.Holidays, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertDatewiseUserLeaves");
            }
        }

        /// <summary>
        /// This method is used to return leave details.
        /// </summary>
        public void GetUserLeavesDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moDatewiseStaffLeaves.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moDatewiseStaffLeaves.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", moDatewiseStaffLeaves.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", moDatewiseStaffLeaves.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", moDatewiseStaffLeaves.UserId, SqlDbType.Int);
                if (moDatewiseStaffLeaves.StaffGroupsId != 0)
                    oSQLServerDbUtility.AddParameter("StaffGroupsId", moDatewiseStaffLeaves.StaffGroupsId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDatewiseUsersLeaves"))
                {
                    if (oSqlDataReader != null)
                    {
                        SetStafLeavesDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetDatewiseStaffLeaves(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetStaffLeaveDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetUserLeavesYearwiseConfiguration(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetLateMarkConfiguration(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetStaffLeaveSortOrder(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetLateMarkLeaves(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetUsersSalaryDeduction(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetUsersLeaveHolidayConfig(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetWeekDayConfig(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetUserDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetUserJoiningDates(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            SetPartialLeaves(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                        {
                            oSqlDataReader.Read();
                            miPreAttachedHolidayId = Convert.ToInt32(oSqlDataReader["IsPreAttachedLeave"]);
                            miIsPostAttachedHolidayId = Convert.ToInt32(oSqlDataReader["IsPostAttachedLeave"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to return monthwise staff attendance details.
        /// </summary>        
        public List<StaffDetails> GetAllMonthwiseStaffAttendance(int miYearId, int miStaffGroupId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearId", miYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", miStaffGroupId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllMonthwiseStaffAttendance"))
                {
                    List<StaffDetails> lstStaffDetails = SetStaffDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetMonthDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetMonthwiseStaffDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetDateWiseStaffLeaves(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetWeekDayDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetHolidayDetails(oSqlDataReader);
                    return lstStaffDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to set Staff details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private List<StaffDetails> SetStaffDetails(SqlDataReader oSqlDataReader)
        {
            List<StaffDetails> lstStaffDetails = new List<StaffDetails>();
            StaffDetails oStaffDetails = null;
            while (oSqlDataReader.Read())
            {
                oStaffDetails = new StaffDetails
                {
                    RowNo = Convert.ToInt32(oSqlDataReader["RowNo"]),
                    StaffUserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    StaffUserName = Convert.ToString(oSqlDataReader["UserName"]),
                    StaffDesignation = Convert.ToString(oSqlDataReader["Designation"]),
                    IsAdminStaff = Convert.ToBoolean(oSqlDataReader["IsAdminStaff"]),
                };
                lstStaffDetails.Add(oStaffDetails);
            }
            return lstStaffDetails;
        }

        /// <summary>
        /// This method is used to set Month details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetMonthDetails(SqlDataReader oSqlDataReader)
        {
            MonthDetails oMonthDetails = null;
            while (oSqlDataReader.Read())
            {
                oMonthDetails = new MonthDetails()
                {
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"]),
                    Month = Convert.ToString(oSqlDataReader["Month"])
                };
                MonthDetailsList.Add(oMonthDetails);
            }

        }

        /// <summary>
        /// This method is used to set monthwise staff details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetMonthwiseStaffDetails(SqlDataReader oSqlDataReader)
        {
            MonthwiseStaffLeaveDetails oMonthwiseStaffLeaveDetails = null;
            while (oSqlDataReader.Read())
            {
                oMonthwiseStaffLeaveDetails = new MonthwiseStaffLeaveDetails
                {
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    StaffAttendanceUserId = Convert.ToInt32(oSqlDataReader["StaffAttendanceUserId"]),
                    PresentDays = Convert.ToDecimal(oSqlDataReader["PresentDays"]),
                    LeaveDetailsId = Convert.ToInt32(oSqlDataReader["LeaveDetailsId"]),
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    LeaveDays = Convert.ToInt32(oSqlDataReader["LeaveDays"]),
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"])
                };
                MonthwiseStaffLeaveDetailsList.Add(oMonthwiseStaffLeaveDetails);
            }
        }

        /// <summary>
        /// This method is used to set datewise staff leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetDateWiseStaffLeaves(SqlDataReader oSqlDataReader)
        {

            DateWiseStaffLeaves oDateWiseStaffLeaves = null;
            while (oSqlDataReader.Read())
            {
                oDateWiseStaffLeaves = new DateWiseStaffLeaves
                {
                    DatewiseStaffLeaveId = Convert.ToInt32(oSqlDataReader["DatewiseStaffLeavesId"]),
                    DateWiseStaffUserId = Convert.ToInt32(oSqlDataReader["DatewiseStaffUserId"]),
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    LeaveDate = Convert.ToDateTime(oSqlDataReader["LeaveDate"]),
                    IsHalfLeave = Convert.ToBoolean(oSqlDataReader["IsHalfLeave"]),
                    IsPartialLeave = Convert.ToBoolean(oSqlDataReader["IsPartialLeave"]),
                };
                DateWiseStaffLeavesList.Add(oDateWiseStaffLeaves);
            }
        }

        /// <summary>
        /// This method is used to set weekDay details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetWeekDayDetails(SqlDataReader oSqlDataReader)
        {

            WeekDayDetails oWeekDayDetails = null;
            while (oSqlDataReader.Read())
            {
                oWeekDayDetails = new WeekDayDetails
                {
                    OriginalWeekDayId = Convert.ToInt32(oSqlDataReader["Original_WeekDays_Id"]),
                    WeekDayName = Convert.ToString(oSqlDataReader["WeekDay_Name"])
                };
                WeekDayDetailsList.Add(oWeekDayDetails);
            }
        }

        /// <summary>
        /// This method is used to set holiday details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetHolidayDetails(SqlDataReader oSqlDataReader)
        {

            HolidayDetails oHolidayDetails = null;
            while (oSqlDataReader.Read())
            {
                oHolidayDetails = new HolidayDetails
                {
                    HolidayId = Convert.ToInt32(oSqlDataReader["Holiday_Id"]),
                    HolidayStartDate = Convert.ToDateTime(oSqlDataReader["Holiday_Start_Date"]),
                    HolidayEndDate = Convert.ToDateTime(oSqlDataReader["Holiday_End_Date"])
                };
                HolidayDetailsList.Add(oHolidayDetails);
            }
        }

        /// <summary>
        /// This method is used too set partial leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetPartialLeaves(SqlDataReader aoSqlDataReader)
        {
            PartialLeaveDetails oPartialLeaveDetails = null;
            while (aoSqlDataReader.Read())
            {
                oPartialLeaveDetails = new PartialLeaveDetails
                {
                    PartialLeaveId = Convert.ToInt32(aoSqlDataReader["PartialLeaveId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    DatewisePartialStaffLeavesId = Convert.ToInt32(aoSqlDataReader["DatewisePartialStaffLeavesId"]),
                    LeaveDate = Convert.ToDateTime(aoSqlDataReader["LeaveDate"])
                };
                PartialLeaveDetailsList.Add(oPartialLeaveDetails);
            }
        }

        /// <summary>
        /// This method is used to set user's joining date and resignation date.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetUserJoiningDates(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                moStaffBaseDetails = new StaffBaseDetails
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    JoiningDate = aoSqlDataReader["DateOfJoining"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(aoSqlDataReader["DateOfJoining"]),
                    ResignDate = aoSqlDataReader["DateOfResign"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(aoSqlDataReader["DateOfResign"]),
                    PermanentDate = aoSqlDataReader["PermanentDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(aoSqlDataReader["PermanentDate"])
                };
            }
        }

        /// <summary>
        /// This method is used to set user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetUserDetails(SqlDataReader aoSqlDataReader)
        {
            SalaryCommonUtility oSalaryCommonUtility = null;
            while (aoSqlDataReader.Read())
            {
                oSalaryCommonUtility = new SalaryCommonUtility
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"])
                };
                SalaryCommonUtilityList.Add(oSalaryCommonUtility);
            }
        }

        /// <summary>
        /// This method is used to set weekday configurtions.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetWeekDayConfig(SqlDataReader aoSqlDataReader)
        {
            WeekDay oWeekDay = null;
            while (aoSqlDataReader.Read())
            {
                oWeekDay = new WeekDay
                {
                    OriginalWeekDaysId = Convert.ToInt32(aoSqlDataReader["OriginalWeekDaysId"]),
                    WeekDayName = Convert.ToString(aoSqlDataReader["WeekDayName"]),
                    IsWeekend = Convert.ToBoolean(aoSqlDataReader["IsWeekend"])
                };
                weekDays.Add(oWeekDay);
            }
        }

        /// <summary>
        /// This method is used to set user's holiday leave configuration.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetUsersLeaveHolidayConfig(SqlDataReader aoSqlDataReader)
        {
            StaffHolidaysSalaryDeduction oStaffHolidayAndSalaryDeductionConfiguration = null;
            while (aoSqlDataReader.Read())
            {
                oStaffHolidayAndSalaryDeductionConfiguration = new StaffHolidaysSalaryDeduction
                {
                    StaffHolidaysSalaryDeductionId = Convert.ToInt32(aoSqlDataReader["StaffHolidayLeavesConfiguratonId"]),
                    HolidayName = Convert.ToString(aoSqlDataReader["HolidayName"]),
                    HolidayStartDate = Convert.ToDateTime(aoSqlDataReader["HolidayStartDate"]),
                    HolidayEndDate = Convert.ToDateTime(aoSqlDataReader["HolidayEndDate"]),
                    PercentageToDeduct = Convert.ToDecimal(aoSqlDataReader["PercentageToDeduct"]),
                    Type = Convert.ToInt32(aoSqlDataReader["Type"]),
                    IsWeekend = Convert.ToBoolean(aoSqlDataReader["IsWeekend"])
                };
                StaffHolidayAndSalaryDeductionConfigurations.Add(oStaffHolidayAndSalaryDeductionConfiguration);
            }
        }

        /// <summary>
        /// This method is used to set user's salary deduction details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetUsersSalaryDeduction(SqlDataReader aoSqlDataReader)
        {
            UsersSalaryDeduction oUsersSalaryDeduction = null;
            while (aoSqlDataReader.Read())
            {
                oUsersSalaryDeduction = new UsersSalaryDeduction
                {
                    UsersSalaryDeductionId = Convert.ToInt32(aoSqlDataReader["UsersSalaryDeductionId"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    StaffHolidayAndLeavesConfigurationId = Convert.ToInt32(aoSqlDataReader["StaffHolidayLeavesConfiguratonId"]),
                    MonthId = Convert.ToInt32(aoSqlDataReader["MonthId"]),
                    Year = Convert.ToInt32(aoSqlDataReader["Year"])
                };
                UsersSalaryDeductions.Add(oUsersSalaryDeduction);
            }
        }

        /// <summary>
        /// This method is used to set late mark configurations.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetLateMarkLeaves(SqlDataReader aoSqlDataReader)
        {
            LateMarkLeave oLateMarkLeave = null;
            while (aoSqlDataReader.Read())
            {
                oLateMarkLeave = new LateMarkLeave
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    Days = Convert.ToDecimal(aoSqlDataReader["Days"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"])
                };
                LateMarkLeaves.Add(oLateMarkLeave);
            }

        }

        /// <summary>
        /// This method is used to set user's yearwise leaves configuration.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetUserLeavesYearwiseConfiguration(SqlDataReader aoSqlDataReader)
        {
            DaywiseLeaves oDaywiseLeaves = null;
            while (aoSqlDataReader.Read())
            {
                oDaywiseLeaves = new DaywiseLeaves
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    LeaveBalance = Convert.ToDecimal(aoSqlDataReader["LeaveBalance"]),
                    MinimumBalance = Convert.ToDecimal(aoSqlDataReader["MinimumBalance"])
                };
                UserLeavesYearwiseConfigurations.Add(oDaywiseLeaves);
            }
        }

        /// <summary>
        /// This method is used to set staff leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetStaffLeaveDetails(SqlDataReader aoSqlDataReader)
        {
            DaywiseLeaves oDaywiseLeaves = null;
            while (aoSqlDataReader.Read())
            {
                oDaywiseLeaves = new DaywiseLeaves
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    Days = Convert.ToDecimal(aoSqlDataReader["Days"])
                };
                StaffLeaveDetails.Add(oDaywiseLeaves);
            }
        }

        /// <summary>
        /// This method is used to set datewise staff leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetDatewiseStaffLeaves(SqlDataReader aoSqlDataReader)
        {
            DaywiseLeaves oDaywiseLeaves = null;
            while (aoSqlDataReader.Read())
            {
                oDaywiseLeaves = new DaywiseLeaves
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    IsHalfLeave = Convert.ToBoolean(aoSqlDataReader["IsHalfLeave"]),
                    Date = Convert.ToDateTime(aoSqlDataReader["Date"]),
                    IsLateMark = Convert.ToBoolean(aoSqlDataReader["IsLateMark"])
                };
                DatewiseLeaves.Add(oDaywiseLeaves);
            }
        }

        /// <summary>
        /// This method is used to set staff leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetStafLeavesDetails(SqlDataReader aoSqlDataReader)
        {
            DaywiseLeaves oDaywiseLeaves = null;
            while (aoSqlDataReader.Read())
            {
                oDaywiseLeaves = new DaywiseLeaves
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    OriginalLeaveId = Convert.ToInt32(aoSqlDataReader["OriginalLeaveId"]),
                    IsUnPaidLeave = Convert.ToBoolean(aoSqlDataReader["IsUnPaidLeave"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    ColorCode = Convert.ToString(aoSqlDataReader["ColorCode"]),
                    ExcludeFromSalaryDeduction = Convert.ToBoolean(aoSqlDataReader["ExcludeFromDeduction"]),
                    AllowZeroBalance = Convert.ToBoolean(aoSqlDataReader["AllowZeroBalance"])
                };
                StaffLeaves.Add(oDaywiseLeaves);
            }
        }

        /// <summary>
        /// This method is used to set late mark configuration.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetLateMarkConfiguration(SqlDataReader aoSqlDataReader)
        {
            LateMarkConfiguration LateMarkConfiguration;
            while (aoSqlDataReader.Read())
            {
                LateMarkConfiguration = new LateMarkConfiguration
                {
                    LateMarkCount = Convert.ToInt32(aoSqlDataReader["LateMarkCount"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    ConsideredLeaves = Convert.ToDecimal(aoSqlDataReader["ConsideredLeaves"]),
                };
                LateMarkConfigurations.Add(LateMarkConfiguration);
            }
        }

        /// <summary>
        /// This method is used to set leave sort oder for holida leave deduction.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void SetStaffLeaveSortOrder(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
                StaffLeaveSortOrders.Add(aoSqlDataReader["ShortName"].ToString());
        }

        /// <summary>
        /// This method is used to return partial leave details.
        /// </summary>
        public void GetUsersPartialLeaveDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moDatewiseStaffLeaves.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moDatewiseStaffLeaves.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", moDatewiseStaffLeaves.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", moDatewiseStaffLeaves.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", moDatewiseStaffLeaves.UserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserPartialLeaveDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        FillUserPartialLeaveDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillLeaveBalance(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillConfiguredLeaves(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                        {
                            if (oSqlDataReader.Read())
                            {
                                SalaryCommonUtility = new SalaryCommonUtility
                                {
                                    Name = Convert.ToString(oSqlDataReader["Name"]),
                                    StaffGroupsName = Convert.ToString(oSqlDataReader["StaffGroupsName"])
                                };
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to set configured leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillConfiguredLeaves(SqlDataReader aoSqlDataReader)
        {
            ConfiguredLeaves oConfiguredLeaves = null;
            while (aoSqlDataReader.Read())
            {
                oConfiguredLeaves = new ConfiguredLeaves
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"])
                };
                ConfiguredLeavesList.Add(oConfiguredLeaves);
            }
        }

        /// <summary>
        /// This method is used to set leave balance details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillLeaveBalance(SqlDataReader aoSqlDataReader)
        {
            UsersLeaveBalance oUsersLeaveBalance = null;
            while (aoSqlDataReader.Read())
            {
                oUsersLeaveBalance = new UsersLeaveBalance
                {
                    LeaveBalance = Convert.ToDecimal(aoSqlDataReader["LeaveBalance"]),
                    configuredLeaves = new ConfiguredLeaves
                    {
                        LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                        ShortName = Convert.ToString(aoSqlDataReader["ShortName"])
                    }
                };
                UsersLeaveBalanceList.Add(oUsersLeaveBalance);
            }
        }

        /// <summary>
        /// This method is used to set user's partial leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillUserPartialLeaveDetails(SqlDataReader aoSqlDataReader)
        {
            PartialLeaveDetails oPartialLeaveDetails = null;
            while (aoSqlDataReader.Read())
            {
                oPartialLeaveDetails = new PartialLeaveDetails
                {
                    ExistingLeaveId = Convert.ToInt32(aoSqlDataReader["ExistingLeaveId"]),
                    PartialLeaveId = Convert.ToInt32(aoSqlDataReader["PartialLeaveId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    DatewisePartialStaffLeavesId = Convert.ToInt32(aoSqlDataReader["DatewisePartialStaffLeavesId"]),
                    LeaveDate = Convert.ToDateTime(aoSqlDataReader["LeaveDate"])
                };
                PartialLeaveDetailsList.Add(oPartialLeaveDetails);
            }
        }

        /// <summary>
        /// This method is used to get user staff group And user id for Search.
        /// </summary>
        /// <param name="asName"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public UserDetailsForLeave GetUserDetailsForLeave(string asName,int aiSchoolId)
        {
            UserDetailsForLeave oUserDetailsForLeave;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserName", asName, SqlDbType.NVarChar);
                oUserDetailsForLeave = new UserDetailsForLeave();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserDetailsForLeave"))                
                    if (oSqlDataReader.Read())
                    {
                        oUserDetailsForLeave.StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]);
                        oUserDetailsForLeave.UserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                    }
                return oUserDetailsForLeave;
            }
        }      

        #endregion
    }
}
