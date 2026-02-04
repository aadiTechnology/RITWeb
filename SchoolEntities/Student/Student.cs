using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace SchoolEntities
{
    public class Student
    {
        public int StudentId { get; set; }
        public int StandardId { get; set; }
        public int DivisionId { get; set; }
        public int StdDivId { get; set; }
        public string RegistraionNo { get; set; }
        public string Name { get; set; }
        public string NameFL { get; set; }
        public bool IsLeft { get; set; }
        public Constants.Action Action { get; set; }
        public int YearWiseStudentId { get; set; }
        public string ClassName { get; set; }
        public int OriginalStandardId { get; set; }
        public int OriginalDivisionId { get; set; }
        public int RollNo { get; set; }
        public char Gender { get; set; }
        public bool IsPublished { get; set; }
        public string MobileNumber { get; set; }
        public int TotalPayable { get; set; }
        public int UserId { get; set; }
    }

    public class Staff
    {
        public int UserId { get; set; }
        public int UserRoleId { get; set; }        
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Constants.Action Action { get; set; }
        public string NameFL { get; set; }
        public int StatusId { get; set; }
    }

    public class AutoSearchUser
    {
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string Name { get; set; }
        public string NameFL { get; set; }
        public int StdDivId { get; set; }
        public bool HasFullAccess { get; set; }
        public bool IsCoordinator { get; set; }
    }

    public class TeacherClassAsso
    {
        public int TeacherUserId { get; set; }
        public int StdDivId { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string NameFL { get; set; }
    }

    public class StandardwiseStudentCount
    {
        public int Year { get; set; }
        public int MonthId { get; set; }
        public int StandardId { get; set; }
        public bool IsNewStudent { get; set; }
        public bool IsRteStudent { get; set; }
        public char Sex { get; set; }
        public int StudentCount { get; set; }
        public DateTime Date { get; set; }
        public string Header { get; set; }
        public string StandardName { get; set; }
        public bool IsStartingCount { get; set; }
        public bool IsStudentRepeatingClass { get; set; }
    }

    public class StudentsBulkEmail
    {
        public int StudentId { get; set; }
        public string RegNo { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public int StandardId { get; set; }
        public int DivisionId { get; set; }
        public string EmailAddress { get; set; }
    }

    public class StudentPhoto
    {
        public int SchoolwiseStudentId { get; set; }
        public string RegNo { get; set; }
        public byte[] PhotoInBinary { get; set; }
    }
}
