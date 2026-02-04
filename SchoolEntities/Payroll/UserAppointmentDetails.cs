using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MasterEntities;

namespace PayrollEntities
{
    public class SalaryParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEarning { get; set; }
        public bool IsBasic { get; set; }
    }

    public class PaymentGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EarningDeductionGroup> EarningDeductionGroups { get; set; }
    }

    public class EarningDeductionGroup
    {
        public int Id { get; set; }
        public int PaymentGroupId { get; set; }
        public int EarningDeductionId { get; set; }
        public string ShortName { get; set; }
        public bool IsEarning { get; set; }
        public decimal Amount { get; set; }
        public int AppointmentId { get; set; }
    }

    public class UserAppointmentDetails
    {
        public int Id { get; set; }
        public int SalutationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public int PaymentGroupId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public DateTime AgreementDate { get; set; }
        public StaffStatusDetails Status { get; set; }
        public List<EarningDeductionGroup> EarningDeductions { get; set; }
        public string EarningDeductionXml { get; set; }
        public string EmployeeNo { get; set; } 
    }
}
