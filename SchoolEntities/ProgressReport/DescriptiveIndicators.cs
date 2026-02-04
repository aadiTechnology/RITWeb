using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class DescriptiveIndicator
    {
        public Student StudentDetails { get; set; }
        public List<DescriptiveSkill> DescriptiveSkills { get; set; }
        public List<DescriptiveParameter> DescriptiveParameters { get; set; }
        public List<StudentwiseDescriptiveObservation> StudentwiseDescriptiveObservations { get; set; }
        public List<StudentwiseDescriptiveMark> StudentwiseDescriptiveMarks { get; set; }
    }

    public class DescriptiveSkill
    {
        public int Id { get; set; }
        public string Skill { get; set; }
        public int ParentSkillId { get; set; }
        public int OutOfMark { get; set; }
        public int SortOrder { get; set; }
    }

    public class DescriptiveParameter
    {
        public int Id { get; set; }
        public string Parameter { get; set; }
        public int SkillId { get; set; }
        public int SortOrder { get; set; }
    }

    public class StudentwiseDescriptiveObservation
    {
        public int Id { get; set; }
        public int YearwiseStudentId { get; set; }
        public int SkillId { get; set; }
        public string Observation { get; set; }
    }

    public class StudentwiseDescriptiveMark
    {
        public int Id { get; set; }
        public int ObservationId { get; set; }
        public int ParameterId { get; set; }
        public decimal Mark { get; set; }
        public int SkillId { get; set; }
        public int AssignedGradeId { get; set; }
    }

    public class StudentDetailsForDescriptiveIndicators
    {
        public int RollNo { get; set; }
        public int YearwiseStudentId { get; set; }
        public string StudentName { get; set; }
        public int StandardId { get; set; }
        public int EditStatus { get; set; }
        public int IsPublished { get; set; }
    }
}
