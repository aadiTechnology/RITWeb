using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Admin
{
    public class UpdateStudentDetailsInBulk
    {
        public string NewValue { get; set; }
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string EnrollmentNumber { get; set; }
        public int RollNumber { get; set; }
        public int YearWise_Student_Id { get; set; }
        public string ExistingValue { get; set; }
        public string ClassName { get; set; }
        public int TotalRecords { get; set; }
    }
}
