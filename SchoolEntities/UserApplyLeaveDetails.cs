using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Payroll
{
    public class UserApplyLeaveDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LeaveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public int ChargeHandoverTo { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
      //  public int LeaveId { get; set; }
        public int TotalRows { get; set; }
    }

    public class LeaveApprovalCatgories
    {

        public int Id { get; set; }
        public string Category { get; set; }
    }
    public class LeaveApprovalDetails
    {
        public int Id { get; set; }
        public int UserLeaveDetailsId { get; set; }
        public int ReportingUserId { get; set; }
        public string Remark { get; set; }
        public int StatusId { get; set; }
      
    
    }
}
