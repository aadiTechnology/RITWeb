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
        public decimal TotalDays { get; set; }
        public int ChargeHandoverTo { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public int TotalRows { get; set; }
        public bool IsFinalApprover { get; set; }
        public string ApproverRemark { get; set; }
        public bool IsApprovedByApprover { get; set; }
        public string LeaveName { get; set; }
        public string LeaveType { get; set; }
        public decimal LeaveBalance { get; set; }
        public string SortExpression { get; set; }
        public bool IsLeaveUpdatedInPayroll { get; set; }
        public int LastApproverUserId { get; set; }
        public string DocumnetPhoto { get; set; }
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

    public class LeaveBalance
    {
        public int LeaveId { get; set; }
        public string LeaveName { get; set; }
        public decimal Balance { get; set; }
        public bool IsUnpaid { get; set; }
        public bool AllowZeroBalance { get; set; }
    }
}
