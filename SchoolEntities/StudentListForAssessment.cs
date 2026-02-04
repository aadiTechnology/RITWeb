using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StudentListForAssessment
    {
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public int StandardId { get; set; }
        public bool IsSelfSubmitted { get; set; }
        public bool IsPeerSubmitted { get; set; }
        public bool IsParentSubmitted { get; set; }
    }
}
