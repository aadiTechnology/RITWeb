using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class AssemblyDetails
    {
        public int Id { get; set; }
        public int AssemblyId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string FreeTextValue { get; set; }
        public string PhotoFilePath { get; set; }
        public string AssemblyPhoto { get; set; }
        public DateTime Date { get; set; }
        public bool IsSubmit { get; set; }
        public bool IsPublish { get; set; }
    }

    public class AssemblyQuestions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public int SortOrder { get; set; }
        public int ParentQuestionId { get; set; }
        public bool AllowFreeText { get; set; }
    }
    [Serializable]
    public class AssemblyAnswers
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public int AnswerGroupId { get; set; }
        public int InputControlId { get; set; }
    }

    public class StandardDetails
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
    }

    public class AssemblyQuestionConfiguration
    {
        public int AssemblyId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int parentQuestionId { get; set; }
        public string PArentQuestion { get; set; }
        public bool IsFreeTextAllow { get; set; }
    }
}
