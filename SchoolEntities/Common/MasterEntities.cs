using System;

namespace MasterEntities
{
	[Serializable]
    public class DesignationMaster
    {
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public int SortOrder { get; set; }
        public bool HasAccountAccess { get; set; }
        // User Role Id
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
    }
    
	public class StandardMaster
    {
        public int StandardId { get; set; }
        public string StandardName { get; set;}
        public string Is_Deleted { get; set; }
        public int Original_Standard_Id { get; set;}
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public bool IsForGrading { get; set; }
    }
    
	[Serializable]
    public class SubjectMaster
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set;}
        public int Original_Subject_Id { get; set;}
        public string Is_Deleted { get; set;}
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public int LanguageGroupId { get; set; }
        public int SubjectGroupId { get; set; }
        public int SecondThirdId { get; set; }
    }
    
	[Serializable]
    public class StandardDivisionMaster
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int StandardDivisionId { get; set; }
        public bool IsPreprimaryStandard { get; set; }
    }

    public class UserSMS
    {
        public int UserId { get; set; }
        public string UserLogin { get; set;}
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string UserPassword { get; set; }
    }

    [Serializable]
    public class MonthMaster
    {
        public int MonthId { get; set; }
        public string Month { get; set; }
        public string MonthAbbreviation { get; set; }
    }

    [Serializable]
    public class PhotoMaster
    {
        public int UserId { get; set; }
        public byte[] TotalBytes { get; set; }        
    }

	public class UserRoleMaster
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class Qualification
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

    public class NonPermanentTeacherDetails 
    {
        public int UserId { get; set; }
        public string TeacherName { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
