using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StudentRecord
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public string Answer { get; set; }
        public int SchoolwiseStudentId { get; set; }
    }

    public class StudentBasicInfo
    {
        public string StudentName { get; set; }
        public DateTime DOB { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherOccupation { get; set; }
        public string MotherOccupation { get; set; }
    }

    public class StudentRecordSibling
    {
        public string SiblingName { get; set; }
        public char Sex { get; set; }
        public int Age { get; set; }
        public string Standard { get; set; }
    }

    public class StudentRecordSection
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public bool DisplayOnScreen {get;set;}
        public int SortOrder {get;set;}
    }

    public class StudentRecordParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }
        public int SortOrder { get; set; }
        public int ControlId { get; set; }
    }

    public class StudentDataCollction
    {
        public List<StudentRecord> StudentRecords { get; set; }
        public StudentBasicInfo StudentBasicInformation { get; set; }
        public List<StudentRecordSibling> StudentRecordSiblings { get; set; }
        public List<StudentRecordSection> StudentRecordSections { get; set; }
        public List<StudentRecordParameter> StudentRecordParameters { get; set; }
        public List<StudentRecordComment> StudentRecordComments { get; set; }
    }

    public class StudentRecordComment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string LectureName { get; set; }        
        public bool IsDefaultComment { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsCommentReadByConsellor { get; set; }
        public bool IsCommentReadByPrincipal { get; set; }
        public bool IsCommentReadByClassTeacher { get; set; }
        public int LoginUserDesignation { get; set; }
        public int InsertedById { get; set; }
        public string UserName { get; set; }
    }

    public class StudentRecordStatus
    {
        public bool IsRecordFound { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public string RegNo { get; set; }
        public int RollNo { get; set; }
        public string Class { get; set; }
        public string Name { get; set; }
        public int TotalRows { get; set; }
        public bool IsReadByPrincipal { get; set; }
        public bool IsReadByCounsellor { get; set; }
        public bool IsSubmitted { get; set; }
        public int PrincipalCommentCount { get; set; }
        public int CouncellorCommentCount { get; set; }
        public int ReadyToReadCount { get; set; }
        public int ReadyToSubmitCount { get; set; }
    }

    public class AssociatedTeacher
    {
        public int StdDivId { get; set; }
        public string TeacherName { get; set; }
    }
}

