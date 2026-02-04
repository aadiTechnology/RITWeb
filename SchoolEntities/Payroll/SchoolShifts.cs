using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using SchoolEntities;

namespace PayrollEntities
{
    [Serializable]
    public class SchoolShifts : SchoolEntity
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public string HalfDayTime { get; set; }
        public string LateMarkTime { get; set; }
        public int AcademicYearID { get; set; }
    }
}
