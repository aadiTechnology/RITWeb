using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities; 

namespace AssociationEntities
{
    class Associations
    { }

    [Serializable]
    public class ParentTeacherAssociationDetails : SchoolEntity
    {
        public int TeacherAssociationDetailsId { get; set; }
        public int ParentAssociationDetailsId { get; set; }
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int DesignationId { get; set; }
		public int UserRoleId { get; set;}
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string TeacherName { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string StandardName { get; set; }
        public int StandardId { get; set; }
        public int OriginalStandardId { get; set; }
        public string DesignationName { get; set; }
        public int RelatedSection { get; set; }
        public int StudentId { get; set; }
        public string ConsideredAsParent { get; set; }
        public string ResidenceArea { get; set; }
        public string ContactTiming { get; set; }
        public string ContactNo { get; set; }
        public string Section { get; set; }
        public int FromStandardId { get; set; }
        public int ToStandardId { get; set; }
        public string Standards { get; set; }
        public string FromStandard { get; set; }
        public string ToStandard { get; set; }
        public int TeacherUserRoleId { get; set; }
        public int StudentUserRoleId { get; set; }
        public int AdminUserRoleId { get; set; }
        public int OriginalFromStandardId { get; set; }
        public int OriginalToStandardId { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public bool IsMobileNo1 { get; set; }
        public bool IsMobileNo2 { get; set; }
        public int SchoolCommitteeId { get; set; }
        public string RelatedSectionName { get; set; }
    }

    public class SectionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

