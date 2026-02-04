/*File Name - ReportingConfigurationDC.cs
 * Created By - Pravin Shinde
 * Created Date - 25 Sept 2013
 * Description - This class is used to manage performance reporting configuration.
 */
using System;
using SchoolEntities;

namespace StaffPerformanceEntity
{
    public class PerformanceReportingConfig : SchoolEntity
    {
        public int Id { get; set; }
        public bool IsFinalApprover { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsSubmitted { get; set; }
        public int ReportingUserId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Year { get; set; }
        public int RoleId { get; set; }
        public bool IsPublished { get; set; }
        public int ApprovalSortOrder { get; set; }
    }
}
