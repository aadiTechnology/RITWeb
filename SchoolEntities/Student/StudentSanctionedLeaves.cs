using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace StudentEntities
{
    [Serializable]
    public class StudentSanctionedLeaves
    {
        public int SchoolID { get; set; }
        public int AcademicYearId { get; set; }
        public int UpdatedByID { get; set; }
        public int SanctionedLeaveDetailsId { get; set; }
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string StudentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCanceled { get; set; }
        public string MobileNumber { get; set; }
        public string RegistrationNo { get; set; }
        public string Class { get; set; }
        public DateTime AStartDate { get; set; }
        public DateTime AEndDate { get; set; }
        public int TotalRows { get; set; }
        public string Remark { get; set; }
        public bool ShowOnAbsectStudentPopUp { get; set; }
    }

    [Serializable]
    public class UserDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MobileNumbers { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ShowOnAbsectStudentPopUp { get; set; }
    }

    [Serializable]
    public class SanctionedLeavesInfo : SchoolEntity
    {
        public List<StudentSanctionedLeaves> lstStudentSanctionedLeaves = new List<StudentSanctionedLeaves>();
    }
}