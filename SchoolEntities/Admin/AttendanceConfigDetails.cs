// -----------------------------------------------------------------------
// <copyright file="AttendanceConfigDetails.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttendanceAlertConfigDetails
    {
        public int ConfigId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public Int16 NoOfDays { get; set; }             
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public int InsertedById { get; set; }
        public string UserName { get; set; }
    }

    public class ClasswiseAttendanceStatus
    {
        public int SchoolWiseStandardDivisionId{get;set;}
		public int StandardId{get;set;}
		public string StandardName{get;set;}
		public int DivisionId{get;set;}
		public string DivisionName{get;set;}
        public Int16 AttendanceTaken { get; set; }
        public string HolidayName { get; set; }
		public string PresentStudentWithTotal { get; set; }
    }

    public class DayDetails
    {
        public string HolidayName { get; set; }
        public string IsWeekDay { get; set; }
        public string OutSideAcademicYear { get; set; }
    }

    public class AttendanceAlertDetails
    {
        public int StandardDivisionId { get; set; }
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public int MissingCount { get; set; }
    }

    public class AbsentStudentDetails
    {
        public string EnrolmentNumber { get; set; }
        public int RollNo { get; set; }
        public string className { get; set; }
        public string StudentName { get; set; }
    }
}
