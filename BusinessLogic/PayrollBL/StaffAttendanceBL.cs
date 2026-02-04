// Class Name       :- StaffAttendanceBL
// Purpose          :- This class is used to manage StaffAttendance details.
// Date Of creation :- 22-March-2011
// Author Name      :- Sachin

using System.Collections.Generic;
using DataCommunicator;
using System.Linq;
using PayrollEntities;
using System;
using Utility;
using System.Data;

namespace BusinessLogic
{
    public class StaffAttendanceBL
    {
        #region Constant

        private const string S_HOLIDAY_LEAVE = "Holiday Leaves";
        private const string S_ATTENDANCE = "Attendance"; 

        #endregion

        #region Data Member

        private StaffAttendanceDC moStaffAttendanceDC;
        
        #endregion

        #region Constructor

        public StaffAttendanceBL()
        {
            moStaffAttendanceDC = new StaffAttendanceDC();
        } 

        #endregion

        #region Properties

        public StaffAttendance StaffAttendance
        {
            get { return moStaffAttendanceDC.StaffAttendance; }
            set { moStaffAttendanceDC.StaffAttendance = value; }
        }

        public List<StaffAttendance> StaffAttendances
        {
            get { return moStaffAttendanceDC.StaffAttendances; }
            set { moStaffAttendanceDC.StaffAttendances = value; }
        }

        public List<StaffLeaveDetails> StaffLeaveDetails
        {
            get{  return moStaffAttendanceDC.StaffLeaveDetails; }
            set { moStaffAttendanceDC.StaffLeaveDetails = value; }
        }

        public List<StaffAttendance> StaffAttendanceDetails
        {
            get { return moStaffAttendanceDC.StaffAttendanceDetails; }
            set { moStaffAttendanceDC.StaffAttendanceDetails = value; }
        }

        public List<ConfiguredLeaves> ConfiguredLeaves
        {
            get { return moStaffAttendanceDC.ConfiguredLeaves; }
        }

        public bool IsSalaryPublished
        {
            get { return moStaffAttendanceDC.IsSalaryPublished; }
        }

        #endregion

        #region Methods

        public void GetStaffGroupUsers()
        {
            moStaffAttendanceDC.GetStaffGroupUsers();
        }

        public void SaveStaffAttendance()
        {
            moStaffAttendanceDC.SaveStaffAttendance();
        }

        public List<DaywiseStaffAttendance> GetAll(int aiSchoolId, int aiAcademicYearId, DateTime adtDate, int aiStaffGroupId, string asFilter)
        {
            return moStaffAttendanceDC.GetAll(aiSchoolId, aiAcademicYearId, adtDate, aiStaffGroupId, asFilter);
        }

        public void SaveDaywiseLeaves(int aiSchoolId, int aiUserId, DateTime adtDate, string asLeaveXml)
        {
            moStaffAttendanceDC.SaveDaywiseLeaves(aiSchoolId, aiUserId, adtDate, asLeaveXml);
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to set attendance details.
        /// </summary>
        /// <param name="aiRowIndex"></param>
        /// <param name="adcUnpaidLeaves"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public decimal SetAttendanceDetails(int aiRowIndex, decimal adcUnpaidLeaves, int aiUserId, DataTable aoDTSalaryDetails, int aiDaysOfMonth, StaffLeaveDetailsBL aoStaffLeaveDetailsBL)
        {
            int iAttendanceId = 0;
            decimal dcTotalDays = 0;
            decimal dcAttendance = 0;

            string sLateMarkLeave = aoStaffLeaveDetailsBL.GetLateMarkLeave(aiUserId);
            List<StaffAttendance> olstStaffAttendance = StaffAttendanceDetails.Where(attendance => attendance.UserId == aiUserId).Select(attendance => attendance).ToList();

            // If attendance is present.
            if (olstStaffAttendance.Count() > 0)
            {
                var oUsersAttendance = olstStaffAttendance.First();

                dcAttendance = oUsersAttendance.PresentDays;
                iAttendanceId = oUsersAttendance.StaffAttendanceId;

                // Get used leave.
                var oStaffLeavesDetails = from StaffLeavesDtl in aoStaffLeaveDetailsBL.StaffLeaveDetails
                                          join Leave in aoStaffLeaveDetailsBL.ConfiguredLeaves
                                          on StaffLeavesDtl.LeaveId equals Leave.LeaveId
                                          where StaffLeavesDtl.StaffAttendanceId == oUsersAttendance.StaffAttendanceId
                                          select StaffLeavesDtl;

                if (oStaffLeavesDetails.Count() > 0)
                {
                    decimal dcTotalLeaves = 0;
                    // Calculate total leaves.
                    foreach (var staffLeaves in oStaffLeavesDetails)
                    {
                        aoDTSalaryDetails.Rows[aiRowIndex][staffLeaves.ShortName.ToString()] = staffLeaves.Days;
                        dcTotalLeaves = dcTotalLeaves + Convert.ToDecimal(staffLeaves.Days);
                    }
                    dcTotalDays = Convert.ToDecimal(oUsersAttendance.PresentDays) + dcTotalLeaves + adcUnpaidLeaves;
                }
            }
            else
            {
                dcAttendance = 0;
                dcTotalDays = 0;
                iAttendanceId = 0;

                List<ConfiguredDefaultLeaves> olstConfiguredLeaves = aoStaffLeaveDetailsBL.ConfiguredLeaves.Select(leave => new ConfiguredDefaultLeaves { ShortName = leave.ShortName, Days = 0, LeaveId = leave.LeaveId }).ToList();
                olstConfiguredLeaves.ForEach(leave => aoDTSalaryDetails.Rows[aiRowIndex][leave.ShortName.ToString()] = leave.Days);
            }

            decimal dcHolidayLeaves = aoStaffLeaveDetailsBL.GetStaffHolidayLeaveDeductions(aiUserId);

            // This is to consider all present days for non salary paid month.
            dcTotalDays = aiDaysOfMonth;

            aoDTSalaryDetails.Rows[aiRowIndex][PayrollConstants.S_LATE_MARK_LEAVES] = sLateMarkLeave;
            aoDTSalaryDetails.Rows[aiRowIndex][S_HOLIDAY_LEAVE] = dcHolidayLeaves;
            aoDTSalaryDetails.Rows[aiRowIndex][S_ATTENDANCE] = dcAttendance;
            aoDTSalaryDetails.Rows[aiRowIndex][PayrollConstants.S_TOTAL] = dcTotalDays;
            return dcTotalDays;
        }

        #endregion
    }
}
