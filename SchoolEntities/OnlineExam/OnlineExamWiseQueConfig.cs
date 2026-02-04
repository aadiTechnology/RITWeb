using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
  public  class OnlineExamWiseQueConfig
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
        public string Class { get; set;}
        public string Exam { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }

    }

  public class OnlineExamQuestConfig
  {
      public int Id { get; set; }
      public int StandardDivisionId { get; set; }
      public int SubjectId { get; set; }
      public string Question { get; set; }
      public int OutOfMarks { get; set; }
      public int SchoolId { get; set; }
      public int AcademicYearId { get; set; }
      public bool IsDeleted { get; set; }
      public DateTime InsertDate { get; set; }
      public int InsertedById { get; set; }
      public DateTime UpdateDate { get; set; }
      public int UpdatedById { get; set; }
      public int QuestionId { get; set; }
      public int AnswerTypeId { get; set; }
      public string Answer { get; set; }
      public bool IsCorrectAnswer { get; set; }
      public string Subject { get; set; }
      public string Class { get; set; }
      public int DisplayOrder { get; set; }
  }

  public class QuestionDetails
  {
      public int QuestionId { get; set; }
      public string Question { get; set; }
      public int SerialNo { get; set; }
      public int Marks { get; set; }
      public bool IsExamSaved { get; set; }
      public bool IsExamSubmited { get; set; }
      public int AnswerTypeId { get; set; }
      public string AttachmentPath { get; set; }
  }

  public class AnswerDetails
  {
      public int AnswerId { get; set; }
      public int QuestionID { get; set; }
      public string Answer { get; set; }
      public int DisplayOrder { get; set; }
      public bool IsCorrectAnswer { get; set; }
      public int UserSelectedAnswer { get; set; }
      public string AttachmentPath { get; set; }
      public string DescriptionFileName { get; set; }
  }

  public class StudentQuestionAnswerDetails
  {
      public int QuestionId { get; set; }
      public int AnswerId { get; set; }
      public string DescriptionFileName { get; set; }
  }
}
