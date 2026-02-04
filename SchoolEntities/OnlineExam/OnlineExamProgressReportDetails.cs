using System.Collections.Generic;

namespace SchoolEntities.OnlineExam
{
    public class OnlineExamProgressReportDetails
    {
        public SchoolInfo SchoolInformation { get; set; }
        public List<StudentInfo> Students { get; set; }
        public List<OnlineExam> OnlineExams { get; set; }
        public List<SubjectInfo> Subjects { get; set; }
        public List<MarkInfo> MarkInformation { get; set; }
    }

    public class SchoolInfo
    {
        public string SchoolName { get; set; }
        public string OrgName { get; set; }        
    }

    public class StudentInfo
    {
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string AcademicYear { get; set; }
    }

    public class OnlineExam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OnlineExamConfigurationId { get; set; }        
    }

    public class SubjectInfo
    {
        public int SortOrder { get; set; }        
        public string Name { get; set; }
        public int SubjectId { get; set; }
    }

    public class MarkInfo
    {
        public int StudentId { get; set; }
        public int Marks { get; set; }
        public int OutOfMarks { get; set; }
        public int OnlineExamConfigurationId { get; set; }
        public int ExamId { get; set; }
        public int SubjectId { get; set; }
    }

    public class DescriptionAnswerDetails
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionAnswerId { get; set; }
        public int Marks { get; set; }
    }
}
