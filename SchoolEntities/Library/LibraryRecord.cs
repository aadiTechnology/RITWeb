using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class LibraryRecord
    {
        public int Id { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string BookNo { get; set; }
        public string Comment { get; set; }
        public bool IsAbsent { get; set; }
        public int UserId { get; set; }
        public DateTime IssueTiming { get; set; }
        public string GrNo { get; set; }
    }
}
