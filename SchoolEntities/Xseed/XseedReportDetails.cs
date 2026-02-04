/* Class Name :- XseedReportDetails.cs
 * Created By :- Shobha
 * Created Date :- 01-Jun-2011
 * Description :- This class is used create basic objects related to XseedReport.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using Utility;
using SchoolEntities;

namespace XseedReportEntities
{
    public class ExamResult : SchoolEntity
    {
        public int YearwiseStudentId { get; set; }
        public int AssessmentId { get; set; }
        public string Observation { get; set; }
        public string SubjectRemark { get; set; }
        public int StandardDivisionId { get; set; }
        public bool ShowSubjectRemark { get; set; }
    }
    public class AssessmentMaster : SchoolEntity
    {
        public int StandardwiseAssessmentId { get; set; }
        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Constants.Action Action { get; set; }
    }
    [Serializable]
    public class GradeSubmitStatus : SchoolEntity
    {
        public int GradeSubmitStatusId { get; set; }
        public int StandardDivisionId { get; set; }
        public int AssessmentId { get; set; }
        public int SubjectId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsPublished { get; set; }
        public int SubjectSectionConfigId { get; set; }
    }

    public class LearningOutcomeConfigMaster : SchoolEntity
    {
        public int LearningOutcomeConfigId { get; set; }
        public int StandardwiseAssessmentId { get; set; }
        public int SubjectSectionConfigId { get; set; }
        public string LearningOutCome { get; set; }
        public int GradeId { get; set; }
        public int LearningOutcomeGradeId { get; set; }
        public int SortOrder { get; set; }
        public bool IsConsidered { get; set; }
        public bool IsSubmitted { get; set; }
        public int StandardId { get; set; }
        public int SubjectSectionConfigurationId { get; set; }
        public int SubjectId { get; set; }
    }

    public class LearningOutcomesGrade : SchoolEntity
    {
        public int LearningOutcomeGradeId { get; set; }
        public int YearwiseStudentId { get; set; }
        public int LearningOutcomeConfigId { get; set; }
        public int GradeId { get; set; }
        public string LearningOutcomeXml { get; set; }
    }

    public class LearningOutcomesObservation : ExamResult
    {
        public int SubjectSectionConfigurationId { get; set; }
        public int LearningOutcomesObservationId { get; set; }
    }

    public class LearningOutcomesSubmitStatus : SchoolEntity
    {
        public int LearnOutcomeSubmitStatusId { get; set; }
        public int StandardwiseAssessmentId { get; set; }
        public int SubjectId { get; set; }
        public bool IsSubmitted { get; set; }
    }

    public class NonXseedSubjectGrades : ExamResult
    {
        public int NonXseedSubjectGradeId { get; set; }
        public int SubjectId { get; set; }
        public int GradeId { get; set; }
        [XmlIgnore]
        public string SubjectName { get; set; }
        [XmlIgnore]
        public string ShortName { get; set; }
        [XmlIgnore]public bool IsCoCurricularActivity { get; set; }
    }
    [Serializable]
    public class StandardwiseAssessmentMaster : SchoolEntity
    {
        public int StandardwiseAssessmentId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public int StandardId { get; set; }
        public int SortOrder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IsFinalAssessment { get; set; }
        public string IsDeleted { get; set; }
        public Constants.Action Action { get; set; }
        public string StandardName { get; set; }
    }

    public class SubjectSectionConfigurationMaster : SchoolEntity
    {
        public int SubjectSectionConfigurationId { get; set; }
        [XmlIgnore]
        public int StandardwiseSubjectId { get; set; }
        public int OrginalSubjectSectionId { get; set; }
        public string SubjectSectionName { get; set; }
        public int SortOrder { get; set; }
        public int SubjectId { get; set; }
        public Constants.Action Action { get; set; }        
        public bool ShowSubjectRemarks { get; set; }
    }
    [Serializable]
    public class GradeMaster : SchoolEntity
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int OriginalGradeId { get; set; }
		public int SchoolId { get; set; }
        public string IsDeleted { get; set; }
        public bool ConsideredAsAbsent { get; set; }
        public bool ConsideredAsExempted { get; set; }
        public int SortOrder { get; set; }
        public Constants.Action Action { get; set; }
    }

    public class YearwiseStudentMaster
    {
        public int YearwiseStudentId { get; set; }
        public string StudentName { get; set; }
        public int RollNo { get; set; }
        public string Class { get; set; }
        public string AcademicYear { get; set; }
        public string Assessment { get; set; }
        public int AssessmentDays { get; set; }
    }

    public class XseedGradesStatus : SchoolEntity
    {
        public int StandardDivisionID { get; set; }
        public string Class { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int EditStatus { get; set; }
        public int SubmitStatus { get; set; }
        public char IsXseedSubject { get; set; }
        public char IsSubmitted { get; set; }
		public string IncompleteRollNo { get; set; }
    }

    public class StudentXseedGradeDetails : SchoolEntity
    {
        public int YaerwiseStudentId { get; set; }
        public int RollNumber { get; set; }
        public string StudentName { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Observations { get; set; }
    }
    public class SchoolwiseAcademicYrDates
    {
        public DateTime AcademicYearStartDate { get; set; }
        public DateTime AcademicYearEndDate { get; set; }
    }

    //public class StandardMaster
    //{
    //    public int StandardId { get; set; }
    //    public string StandardName { get; set; }
    //}

    public class SubjectMaster
    {
        public int StandardwiseSubjectId { get; set; }
        public string SubjectName { get; set; }
    }
    public class XseedResultPublishStatus
    {
        public int StandardDivisionId { get; set; }
        public char PublishStatus { get; set; }
        public char IsPublished { get; set; }
    }

    [Serializable]
    public class ClassTeacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
		public bool IsClassTeacher { get; set; }
		public string ClassName { get; set; }
    }
    public class StudentAttendance
    {
        public int YearwiseStudentId { get; set; }
        public bool IsPresent { get; set; }
    }
    public class XseedTheme
    { 
      public int StandardwiseAssessmentId{get;set;}
      public string Theme {get;set;}
      public int SortOrder {get;set;}
      public string Is_Deleted {get;set;}
      public int ThemeId { get; set; }
      public string StandardName { get; set; }
      public string AssessmentName { get; set; }
      public int StandardId { get; set; }
      public int AssessmentId { get; set; } 
    }

    public class XseedRemark
    {
        public int YearwiseStudentId { get; set; }
        public string Remark { get; set; }
    }

    public class SubjectRemark
    {
        public int SubjectId { get; set; }
        public string Remark { get; set; }
    }

}
