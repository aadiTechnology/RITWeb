using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Survey
{
    class FeedbackDetails
    {
    }

    public class FeedbackGrade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int OriginalGradeId { get; set; }
        public int SortOrder { get; set; }
    }

    public class FeedbackCategory
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public int SortOrder { get; set; }
      public int OriginalCategoryId { get; set; }
      public int SchoolId { get; set; }
      public int SurveyId { get; set; }
      public int InputTypeId { get; set; }
      public bool IsEditableToAll { get; set; }
      public bool ShowNameOnReport { get; set; }
    }

    public class FeedbackParameter
    {
      public int Id { get; set; }
      public string Title { get; set; }
      public int SortOrder { get; set; }      
      public int FeedbackCategoryId { get; set; }
      public bool IsSubmitted { get; set; }
      public int SurveyId { get; set; }
      public bool IsAnswerRequired { get; set; }
      public bool AllowParameterUpdation { get; set; }
      public bool IsMandatory { get; set; }
    }

    public class SurveyFeedbackDetails
    {
      public int Id { get; set; }
      public int SurveyId { get; set; }
      public int UserId { get; set; }
      public int FeedbackParameterId { get; set; }
      public int FeedbackGradeId { get; set; }
      public string Observation { get; set; }
      public string ParameterSubject { get; set; }
    }
}
