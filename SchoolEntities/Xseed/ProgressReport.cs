using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XseedReportEntities
{
    public class ProgressReport
    {
    }

    public class StandardwiseSubject
    {
        public int StandardDivisionId { get; set; }
        public int StandardSubjectId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int SortOrder { get; set; }        
    }

    public class StudentsLearningOutcome
    {
        public int YearwiseStudentId { get; set; }
        public int LearningOutcomeConfigId { get; set; }
        public int SubjectSectionConfigId { get; set; }
        public string LearningOutcome { get; set; }
        public int GradeId { get; set; }
        public string ShortName { get; set; }
        public int SubjectSectionSortOrder { get; set; }
        public int LearningOutcomeSortOrder { get; set; }
        public int LearningOutcomeGradeId { get; set; }
    }

    public class ClassTeacherDetails
    {
        public int TeacherId { get; set; }
        public int StandardDivisionId { get; set; }
        public string TeacherName { get; set; }
    }
}
