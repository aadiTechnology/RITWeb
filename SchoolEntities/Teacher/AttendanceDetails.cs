using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class AttendanceDetails
    {
        public string StudentName { get; set; }
        public decimal Mobile_Number { get; set; }
        public decimal Mobile_Number2 { get; set; }
        public int User_Id { get; set; }
        public DateTime FromAbsentDate { get; set; }
    }
}
