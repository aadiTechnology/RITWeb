using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
  public  class OnlineExamConfiguration
    {
        public int Id { get; set; }
        public int StandardDivisionId { get; set; }
        public int SubjectId { get; set; }
        public int ExamId { get; set; }
        public DateTime StartDateAndTime { get; set; }
        public DateTime EndDateAndTime { get; set; }
        public bool ShuffleForCount { get; set; }
        public int NoOfQuestions { get; set; }
        public bool ShuffleForSequence { get; set; }
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertedById { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedById { get; set; }
        public int QuestionId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ExamConfigurationId { get; set; }
        public string Class { get; set; }
        public string Exam { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public int TotalRows { get; set; }
        public bool IsSubmitted { get; set; }
    }
}
