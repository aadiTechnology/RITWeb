// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class LateFeeConfiguration
    {
        public string Fee_Type { get; set; }
        public int Fee_Type_Id { get; set; }
        public string Late_Fee { get; set; }
        public int School_Id { get; set; }
        public int Academic_Year_Id { get; set; }
        public int Standard_Id { get; set; }
        public int StandardwiseFeeTypeId { get; set; }
        public int Day { get; set; }
        public int Interval { get; set; }     
        public int LateFeePerTypeId { get; set; }
        public int ValueForType { get; set; }        
        public int Original_Fee_Type_Id { get; set; }
        public int IsStudentFeeCount { get; set; }
        public int DeactivateUser { get; set; }
        public int ThresholdMonths { get; set; }
        public int ThresholdDays { get; set; }
        public int ReminderDays { get; set; }
        public int ReminderInterval { get; set; }
        public int ReminderSMS { get; set; }
        public int IsConfigured { get; set; }
        public List<FeeTypeInterval> LateFeeIntervals { get; set; }
    }

     [Serializable]
    public class FeeTypeInterval
    {
        public int StandardwiseFeeTypeId { get; set; }
        public string IntervalName { get; set; }
        public DateTime Due_Date { get; set; }
        public int DueDateDetailsId { get; set; }
        public DateTime IntervalStartDate { get; set; }
        public DateTime IntervalEndDate { get; set; }
    }

    public class LateFeeTypes
    {
        public int LateFeeTypeId { get; set; }
        public string LateFeeType { get; set; }
    }

    public class LateFeeDetails
    {
        public int Id { get; set; }
        public int FeeTypeId { get; set; }
        public int LateFeeId { get; set; }
        public int ValueForType { get; set; }
        public int LateFeePerTypeId { get; set; }
        public int Amount { get; set; }
        public int RepeatCount { get; set; }
        public int SortOrder { get; set; }
        public bool ExcludeHolidays { get; set; }
        public bool ExcludeWeekends { get; set; }
        public int SrNo { get; set; }
    }
}
