using System;

namespace StaffPerformanceEntity
{
    [Serializable]
    public class StaffPerformanceEvaluation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReportingUserId { get; set; }
        public int Year { get; set; }        
    }

    [Serializable]
    public class StaffPerformanceObservation
    {
        public int Id { get; set; }
        public int StaffPerformanceEvalDetailsId { get; set; }
        public int ParameterId { get; set; }
        public int GradeId { get; set; }
        public int ReportingUserId { get; set; }
        public string Observation { get; set; }        
    }
    
    [Serializable]
    public class StaffPerformanceStatus
    {
        public int Id { get; set; }
        public int StaffPerformanceEvalDetailsId { get; set; }
        public bool IsPublished { get; set; }
        public int ReportingUserId { get; set; }
    }

    [Serializable]
    public class ReportingStaff
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public bool IsFinalApprover { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsSubmitted { get; set; }
        public int ReportingUserId { get; set; }

        public string JobStatus { get; set; }
        public string EmployeeNo { get; set; }
        public string JoiningDate { get; set; }
        public string ServiceLength { get; set; }

        public string FormFor { get; set; }
        public string Standards { get; set; }
        public string Subjects { get; set; }

        public int ApprovalSortOrder { get; set; }
        public int UserRoleId { get; set; }
        public string AcademicYear { get; set; }
        public string EffectiveFromDate { get; set; }
        public string LastIncrementDate { get; set; }
        public string AttachmentCount { get; set; }

        public string Address { get; set; }
        public string HighestEducation { get; set; }
    }

    [Serializable]
    public class FormType
    {
        public int Id { get; set; }
        public string name { get; set; }
    }

    public class ButtonState
    {
        public bool EnableSaveButton { get; set; }
        public bool EnableSubmitButton { get; set; }
        public bool EnablePublishButton { get; set; }
        public bool EnableRejectButton { get; set; }
        public bool IsPublished { get; set; }
        public bool CanUserAddComments { get; set; }
        public bool EnableApproveButton { get; set; }
        public bool IsApprover { get; set; }
    }
}
