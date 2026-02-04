/* Class Name :- StudentProgressReport.cs
 * Created By :- Vipul
 * Created Date :- 03-Feb-2013
 * Description :- These are the classes used to display student's progress report.
*/

using System;
using System.Collections.Generic;
using MasterEntities;
using Utility;
using SchoolEntities;
using System.Xml.Serialization;

namespace ProgressReportEntities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StudentProgressReport
    {
        public StudentDetails StudentDetails { get; set; }
        public List<Subject> SubjectDetails { get; set; }
        public List<Exam> ExamDetails { get; set; }
        public List<MarkAssignment> MarkAssignmentDetails { get; set; }
        public List<ExamWisePercentage> ExamWisePercentageDetails { get; set; }
        public List<SubjectTestGroupTotal> SubjectTestGroupTotalDetails { get; set; }
        public List<SubjectTestTypeGroupTotal> SubjectTestTypeGroupTotalDetails { get; set; }
        public List<SubjectTestType> SubjectTestTypeDetails { get; set; }
        public List<TestType> TestTypeDetails { get; set; }
        public List<Grade> GradeDetails { get; set; }
        public List<ExamStatus> ExamStatusDetails { get; set; }
        public List<DependentExam> DependentExamDetails { get; set; }
        public int GraceMarks { get; set; }
        public string GraceMarksMessage { get; set; }
    }

    public class StudentDetails
    {
        public int YearWiseStudentId { get; set; }
        public string StudentName { get; set; }
        public StandardDivisionMaster StandardDivisionDetails { get; set; }
        public string AcademicYear { get; set; }
        public int RollNo { get; set; }
        public string EnrolmentNumber { get; set; }
        public string SchoolName { get; set; }
        public string OrganizationName { get; set; }
        public bool ShowOnlyGrades { get; set; }
        public string IsFailCriteriaNotApplicable { get; set; }
    }

    public class StudentWiseProgressReportStudentDetails : StudentDetails
    {
        public bool IsGradesStandard { get; set; }
        public int UserId { get; set; }   
    }
    
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int ParentSubjectId { get; set; }
        public string TotalConsideration { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsThirdLanguage { get; set; }
    }

    public class ProgressReportSubujectDetails : Subject
    {
        public int SortOrder { get; set; }
    }


    public class FinalResultSubjectDetails : Subject
    {
        public int GraceMarks { get; set; }
        public decimal MarksScored { get; set; }
        public int SubjectTotalMarks { get; set; }
        public string Grade { get; set; }
        public string GradeOrMarks { get; set; }
        public int SubjectMaxGrace { get; set; }
        public int StandardMaxGrace { get; set; }   
    }
    
    public class Exam 
    {
        public int SchoolWiseTestId { get; set; }
        public string SchoolWiseTestName { get; set; }
        public int OriginalShcoolWiseTestId { get; set; }
    }

    public class MarkAssignment
    {
        public int SubjectId { get; set; }
        public string Marks { get; set; }
        public int SchoolWiseTestId { get; set; }
        public int OriginalShcoolWiseTestId { get; set; }
        public string SchoolWiseTestName { get; set; }
        public string SubjectName { get; set; }
        public decimal TotalMarksScored { get; set; }
        public int SubjectTotalMarks { get; set; }
        public decimal PassingTotalMarks { get; set; }
        public string SubjectTotal { get; set; }
        public string GradeOrMarks { get; set; }
        public int TestTypeId { get; set; }
        public decimal MarksScored { get; set; }
        public string TestTypeName { get; set; }
        public string ShortenTestTypeName { get; set; }
        public string Grade { get; set; }
        public string TotalGrade { get; set; }
        public int TestTypeTotalMarks { get; set; }
        public decimal TestTypePassingMarks { get; set; }
        public string IsAbsent { get; set; }
        public int SchoolWiseStudentTestId { get; set; }
        public int TestWiseSubjectId { get; set; }
        public string ConsiderExamStatus { get; set; }
        public string ConsiderInResult { get; set; }
        public bool ShowOnlyGrades { get; set; }
        public bool AllowDecimal { get; set; }
        public bool IsCoCurricularActivity { get; set; }
        public bool IsActivitySubject { get; set; }

    }

    public class StudentWiseProgressReportMarkAssignment : MarkAssignment
    {
        //public DateTime ActualTestDate { get; set; }
        //public DateTime JoiningDate { get; set; }
        public DateTime TestDate { get; set; }
        public int TestOutOfMarks { get; set; }
        public int TestTypeOutOfMarks { get; set; }
        public string TotalConsideration { get; set; }
        public bool IsExamStatusApplicable { get; set; }
        public string StudentWiseTestPublishStatus { get; set; }
        public string ExamPublishStatus { get; set; }
    }
    
    public class ExamWisePercentage
    {
        public int SchoolWiseTestId { get; set; }
        public decimal TotalMarksScored { get; set; }
        public int SubjectTotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public string Grade { get; set; }
        public int GradeId { get; set; }
        public string Result { get; set; }
        public int Rank { get; set; }
    }

    public class FinalResultExamWisePercentage : ExamWisePercentage
    {
        public int StudentId { get; set; }   
    }

    public class StudentWiseProgressReportExamWisePercentage : ExamWisePercentage
    {
        public string StudentWiseTestPublishStatus { get; set; }
        public string ExamPublishStatus { get; set; }
        public string ExamSubmitStatus { get; set; }
        public string SchoolWiseTestName { get; set; }
    }
    
    public class SubjectTestGroupTotal
    {
        public int ParentSubjectId { get; set; }
        public string ParentSubjectName { get; set; }
        public decimal TotalMarksScored { get; set; }
    }

    public class StudentWiseProgressReportSubjectTestGroupTotal : SubjectTestGroupTotal
    {
        public decimal ChildSubjectMarksTotal { get; set; }
        public int SchoolWiseTestId { get; set; }
        public string Grade { get; set; }
        public decimal AverageMarks { get; set; }
        public decimal OutOfMarks { get; set; }
    }

    public class FinalResultSubjectTestGroupTotal : SubjectTestGroupTotal
    {
        public int OriginalSubjectId { get; set; }
        public int GraceMarks { get; set; }
        public string GradeOrMarks { get; set; }
        public int SubjectMaxGrace { get; set; }
        public int StandardMaxGrace { get; set; }
        public int SubjectTotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public decimal AverageMarks { get; set; }
        public decimal OutOfMarks { get; set; }
    }

    public class SubjectTestTypeGroupTotal
    {
        public int SchoolWiseTestId { get; set; }
        public int TestTypeId { get; set; }
        public int TestTypeSortOrder { get; set; }
        public int ParentSubjectId { get; set; }
        public decimal TestTypeTotalMarksScored { get; set; }
        public decimal TestTypeTotalMarks { get; set; }
        public string Grade { get; set; }
    }
    
    public class SubjectTestType
    {
        public int SubjectId { get; set; }
        public int TestTypeId { get; set; }
        public string ShortenTestTypeName { get; set; }
        public decimal TotalMarksScored { get; set; }
        public int TestTypeSortOrder { get; set; }
    }
    
    public class TestType
    {
        public int TestTypeId { get; set; }
        public string TestTypeName { get; set; }
        public string ShortenTestTypeName { get; set; }
        public int TestTypeSortOrder { get; set; }
    }
    
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Remarks { get; set; }
        public bool IsForCoCurricularSubjects { get; set; }
        public bool IsActivitySubject { get; set; }
    }

    public class StudentWiseProgressReportGrade : Grade
    {
        public int StartingMarksRange { get; set; }
        public decimal ActualEndingMarksRange { get; set; }
    }

    public class FinalResultGrade : Grade
    {
        public string Range { get; set; }
    }
    
    public class ExamStatus
    {
        public string DisplayName { get; set; }
        public string DisplayValue { get; set; }
        public string ShortName { get; set; }
        public string ForeColor { get; set; }
        public string BackColor { get; set; }
        public string ConsiderInTotal { get; set; }
    }

    public class SubjectDetailsForProgressReport
    {
        public int CellIndex { get; set; }
        public string Subjectname { get; set; }
        public int SubjectId { get; set; }
        public int ParentSubjectId { get; set; }
        public int SubjectCellColSpan { get; set; }
        public int SubjectCellRowSpan { get; set; }
        public string TestTypeName { get; set; }
        public bool IsConsideredInTotal { get; set; }
        public Constants.ReportCellType SubjectCellType { get; set; }
    }

    public class DependentExam
    {
        public int ParentExamId { get; set; }
        public string ExamName { get; set; }
        public int DependentExamId { get; set; }
    }

	public  class BlockStudentsProgressReportDetails : SchoolEntity
	{
				public int YearwiseStudentId { get; set; }
	 [XmlIgnore]public int RollNo { get; set; }
	 [XmlIgnore]public string StudentName { get; set; }
				public string Reason { get; set; }
	 [XmlIgnore]public bool HasFeesPending { get; set; }
				
				
	}
}