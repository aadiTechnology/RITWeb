using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class OldYearPendingFeeReport
    {

        public List<OldYearPendingFeeStudent> OldYearPendingFeeStudents { get; set; }
        public List<OldYearPendingFee> PendingFees { get; set; }
        public List<OldYearPendingFeeStudent> OldYearPaidFeeStudents { get; set; }
        public List<OldYearPendingFee> PaidFees { get; set; }
    }
     [Serializable]
    public class OldYearPendingFeeStudent
    {
        public string RegNo { get; set; }
        public string Class { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string MobileNo { get; set; }
        public int YearWiseStudentId { get; set; }
        public int OriginalStandardId { get; set; }
        public int OriginalDivisionId { get; set; }
        public string HasSibling { get; set; }
    }

     [Serializable]
     public class OldYearPendingFee
     {
         public int StudentId { get; set; }
         public int AcademicYearId { get; set; }
         public string AcademicYear { get; set; }
         public int Amount { get; set; }
     }
}
