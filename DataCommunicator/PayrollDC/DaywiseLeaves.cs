using System;

namespace PayrollEntities
{
   [Serializable]
   public class DaywiseLeaves
    {
        public int Day { get; set; }
        public int LeaveId { get; set; }
        public double LeaveCount { get; set; }
        public string ShortName { get; set; }
        public int OriginalLeaveId { get; set; }
        public bool IsHalfLeave { get; set; }
        public DateTime Date { get; set; }
        public decimal LeaveBalance { get; set; }
        public decimal Days { get; set; }
        public bool IsSelected { get; set; }
        public bool IsUnPaidLeave { get; set; }
        public bool IsLateMark { get; set; }
        public int SortOrder { get; set; }
        public decimal MinimumBalance { get; set; }
       public string ColorCode { get; set; }
    }

   [Serializable]
   public class LateMarkConfiguration
   {   
       public int LateMarkCount { get; set; }
       public int SortOrder { get; set; }
       public decimal ConsideredLeaves { get; set; }
   }

   [Serializable]
   public class LateMarkLeave
   {
       public int LeaveId { get; set; }
       public int UserId { get; set; }
       public decimal Days { get; set; }
   }
}
