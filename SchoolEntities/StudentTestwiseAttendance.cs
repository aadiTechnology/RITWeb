using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace StudentTestwiseAttendanceEntities
{
    public class StudentTestwiseAttendance : SchoolEntity

    {
        public int TestId { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public DateTime TestStartDate { get; set; }
        public DateTime TestEndDate { get; set; }
        public StudentTestwiseAttendance AttendanceEvalution { get; set; }

    }
}
