using System;

namespace SchoolEntities
{
    public class ObservationDetails
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ParameterId { get; set; }
        public int GradeId { get; set; }
        public string Remark { get; set; }
    }

    public class StudentBasicDetails
    {
        public int YearwiseStudentId { get; set; }
        public int RollNo { get; set; }
        public string EnrolmentNumber { get; set; }
        public string StudentName { get; set; }
    }

    public class ObservationGrade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable]
    public class ObservationParameter
    {
        public int Id { get; set; }
        public string Parameter { get; set; }
        public int SkillId { get; set; }
        public int SortOrder { get; set; }
        public int ControlTypeId { get; set; }
    }

    public class ObservationSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool DisplayOnReport { get; set;}
        public int SortOrder { get; set; }
    }

    public class ObservationRemarks
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public int ParameterId { get; set; }
        public int GradeId { get; set; }
    }
}
