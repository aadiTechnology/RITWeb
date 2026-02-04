using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace WeekDayNameDetails
{
    public class WeekdaysName : SchoolEntity
    {
        public int Id {get; set;}
        public string WeekDayName { get; set; }
    }
}
