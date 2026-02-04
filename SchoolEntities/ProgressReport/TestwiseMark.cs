using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.ProgressReport
{
    public class TestwiseMark
    {
        public List<StudentDetailsForTestReport> StudentDetails { get; set; }
        public List<MarkDetails> Marks { get; set; }
        public List<SubjectForTestReport> Subjects { get; set; }
        public List<TestForTestReport> Exams { get; set; }
        public List<GradeDetailsForTestReport> Grades { get; set; }
        public List<TestSummaryDetails> MarkSummary { get; set; }
        public ClassDetails OtherDetails { get; set; }
        public List<TestTypeMarks> TestTypeMarkDetails { get; set; }
    }

    public class StudentDetailsForTestReport
    {
        public int YearWiseStudentId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }        
    }


    public class MarkDetails
    {
        public int SchoolWiseStudentTestMarksId { get; set; }
        public int StudentId {get;set;}
        public int SchoolWiseTestId {get;set;}
        public int SubjectId { get; set; }
        public decimal TotalMarksScored {get;set;}                
        public string IsAbsent { get; set; }
        public decimal Percentage { get; set; }
    }

    public class SubjectForTestReport
    {
        public int SubjectId { get; set; }
        public int SortOrder { get; set; }
        public string SubjectName { get; set; }
        public bool IsGradeApplicable { get; set; }
        public int ParentSubjectId { get; set; }
        public string ParentSubjectName { get; set; }
        public int SubjectTotalMarks { get; set; }
    }

    public class TestForTestReport
    {
        public int SchoolWiseTestId { get; set; }
        public int SortOrder { get; set; }
        public string SchoolWiseTestName { get; set; }
    }

    public class GradeDetailsForTestReport
    {
        public int StartingMarkRange { get; set; }
        public decimal EndingMarkRange { get; set; }
        public string GradeName { get; set; }
    }

    public class ClassDetails
    {
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public string SchoolName { get; set; }
        public string TestName { get; set; }
        public bool DisplaySubTypes { get; set; }
    }

    public class TestSummaryDetails
    {
        public int StudentId { get; set; }
        public int TotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public int Rank { get; set; }
    }

    public class TestTypeMarks
    {
        public int SchoolWiseStudentTestMarksId { get; set; }
        public decimal Marks_Scored { get; set; }
        public int OutOfMarks { get; set; }
        public string IsAbsent { get; set; }
        public int TestTypeId { get; set; }
        public string TestTypeName { get; set; }
        public int SortOrder { get; set; }       
    }
}
