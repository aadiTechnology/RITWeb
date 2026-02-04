using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using Utility;
using System.Xml.Serialization;

namespace ProgressReportEntities
{
    public class PrePrimaryProgressReportSubSubjects : SchoolEntity
    {
        public string SubSubjectName { get; set; }
        public string ModuleName { get; set; }
        public string SubjectName { get; set; }
        public int SubSubjectID { get; set; }
        public int ModuleID { get; set; }
        public int SubjectID { get; set; }
        public int StandardID { get; set; }
    }

    public class PrePrimaryStandards : SchoolEntity
    {
        public int StandardID { get; set; }
        public string StandardName { get; set; }
    }

    public class PrePrimaryModule : SchoolEntity
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
    }

    public class PrePrimaryProgressReportSubjects : SchoolEntity
    {
        public int PrePrimaryProgressReportSubjectID { get; set; }
        public string PrePrimaryProgressReportSubjectName { get; set; } 
    }

    public class PrePrimaryStudentsExamResult : SchoolEntity
    {
        public string PrePrimaryProgressReportSubSubjectName { get; set; }
        public string ExamModuleName { get; set; }
        public string PrePrimarySubjectName { get; set; }
        public int PrePrimaryProgressReportSubSubjectId { get; set; }
        public int ExamModuleId { get; set; }
        public int PrePrimarySubjectId { get; set; }
        public int PreprimaryExamConfigurationId { get; set; }
        public bool Is_SubjectApplicable { get; set; }
        public int PrePrimaryRemarkId { get; set; }
    }

    public class PrePrimaryConfiguredMonthDetails : SchoolEntity
    {
        public string MonthAbbreviation { get; set; }
        public int PreprimaryProgressReportMonthID { get; set; }
        public int PreprimaryExamConfigurationId { get; set; }
        public bool IsPublished { get; set; }
        public bool IsSubmitted { get; set; }
        public string RollNos { get; set; }
        public string UnpublishReason { get; set; }
        public string PreprimaryStudentWiseTestPublishStatus { get; set; }
    }

    [Serializable]
    public class PrePrimaryRemark : SchoolEntity
    {
        public int PrePrimaryProgressReportRemarkId { get; set; }
        public string PrePrimaryProgressReportRemarkName { get; set; }
        public int OriginalPrePrimaryProgressReportRemarkId { get; set; }
        //public int sortOrder { get; set; }
        public Constants.Action Action { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable]
    public class PrePrimaryProgressReportMonth : SchoolEntity
    {
        public int MonthId { get; set; }
        public string Month { get; set; }
        public int PrePrimaryProgressReportMonthId { get; set; }
        public string MonthAbbreviation { get; set; }
        public string CommentAbbreviation { get; set; }
        public int IsCommentable { get; set; }
        public int SortOrder { get; set; }
        
    }

    [Serializable]
    public class PrePrimarySubject : SchoolEntity
    {
        public int PrePrimarySubjectId { get; set; }
        public string PrePrimarySubjectName { get; set; }
        public int OriginalPrePrimarySubjectId { get; set; }
        public int ModuleId { get; set; }
        public Constants.Action Action { get; set; }
        public int IsVisibleInReport { get; set; }
        public int SortOrder { get; set; }
    }
    [Serializable]
    public class PrePrimaryStudentsExamComment : SchoolEntity
    {
        public string Header { get; set; }
        public string Comment { get; set; }
        public int Progress_Entry_Id { get; set; }
        public int MonthId { get; set; }
        public bool IsPublished { get; set; }
        public bool IsSubmitted { get; set; }
    }
    public class StudentwiseRemarkConfigDetails : SchoolEntity
    {
        public int YearwiseStudentId { get; set; }        
        public int StudentwiseRemarkId { get; set; }
        [XmlIgnore]public string StudentName { get; set; }
         public string ClassName { get; set; }
        [XmlIgnore]public int TermId { get; set; }
        public string Term { get; set; }
        public string Remark { get; set; }
        public string RemarkDetails { get; set; }
        public int RemarkConfigId { get; set; }
        [XmlIgnore]public int RollNo { get; set; }
        [XmlIgnore]public int StandardDivisionId { get; set; }
        public RemarkMaster RemarkMaster { get; set; }
        public int SalutationId { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public bool IsPassedAndPromoted { get; set; }
        public int IsLeftStudent { get; set; }
        public string OldRemark { get; set; }
    }
    public class RemarkMaster : SchoolEntity
    {
        public string RemarkName { get; set; }
        public int RemarkConfigId { get; set; }        
    }
    public class PublishExamDependencyMaster
    {
        public string DependentExamName { get; set; }
        public string ExamDependentMessage { get; set; }
    }

    public class StandardwiseRemarkLength : SchoolEntity
    {
        public int StandardwiseRemarkLengthId { get; set; }
        public int MaxRemarkLength { get; set; }
        [XmlIgnore] public int TermId { get; set; }
        public string Term { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
    }
}
