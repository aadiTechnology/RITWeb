using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class UserSurveyDetails
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string FreeTextValue { get; set; }
    }

    public class SurveyQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ParentQuestionId { get; set; }
        public bool AllowFreeText { get; set; }
        public int SurveyGroupId { get; set; }
        public int SortOrder { get; set; }
    }

    public class SurveyAnswer
    {
        public int Id { get; set; }
        public int SurveyAnswerId { get; set; }
        public string Answer { get; set; }
        public int InputControlId { get; set; }
        public int SurveyGroupId { get; set; }
    }

    public class SurveyUserDetails
    {
        public string UserName { get; set; }
        public string ParentName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ClassName { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string RegNo { get; set; }
        public int TotalRecordCount { get; set; }
        public int UserId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool AllowSubmission { get; set; }
    }

    public class SurveyHeader
    {
        public string Header { get; set; }
        public int SortOrder { get; set; }
    }

    public class SurveyConfig
    {
        public int Id { get; set; }
        public string SurveyName { get; set; }
    }
}
