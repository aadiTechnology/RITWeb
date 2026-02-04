using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace PayrollEntities
{
    class StaffHolidayDetails
    {
    }

    public class StaffDetails : SchoolEntity
    {
        public int RowNo { get; set; }
        public int StaffUserId { get; set; }
        public string StaffUserName { get; set; }
        public string StaffDesignation { get; set; }
        public bool IsAdminStaff { get; set; }
    }

    public class MonthDetails : SchoolEntity
    {
        public int MonthId { get; set; }
        public string Month { get; set; }
    }

    public class MonthwiseStaffLeaveDetails : SchoolEntity
    {
        public int StaffAttendanceId { get; set; }
        public int StaffAttendanceUserId { get; set; }
        public decimal PresentDays { get; set; }
        public int LeaveDetailsId { get; set; }
        public int LeaveId { get; set; }
        public int LeaveDays { get; set; }
        public int MonthId { get; set; }
    }

    public class DateWiseStaffLeaves : SchoolEntity
    {
        public int DatewiseStaffLeaveId { get; set; }
        public int DateWiseStaffUserId { get; set; }
        public int LeaveId { get; set; }
        public DateTime LeaveDate { get; set; }
        public bool IsHalfLeave { get; set; }
        public bool IsPartialLeave { get; set; }
    }

    public class WeekDayDetails : SchoolEntity
    {
        public int OriginalWeekDayId { get; set; }
        public string WeekDayName { get; set; }
    }

    public class HolidayDetails : SchoolEntity
    {
        public int HolidayId { get; set; }
        public DateTime HolidayStartDate { get; set; }
        public DateTime HolidayEndDate { get; set; }
    }
}
