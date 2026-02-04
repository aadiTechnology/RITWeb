using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.MusterRollDetails
{
    class MusterRollDetails
    {
    }

    public class AttendanceDetails
    {
        public DateTime AttendanceDate { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
        public bool IsHalfDayPresent { get; set; }
    }

    public class StudentDetails
    {
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string EnrolmentNumber { get; set; }
        public DateTime DOB { get; set; }
        public DateTime SchoolLeftDate { get; set; }
        public char Sex { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    public class HolidayDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HolidayId { get; set; }        
    }

    public class AttendanceSummaryDetails
    {
        public int StudentId { get; set; }
        public int LastMonthCount { get; set; }
        public int CurrentMonthCount { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalPercentage { get; set; }
    }

    public class SchoolDetails
    {
        public string SchoolName { get; set; }
        public string OrgName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AcademicYear { get; set; }
    }

    public class GenderwiseAttendanceSummary
    {
        public bool IsPresent { get; set; }
        public int CategoryId { get; set; }
        public int TotalCount { get; set; }
        public char Sex { get; set; }
    }
}
