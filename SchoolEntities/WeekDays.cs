using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class WeekDay : SchoolEntity
    {
        public int OriginalWeekDaysId { get; set; }
        public string WeekDayName { get; set; }
        public bool IsWeekend { get; set; }
    }
}
