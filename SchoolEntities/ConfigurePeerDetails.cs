using SchoolEntities;
using System;
using System.Collections.Generic;

namespace SchoolEntities
{
    public class ConfigurePeerDetails
    {
        public int YearwiseStudentId { get; set; }
        public int PeerYrStudentId { get; set; }
        public int Id { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string PeerName { get; set; }
    }
}
