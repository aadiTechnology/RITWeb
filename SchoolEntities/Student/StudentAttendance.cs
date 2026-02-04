using System;
using System.Collections.Generic;

namespace AttendanceReportEntity
{
    public class StudentAttendanceReport
    {
        public List<AttendanceDetails> AttendanceDetails { get; set; }
        public List<StudentInfo> StudentDetails { get; set; }
    }

    public class AttendanceDetails
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public string IsPresent { get; set; }
        public int LectureNo { get; set; }
    }

    public class StudentInfo
    {
        public int YearWiseStudentId { get; set; }
        public string EnrolmentNumber { get; set; }
        public string StudentName { get; set; }
        public string className { get; set; }
        public int RollNo { get; set; }
        public string TermCount { get; set; }
        public decimal TermPercentage { get; set; }
    }
}
