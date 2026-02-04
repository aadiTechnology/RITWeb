using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
   public class PublishOnlineExam
    {
        public int Id { get; set; }
        public int StandardDivisionId { get; set; }
        public int SubjectId { get; set; }
        public int ExamId { get; set; }
        public string Class { get; set; }
    }

   public class OnlineExamResult
   {
       public string Question { get; set; }
       public string Answer { get; set; }
       public int SortOrder { get; set; }
       public int StudentId { get; set; }
       public bool IsCorrectAnswer { get; set; }
       public int AnswerTypeId { get; set; }
       public string QuestionAttachmentPath { get; set; }
       public string AnswernAttachmentPath { get; set; }
   }

   public class OnlineExamStatus
   {
       public string Subject { get; set; }
       public int Present { get; set; }
       public int Absent { get; set; }
       public int SubjectId { get; set; }
       public bool IsPublished { get; set; }
       public int AnswerTypeId { get; set; }
   }
}
