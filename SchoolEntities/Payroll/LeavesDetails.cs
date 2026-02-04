using System;
using System.Xml.Serialization;
using SchoolEntities;
using System.Collections.Generic;
using MasterEntities;

namespace PayrollEntities
{   
    [Serializable]
    public class DaywiseLeaves : SchoolEntity
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
        public bool ExcludeFromSalaryDeduction { get; set; }
        public bool AllowZeroBalance { get; set; }

    }

   [Serializable]
   public class LateMarkConfiguration : SchoolEntity
   {   
       public int LateMarkCount { get; set; }
       public int SortOrder { get; set; }
       public decimal ConsideredLeaves { get; set; }
       public string LateMarkConfigurationXML { get; set; }
       public string StaffLeavesSortOrderXML { get; set; }
   }

   [Serializable]
   public class LateMarkLeave : SchoolEntity
   {
       public int LeaveId { get; set; }
       public int UserId { get; set; }
       public decimal Days { get; set; }
   }

   //Table Name-LateMarkLeaves
   [Serializable]
   public class UserLateMarkLeave : SchoolEntity
   {
       public int LateMarkLeavesId { get; set; }
       public int UserId { get; set; }
       public int LeaveId { get; set; }
       public int MonthId { get; set; }
       public int Year { get; set; }
       public decimal Days { get; set; }
       public bool IsUnPaidLeave { get; set; }
   }
   // Table StaffHolidayLeavesConfiguraton
   public class StaffHolidaysSalaryDeduction : SchoolEntity
   {
       public int StaffHolidaysSalaryDeductionId { get; set; }
       public DateTime HolidayStartDate { get; set; }
       public DateTime HolidayEndDate { get; set; }
       public string HolidayName { get; set; }
       public int Days { get; set; }
       public decimal PercentageToDeduct { get; set; }
       public int Type { get; set; }
       public bool IsWeekend { get; set; }
   }

   public class UsersSalaryDeduction : SchoolEntity
   {
       public int UsersSalaryDeductionId { get; set; }
       public int UserId { get; set; }
       public int StaffHolidayAndLeavesConfigurationId { get; set; }
       public int MonthId { get; set; }
       public int Year { get; set; }
       public int LeaveId { get; set; }
       public int Days { get; set; }
       public decimal PercentageToDeduct { get; set; }
   }

   [Serializable]
   public class DatewiseStaffLeave : SchoolEntity
   {
       public int DatewiseStaffLeavesId { get; set; }
       public int UserId { get; set; }
       public int LeaveId { get; set; }
       public DateTime Date { get; set; }
       public bool IsHalfLeave { get; set; }
       public int MonthId { get; set; }
       public int Year { get; set; }
       public string LeaveXml { get; set; }
       public string LateMarkLeaveXml { get; set; }
       public int StaffGroupsId { get; set; }
       public int LateMarkLeaveId { get; set; }
       public decimal LateMarkLeaves { get; set; }
       public string StaffHolidayLeaveConfigIds { get; set; }
       public bool ExcludeFromSalaryDeduction { get; set; }
       public string Holidays { get; set;}
       public decimal LateMarkLeaveCount { get; set; }
   }
   public class StaffLeaveDetails : SchoolEntity
   {
       public int LeaveDetailsId { get; set; }
       public string ShortName { get; set; }
       public int LeaveId { get; set; }
       public decimal Days { get; set; }
       public int StaffAttendanceId { get; set; }
       public int OriginalLeaveId { get; set; }
   }

   //Table Name-StaffLeaves
   [Serializable]
   public class ConfiguredLeaves : SchoolEntity
   {
       public int LeaveId { get; set; }
       public string LeaveName { get; set; }
       public string ShortName { get; set; }
       public bool CanAccumulate { get; set; }
       public bool IsUnpaidLeave { get; set; }
       public int OriginalLeaveId { get; set; }
       public decimal AccumulateValues { get; set; }
       public decimal MinimumBalance { get; set; }
       public decimal BasicLeaves { get; set; }
       public string ColorCode { get; set; }
       public bool ExcludeFromSalaryDeduction { get; set; }
       public string LeaveXML { get; set; }
       public bool AllowZeroBalance { get; set; }
   }
   [Serializable]
   public class UserLeaveConfiguration : SchoolEntity
   {
       public int UserLeavesYearwiseConfiguration { get; set; }
       public int UserId { get; set; }
       public int LeaveId { get; set; }
       public decimal OriginalLeaveBalance { get; set; }
       public decimal LeaveBalance { get; set; }
       public int Year { get; set; }
       public string AllowedLeaveXML { get; set; }
       public string BasicLeaveXml { get; set; }
   }

   public class PartialLeaveDetails : SchoolEntity
   {
       public int ExistingLeaveId { get; set; }       
       public int PartialLeaveId { get; set; }
       public int DatewisePartialStaffLeavesId { get; set; }
       public string ShortName { get; set; }
       public DateTime LeaveDate { get; set; }
   }

   public class UsersLeaveBalance : SchoolEntity
   {
       public ConfiguredLeaves configuredLeaves { get; set; }
       public decimal LeaveBalance { get; set; }
   }

   public class BasicLeaveConfiguration : SchoolEntity
   {
       public int Id { get; set; }       
       public StaffGroupsEntity StaffGroups { get; set; }
       public MonthMaster Month { get; set; }
       public int Year { get; set; }
       public bool IsAccumulationMonth { get; set; }
       public string LeaveXml { get; set; }
       public List<BasicLeaveDetails> Leaves { get; set; }
       public bool ApplyToAllUsers { get; set; }
       public bool UpdateExistng { get; set; }
   }

   public class BasicLeaveDetails
   {
       public int Id { get; set; }
       public int BasicLeaveConfigId { get; set; }
       public int UserId { get; set; }
       public ConfiguredLeaves Leave { get; set; }
       public decimal BasicLeaves { get; set; }
       public decimal AccumulateLeaves { get; set; }
       public int LeaveId { get; set; }
       public MonthMaster Month { get; set; }
       public int MonthId { get; set; }
   }

   public class StaffHolidayLeavesConfigTypes : SchoolEntity
   {
       public int Id { get; set; }
       public string Type { get; set; }
   }

   public class LeaveYear
   {
       public int Id { get; set; }
       public string Year { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
   }

   public class DaywiseStaffAttendance
   {
       public int Id { get; set; }
       public int SrNo { get; set; }
       public string Name { get; set; }
       public string MobileNo { get; set; }
       public string Designation { get; set; }
       public DateTime JoiningDate { get; set; }
       public DateTime ResignationDate { get; set; }
       public int LeaveId { get; set; }
       public bool IsHalfLeave { get; set; }
       public bool IsLateMark { get; set; }
       public int PartialLeaveId { get; set; }
       public int UserId { get; set; }
       public string LeaveBalance { get; set; }
       public string LeaveDetails { get; set; }
       public string EmployeeNo { get; set; }
   }

   public class UserLeaveDetails
   {
       public int RowNo { get; set; }
       public int UserId { get; set; }
       public string UserName { get; set; }
       public int LeaveId { get; set; }
       public int Day { get; set; }
       public int MonthId { get; set; }
       public int Year { get; set; }
       public bool IsHalfLeave { get; set; }
       public bool IsLateMark { get; set; }
       public int PartialLeaveId { get; set; }
       public string LeaveName { get; set; }
       public string LeaveColor { get; set; }       
   }

   public class LeaveBalanceDetails
   {
       public int RowNo { get; set; }
       public int UserId { get; set; }
       public string UserName { get; set; }
       public int LeaveId { get; set; }     
       public string LeaveName { get; set; }
       public decimal LeaveBalance { get; set; }
       public int LeaveYear { get; set; }
   }

   public class HolidayMaster
   {
       public int Id { get; set; }
       public DateTime StatDate { get; set; }
       public DateTime EndDate { get; set; }
       public string HolidayName { get; set; }
   }

   public class UserDetailsForLeave : SchoolEntity
   {
       public int StaffGroupsId { get; set; }
       public int UserId { get; set; }
   }

   public class LeaveEncashmentDetails
   {
       public int Id { get; set; }
       public int Year { get; set; }
       public int UserId { get; set; }
       public int LeaveId { get; set; }
       public decimal EncashCount { get; set; }
       public decimal Amount { get; set; }
       public string Description { get; set; }
       public string LeaveType { get; set; }
       public DateTime Date { get; set; }
       public decimal LeaveBalance { get; set; }
   }
}
