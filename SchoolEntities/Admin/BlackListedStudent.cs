using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Admin
{
    public class BlackListedStudent
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public string EnrolmentNumber { get; set; }
        public string StudentName { get; set; }
        public string SchoolLeftDate { get; set; }
        public int StudentId { get; set; }
        public string Comment { get; set; }
        public int TotalRows { get; set; }
    }
}
