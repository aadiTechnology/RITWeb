// -----------------------------------------------------------------------
// <copyright file="BirthDaySMS.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 
    [Serializable]
    public class BirthDaySMS
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }
        public int AcademicYearId { get; set; }
        public int UserId { get; set; }
        public string SMSTemplate { get; set; }
        public string SalutationName { get; set; }
        public string IsStuudent { get; set; }
    }

    [Serializable]
    public class ScheduledSMSDetails
    {
        public string SMSText { get; set; }
        public string MoblieNumbers { get; set; }
        public DateTime ScheduleAt { get; set; }
    }

    public class SMSDetails
    {
        public string SMSTime { get; set; }
        public string MobileNos { get; set; }
        public string SMSText { get; set; }
        public int TotalSMS { get; set; }
        public string DeliveryStatus { get; set; }
    }
}
