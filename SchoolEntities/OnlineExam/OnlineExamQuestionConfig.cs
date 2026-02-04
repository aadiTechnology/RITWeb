using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
   public class OnlineExamQuestionConfig
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
        public string CorrectAnswer { get; set; }
        public bool IsSubmitted { get; set; }
        public int TotalRows { get; set; }
        public string QuestionFilePath { get; set; }
        public string AnswerFilePath { get; set; }
    }
   public class OnlineExamAnswer
   {
       public string Answer { get; set; }
       public bool IsCorrectAnswer { get; set; }
       public int DisplayOrder { get; set; }
       public string AnswerFilePath { get; set; }
   }


   public class ButtonStateDetails
   {
       public bool EnableSubmitButtton { get; set; }
       public bool EnableUnSubmitButtton { get; set; }
   }
}
