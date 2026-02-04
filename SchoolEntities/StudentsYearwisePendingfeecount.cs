using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class StudentsAcademicYearwisePendingFeeCountDetails
    {
        public List<AcademicYearDetails> AcademicYears { get; set; }
        public List<StudentCount> StudentCounts { get; set; }
    }

    public class AcademicYearDetails
    {
        public int AcademicYearId { get; set; }
        public string AcademicYearName { get; set; }
    }
    public class StudentCount
    {
        public int CategoryId { get; set; }
        public int AcademicYearId { get; set; }
        public int Count { get; set; }

    }
}
