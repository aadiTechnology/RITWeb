using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace LessonPlanEntities
{
    public class LessonPlanParameters :SchoolEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
        public int LessonPlanCategoryId { get; set; }
        public int LessonPlanSectionId { get; set; }
        public bool IsSubmitted { get; set; }
        public int SubjectCategoryId { get; set; }
        public string SubjectCategoryName { get; set; }
        public int ParentParameterId { get; set; }
        public string ParentParameter { get; set; }
    }

    public class LessonPlanConfig : SchoolEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StdDivId { get; set; }
        public int SubjectId { get; set; }
        public bool IsSubmitted { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public int LessonPlanCategoryId { get; set; }
        public int LessonPlanSectionId { get; set; }
        public int UserId { get; set; }
        public string Remarks { get; set; }
        public bool IsSuggestionAdded { get; set; }
        public bool IsSuggestionRead { get; set; }
        public bool IsSubmitedByReportingUser { get; set; }
        public int StandardId { get; set; }
        public int SubjectCategoryId { get; set; }
        public int ParentParameterId { get; set; }
        public string ParentParameter { get; set; }
    }

    public class LessonPlanBasicDetails
    {
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
    }

    public class LessonPlanStatus : SchoolEntity
    {
        public int Id { get; set; }
        public int LessionPlanConfigId { get; set; }
        public int UserId { get; set; }
        public int ReportingUserId { get; set; }
        public bool IsPublished { get; set; }
    }

    public class LessonPlanStandardDivIds : SchoolEntity
    {
        public string StandardDivisionIds { get; set; }
    }

    [Serializable]
    public class LessonPlanReportingConfig : SchoolEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ReportingUserId { get; set; }
        public string ReportingUserName { get; set; }
        public bool IsFinalApprover { get; set; }        
        public int ApprovalSortOrder { get; set; }
        public bool IsSubmitted { get; set; }
        public int LessonPlanConfigId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPublished { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }       
    }

    public class LessonPlanDetails : SchoolEntity
    {
        public int Id { get; set; }
        public int ReportingUserId { get; set; }
        public int ParameterId { get; set; }
        public string Comment { get; set; }
        public int StdDivId { get; set; }
        public int SubjectId { get; set; }
        public int LessonPlanCategoryId { get; set; }
        public int LessonPlanSectionId { get; set; }
        public int SubjectCategoryId { get; set; }
        public string SubjectStartDate { get; set; }
        public string SubjectEndDate { get; set; }
    }

    public class TeacherDetails
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class ClassSubjectDetails
    {
        public int StdDivId { get; set; }
        public string ClassName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int LessonPlanCategoryId { get; set; }
    }

    public class ApproverComment
    {
        public int ReportingUserId { get; set; }
        public string Comment { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsPublished { get; set; }
        public int LessonPlanXMLId { get; set; }
        public bool IsReportingUser { get; set; }
    }

    public class LessonPlanCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
    }

    public class LessonPlanStdSubject
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
    public class LessonSubjectCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LessonPlanPhrase
    {
        public string Title { get; set; }
        public bool IsPhrase { get; set; }
    }
}
