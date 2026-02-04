using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StudentMarkDetails
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int OutOfMarks { get; set; }
        public decimal ScoredMarks { get; set; }
        public string Grade { get; set; }
        public string ExamStatus { get; set; }
        public int SchoolwiseTestId { get; set; }        
    }

    public class TestDetails
    {
        public int SchoolwiseTestId { get; set; }
        public string TestName { get; set; }
        public int TestSortOrder { get; set; }
        public int OutOfMarks { get; set; }
        public int GroupSortOrder { get; set; }
        public int TermId { get; set; }
    }

    public class SubjectInfo
    {
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int SortOrder { get; set; }
        public string ParentSubject { get; set; }
        public bool IsCoCurricularSubject { get; set; }
        public bool IsGradingSubject { get; set; }
    }

    public class StudentInfoForExam
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string HouseName { get; set; }
        public int RollNo { get; set; }
        public int OriginalDivisionId { get; set; }
    }

    public class StudentMarkSummary
    {
        public int StudentId { get; set; }
        public int OutOfMarks { get; set; }
        public decimal TotalScoredMarks { get; set; }
        public decimal Percentage { get; set; }
        public int Rank { get; set; }
        public string Grade { get; set; }
    }

    public class BasicInfo
    {
        public string OrgName { get; set; }
        public string SchoolName { get; set; }
        public string Location { get; set; }
        public string TestName { get; set; }
        public string AcademicYear { get; set; }
        public string ClassName { get; set; }
        public bool ShowGrades { get; set; }
        public string PrincipalName { get; set; }
        public string ClassTeacherName { get; set; }
    }
}
