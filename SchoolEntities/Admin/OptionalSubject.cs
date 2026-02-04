using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace SchoolEntities
{
    [Serializable]
    public class OptionalSubject
    {
        public int OptionalSubjectsId { get; set; }
        public String SubjectName { get; set; }
        public int SchoolWiseStandardDivisionId { get; set; }
        public bool IsConfigured { get; set; }
        public string OptionalSubjectName { get; set; }
        public int NoOfSubjects { get; set; }
        public int SubjectGroupId { get; set; }
        public int ParentOptionalSubjectId { get; set; }
        public int ChildOptionalSubjectId { get; set; }
        public int SubjectId { get; set; }
        public bool IsDefault { get; set; }
        public Constants.Action Action { get; set; }
    }

    public class TransferStudentSubjectsMarkDetails
    {
        public int YearwiseStudentId { get; set; }
        public string RegNo { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public int Standard_Division_Id { get; set; }
        public int SubjectId { get; set; }
        public string TransferFromSubjectName { get; set; }
    }
}
