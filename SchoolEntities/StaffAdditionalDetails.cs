using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StaffAdditionalDetails : SchoolEntity
    {
        public int BloodGroupId { get; set; }
        public int MaritialStatusId { get; set; }
        public int ReligionId { get; set; }
        public int CategoryId { get; set; }
        public string Cast { get; set; }
        public string AadharNumber { get; set; }
        public int QualificationId { get; set; }
        public string Specialization { get; set; }
        public string YearOfPassing { get; set; }
        public int ClassId { get; set; }
        public string University { get; set; }
        public string OrganisationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime LeftDate { get; set; }
        public int ExperianceId { get; set; }
        public int EducationId { get; set; }
        public string Qualification { get; set; }
        public string ClassName { get; set; }
        public string PreviousDesignation { get; set; }
        public decimal LastSalary { get; set; }
        public int Duration { get; set; }
        public string JobDescription { get; set; }
        public string ReasonForLeaving { get; set; }
        public string Achievement { get; set; }
    }
}
