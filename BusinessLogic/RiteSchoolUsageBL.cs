using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using DataCommunicator;

namespace BusinessLogic
{
    public class RiteSchoolUsageBL
    {
        public static List<ExecutionDate> GetAllDates()
        {
            return RiteSchoolUsageDC.GetAllDates();
        }

        public static List<UsageDetails> GetRitUsageDetails(int aiStartIndex, int aiEndIndex, string asSortingCriteria, string asDate)
        {
            return RiteSchoolUsageDC.GetRitUsageDetails(aiStartIndex, aiEndIndex, asSortingCriteria, asDate);
        }

        public static void GenerateReport()
        {
            RiteSchoolUsageDC.GenerateReport();
        }
    }
}
