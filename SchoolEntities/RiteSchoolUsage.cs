using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class RiteSchoolUsage
    {
    }

    public class ExecutionDate
    {
        public DateTime Date { get; set; }
    }

    public class UsageDetails
    {
        public string QueryName { get; set; }
        public string Legend { get; set; }
        public int TotalRows { get; set; }
    }
}
