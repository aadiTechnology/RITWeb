using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
   public class EmployeeDetails
    {
        public int EmployeeJonDetailsId { get; set; }
        public int UserId { get; set; }
        public string EPFNumber { get; set; }
        public bool IsVPSDeduction { get; set; }
        public int VPSContributionId { get; set; }
        public DateTime VPSContributionEffectiveForm { get; set; }
        public decimal UPFAmount { get; set; }
        public DateTime IncrementDate { get; set; }
        public int IncomeTaxStatusId { get; set; }
        public int PayrollId { get; set; }
        public int PayrollGroupId { get; set; }
        public decimal BasicPay { get; set; }
        public DateTime EPFJoinDate { get; set; }
        public string Branch { get; set; }
        public decimal VPFPercentage { get; set; }
        // public string BankName { get; set; }
        public decimal PayScale { get; set; }
        // public DateTime EPFJoinDate { get; set; }
        //public DateTime EPFJoinDate { get; set; }
        //public DateTime EPFJoinDate { get; set; }
        public int AcademicYearId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertedById { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedById { get; set; }


        public int EmployeeFamilyDetailsId { get; set; }
        public string FamilyMemberName { get; set; }
        public int Age { get; set; }
        public string Relation { get; set; }
        public string Occupaton { get; set; }


        public int EmployeeId { get; set; }
        public string Employeecode { get; set; }
        public bool Gender { get; set; }
        public string Reference { get; set; }
        public bool Maritalstatus { get; set; }
        public DateTime Incrementdate { get; set; }
        public decimal SalaryScale { get; set; }
        public string WhatsAppNo { get; set; }
        public string GPFAcNumber { get; set; }
        public string BankName { get; set; }
        public string BankAcNo { get; set; }
        //public string Branch { get; set; }



        public int ExperienceDetailsId { get; set; }
        public int School_Id { get; set; }
        public string SchoolName { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime leftDate { get; set; }
        public string PreviousDesignation { get; set; }
        public decimal LastSalary { get; set; }
        public string Duration { get; set; }
        public string JobDescription { get; set; }
        public string ReasonforLeaving { get; set; }
        public string OrganizationName { get; set; }




        public string MobileNo { get; set; }
        public string PrimaryEmailId { get; set; }
        public string AccountNo { get; set; }
        public string PanNo { get; set; }
        public string UAN { get; set; }
        public string DesignationName { get; set; }


        public string CompanyEmail { get; set; }
        public string CompanyContNo { get; set; }
        public string PermanentContactNo { get; set; }
        public string Extensionno { get; set; }
        public string CompanyContactNo { get; set; }
        
    }
}
