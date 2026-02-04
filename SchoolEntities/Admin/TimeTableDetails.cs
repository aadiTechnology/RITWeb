using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class TimeTableDetails : IComparable
    {
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string Class { get; set; }
        public string Day { get; set; }
        public int Period { get; set; }
        public int StandardDivisionID { get; set; }
        public int WeekDayID { get; set; }
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }
        public int IsAdditional { get; set; }
        public int ParentGroupId { get; set; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            TimeTableDetails c = (TimeTableDetails)obj;
            return String.Compare(this.Class, c.Class);
        }

        #endregion
    }
}
