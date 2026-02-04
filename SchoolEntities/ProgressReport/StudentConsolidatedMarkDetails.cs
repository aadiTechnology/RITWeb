using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.ProgressReport
{
    public class StudentConsolidatedMarkDetails
    {
        public List<Mark> Marks { get; set; }
        public List<ExamConfig> ExamConfigs { get; set; }
        public List<StudentInfo> Students { get; set; }
        public List<ExamStatusConfig> ExamStatusConfigs { get; set; }
    }

    public class Mark
    {
        public int TestWiseSubjectMarksId { get; set; }
        public int StudentId { get; set; }
        public int TotalMarksScored { get; set; }
        public int SubjectTotalMarks { get; set; }
        public int SchoolWiseTestId { get; set; }
        public int SubjectId { get; set; }
        public char IsAbsent { get; set; }
    }

    public class ExamConfig
    {
        public int TestWiseSubjectMarksId { get; set; }
        public int SchoolWiseTestId { get; set; }
        public int SubjectId { get; set; }
        public int SubjectTotalMarks { get; set; }
        public int SubjectSortOrder { get; set; }
        public int TestSortOrder { get; set; }
        public string SubjectName { get; set; }
        public string SchoolWiseTestName { get; set; }
    }

    public class Test
    {
        public int TestId { get; set; }
        public string Name { get; set; }
    }

    public class StudentInfo
    {
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string Name { get; set; }
    }

    public class ExamStatusConfig
    {
        public string ShortName { get; set; }
        public string ConsiderInTotal { get; set; }
        public string DisplayTotal { get; set; }
        public string DisplayValue { get; set; }
    }
}

