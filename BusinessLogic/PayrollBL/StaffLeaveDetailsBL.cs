// Class Name       :- StaffLeaveDetailsBL
// Purpose          :- This class is used to manage StaffLeaveDetails details.
// Date Of creation :- 15-1-2010
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataCommunicator;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    public class StaffLeaveDetailsBL
    {
        #region Constants

        private const string S_UNPAID_LEAVES = "Unpaid Leaves";
        private const string S_ATTENDANCE = "Attendance";
        private const string S_HOLIDAY_LEAVE = "Holiday Leaves";

        #endregion

        #region Data Member(s)

        private StaffLeaveDetailsDC moStaffLeaveDetailsDC;

        #endregion

        #region Constructor(s)

        public StaffLeaveDetailsBL()
        {
            this.moStaffLeaveDetailsDC = new StaffLeaveDetailsDC();
        }

        public StaffLeaveDetailsBL(int aiSchoolId)
        {
            this.moStaffLeaveDetailsDC = new StaffLeaveDetailsDC(aiSchoolId);
        }

        #endregion

        #region Property(s)

        public List<ConfiguredLeaves> ConfiguredLeaves
        {
            get { return this.moStaffLeaveDetailsDC.ConfiguredLeaves; }
            set { this.moStaffLeaveDetailsDC.ConfiguredLeaves = value;  }
        }

        public List<UserLateMarkLeave> UserLateMarkLeaves
        {
            get { return this.moStaffLeaveDetailsDC.UserLateMarkLeaves; }
            set { this.moStaffLeaveDetailsDC.UserLateMarkLeaves = value; }
        }

        public List<LateMarkConfiguration> LateMarkConfigurations
        {
            get { return this.moStaffLeaveDetailsDC.LateMarkConfigurations; }
            set { this.moStaffLeaveDetailsDC.LateMarkConfigurations = value; }
        }

        public List<StaffLeaveDetails> StaffLeaveDetails
        {
            get { return this.moStaffLeaveDetailsDC.StaffLeaveDetails; }
            set { this.moStaffLeaveDetailsDC.StaffLeaveDetails = value; }
        }

        public List<UsersSalaryDeduction> UsersSalaryDeductions
        {
            get { return this.moStaffLeaveDetailsDC.UsersSalaryDeductions; }
            set { this.moStaffLeaveDetailsDC.UsersSalaryDeductions = value; }
        }

        public List<UserLateMarkLeave> UserLateMarks
        {
            get { return this.moStaffLeaveDetailsDC.UserLateMarks; }
        }

        public List<int> WeekendDays
        {
            get { return this.moStaffLeaveDetailsDC.WeekendDays; }
        }

        public List<DaywiseStaffAttendance> DaywiseStaffAttendances
        {
            get { return this.moStaffLeaveDetailsDC.DaywiseStaffAttendances; }
        }

        public List<LeaveYear> LeaveYears
        {
            get { return this.moStaffLeaveDetailsDC.LeaveYears; }
        }

        public List<HolidayMaster> Holidays
        {
            get { return this.moStaffLeaveDetailsDC.Holidays; }
        }

        public List<DatewiseStaffLeave> DatewiseStaffLeaves
        {
            get { return this.moStaffLeaveDetailsDC.DatewiseStaffLeaves; }
        }

        public List<UserBasicDetails> UserDetails
        {
            get { return this.moStaffLeaveDetailsDC.UserDetails; }
        }

        public List<LeaveBalanceDetails> LeaveBalanceDetails
        {
            get { return this.moStaffLeaveDetailsDC.LeaveBalanceDetails; }
        }

        public string SchoolName
        {
            get { return this.moStaffLeaveDetailsDC.SchoolName; }
        }

        public bool IsAttendanceMarked
        {
            get { return this.moStaffLeaveDetailsDC.IsAttendanceMarked; }
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to return late mark leaves.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public string GetLateMarkLeave(int aiUserId)
        {
            string sLateMarkLeave = string.Empty;

            if (this.LateMarkConfigurations != null)
            {
                var oLateMarkLeaves = from leave in this.ConfiguredLeaves
                                      join lateMark in this.UserLateMarkLeaves
                                      on leave.LeaveId equals lateMark.LeaveId
                                      where leave.LeaveId == lateMark.LeaveId
                                      && lateMark.UserId == aiUserId
                                      select leave.ShortName + "(" + Math.Round(lateMark.Days, 1) + ")";
                sLateMarkLeave = string.Join(", ", oLateMarkLeaves.ToArray());
            }

            // If late mark present then return details otherwise return '-'.
            if (sLateMarkLeave == string.Empty)
                sLateMarkLeave = "-";
            return sLateMarkLeave;
        }

        /// <summary>
        /// This method is used to return holiday leave deduction details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public decimal GetStaffHolidayLeaveDeductions(int aiUserId)
        {
            decimal dcTotalAmount = 0;

            // Return holiday leaves.
            if (this.UsersSalaryDeductions.Count > 0)
            {
                this.UsersSalaryDeductions.Where(config => config.UserId == aiUserId).ToList()
                .ForEach(config => dcTotalAmount = dcTotalAmount + (config.Days * (config.PercentageToDeduct / 100)));
            }

            return Math.Round(dcTotalAmount, 2);
        }

        /// <summary>
        /// This method is used to return unpaid leave's count.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiRowIndex"></param>
        /// <returns></returns>
        public decimal GetUnpaidLeavesCount(int aiUserId, int aiRowIndex, DataTable aoDTSalaryDetails, List<StaffAttendance> alstStaffAttendanceDetails)
        {
            decimal dcUnpaidLeaves = 0;
            // Get unpaid leaves.
            var oUnpaidLeavesList = from StaffLeaveDetail in this.StaffLeaveDetails
                                    join LeaveConfig in this.ConfiguredLeaves
                                    on StaffLeaveDetail.LeaveId equals LeaveConfig.LeaveId
                                    join attendance in alstStaffAttendanceDetails
                                    on StaffLeaveDetail.StaffAttendanceId equals attendance.StaffAttendanceId
                                    where attendance.UserId == aiUserId
                                    && LeaveConfig.IsUnpaidLeave == true
                                    select StaffLeaveDetail;

            if (oUnpaidLeavesList.Count() > 0)
            {
                // Update table with unpaid leave count.
                var oUnpaidLeaves = (from UserLeaves in oUnpaidLeavesList
                                     group UserLeaves by UserLeaves.StaffAttendanceId into sumDays
                                     select new
                                     {
                                         sumDays.Key,
                                         TotalDays = sumDays.Sum(p => Convert.ToDecimal(p.Days))
                                     }).First();

                aoDTSalaryDetails.Rows[aiRowIndex][S_UNPAID_LEAVES] = oUnpaidLeaves.TotalDays;
                dcUnpaidLeaves = Convert.ToDecimal(oUnpaidLeaves.TotalDays) * -1;
            }
            else
                aoDTSalaryDetails.Rows[aiRowIndex][S_UNPAID_LEAVES] = 0.00;

            decimal dcUnpaidLeavesCount2 = 0;
            dcUnpaidLeavesCount2 = (from UsersLeaveBank in this.UserLateMarkLeaves
                                    where UsersLeaveBank.UserId == aiUserId
                                    && UsersLeaveBank.IsUnPaidLeave == true
                                    select UsersLeaveBank.Days).FirstOrDefault();
            if (dcUnpaidLeavesCount2 != 0)
            {
                dcUnpaidLeaves = dcUnpaidLeaves - Math.Round(dcUnpaidLeavesCount2, 1);
                aoDTSalaryDetails.Rows[aiRowIndex][S_UNPAID_LEAVES] = dcUnpaidLeaves;
            }

            // exclude holiday leaves from unpaid leaves.
            decimal dcHolidayLeaves = this.GetStaffHolidayLeaveDeductions(aiUserId);
            dcUnpaidLeaves = dcUnpaidLeaves - dcHolidayLeaves;
            aoDTSalaryDetails.Rows[aiRowIndex][S_UNPAID_LEAVES] = dcUnpaidLeaves * -1;

            return dcUnpaidLeaves;
        }

        /// <summary>
        /// This method is used to add attendance and leaves columns.
        /// </summary>
        public void AddAttendanceLeavesColumns(DataTable aoDTSalaryDetails, List<string> aolstTotalEarningsDeductions)
        {
            // Leaves       
            aoDTSalaryDetails.AddColumns(this.ConfiguredLeaves.Select(leave => leave.ShortName).ToArray());
            aolstTotalEarningsDeductions.AddRange(this.ConfiguredLeaves.Select(leave => leave.ShortName).ToList());

            // Leave Total       
            aoDTSalaryDetails.AddColumns(new string[] { S_UNPAID_LEAVES, PayrollConstants.S_LATE_MARK_LEAVES, S_HOLIDAY_LEAVE, PayrollConstants.S_TOTAL });
            aolstTotalEarningsDeductions.AddRange(new string[] { S_UNPAID_LEAVES, PayrollConstants.S_TOTAL, S_ATTENDANCE, S_HOLIDAY_LEAVE });
        }

        /// <summary>
        /// This method is used to read default leaves.
        /// </summary>
        public List<ConfiguredDefaultLeaves> SetDefaultLeaves()
        {
            return this.ConfiguredLeaves.Select(leave => new ConfiguredDefaultLeaves { ShortName = leave.ShortName, Days = 0, LeaveId = leave.LeaveId }).ToList();
        } 

        #endregion

        #region Public Method(s)

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
            return moStaffLeaveDetailsDC.GetAllUsers(aiStaffGroupId, aiSchoolId, aiAcadmicYearId, aiYear);
        }

        /// <summary>
        /// This method is used to return all user details Screen.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcadmicYearId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public List<UserBasicDetails> GetAllUsersForODDetails(int aiStaffGroupId, int aiSchoolId, int aiAcadmicYearId, int aiYear, bool abIsForInOutDetails)
        {
            return moStaffLeaveDetailsDC.GetAllUsersForODDetails(aiStaffGroupId, aiSchoolId, aiAcadmicYearId, aiYear, abIsForInOutDetails);
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
            return moStaffLeaveDetailsDC.GetLeaveDetailsToExport(aiStaffGroupId, aiSchoolId, aiUserId, aiYear, aiMonthId);
        }

        /// <summary>
        /// This method is sued to return leave balance details to export.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<LeaveBalanceDetails> GetLeaveBalanceToExport(int aiSchoolId, int aiStaffGroupId, int aiUserId)
        {
            return moStaffLeaveDetailsDC.GetLeaveBalanceToExport(aiSchoolId, aiStaffGroupId, aiUserId);
        }

        /// <summary>
        /// This method is used to return staff leave details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <returns></returns>
        public List<DateWiseStaffLeaves> GetStaffwiseLeaves(int aiUserId, int aiStaffGroupId, DateTime adtStartDate, DateTime adtEndDate)
        {
            return this.moStaffLeaveDetailsDC.GetStaffwiseLeaves(aiUserId, aiStaffGroupId, adtStartDate, adtEndDate);
        }

        #endregion
    }
}
