using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Transport
{
    public class RFIDDetails
    {
        public int SchoolWiseStudentId { get; set; }
        public string ClassName { get; set; }
        public string EnrolmentNumber { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string RFID { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
