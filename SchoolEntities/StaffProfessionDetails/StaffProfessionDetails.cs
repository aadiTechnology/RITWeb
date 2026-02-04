using System;

namespace StaffProfessionEntity
{
    [Serializable]
    public class StaffDetailsForProfession
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Salutation { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MartialStatus { get; set; }
        public string Address { get; set; }
        public string EmailID { get; set; }
        public string PhoneNumber { get; set; }
        public string PrincipalName { get; set; }
        public string SchoolName { get; set; }
        public int SubmitedStatus { get; set; }
    }

    [Serializable]
    public class ProfessionDetails
    {
        public int AwardId { get; set; }
        public string AwardName { get; set; }
        public int ProfessionSkillId { get; set; }
        public string ProfessionSkill { get; set; }
        public int SkillSortOrder { get; set; }
        public int ParameterId { get; set; }
        public string ParameterTitle { get; set; }
        public int ParameterSortOrder { get; set; }
    }

    [Serializable]
    public class StaffProfessionDetails
    {
        public int UserId { get; set; }
        public int AwardId { get; set; }
        public int ParameterId { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class AwardDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class UserAwardDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Designation { get; set; }
        public int TotalRows { get; set; }
    }
}
