
namespace SchoolEntities
{
   public class ParentFeedbackQuestion
   {
       public int Id { get; set; }
       public string Title { get; set; }
       public int ParentQuestionId { get; set; }
       public int ControlId { get; set; }
       public int FeedbackId { get; set; }
   }

   public class ParentFeedbackGrade
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public int SortOrder { get; set; }
   }

   public class ParentFeedbackDetails
   {
       public int Id { get; set; }
       public int UserId { get; set; }
       public int QuestionId { get; set; }
       public int GradeId { get; set; }
       public string Remark { get; set; }
   }
}
