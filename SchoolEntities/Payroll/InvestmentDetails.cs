using SchoolEntities;
using System;

namespace PayrollEntities
{
    public class SectionDetails : SchoolEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int SectionGroupId{ get; set; }
        public string SectionGroupName { get; set; }
        public bool IsExemption { get; set; }
        public int CategoryId { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal GroupMaxAmount { get; set; }        
        public int GroupId { get; set; }        
    }

    public class InvestmentMethod : SchoolEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }        
        public int AssociatedEarnDeductId { get; set; }
        public int MaxLimit { get; set; }
		public string AssociatedEarnDeductName { get; set; }
        public bool ApplyToAllUsers { get; set; }
        public string IsReset { get; set; }
        public int MaxAmount { get; set; }
        public int DocumentCount { get; set; }
    }

    public class InvestmentDeclaration : SchoolEntity
    {
        public int Id { get; set; }        
        public int InvestmentMethodId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string FilePath { get; set; }
        public bool IsDocSubmitted { get; set; }
        public int SectionId { get; set; }
        public int SortOrder { get; set; }
        public string SectionName { get; set; }        
        public string IsDeleted { get; set; }
        public int DocumentCount { get; set; }
        public int RegimId { get; set; }
    }

    public class EarningDeductionAmount : SchoolEntity
    {
        public int UserId { get; set; }
        public int EarningDeductionId { get; set; }
        public int InvestmentIncomeMethodId { get; set; }
        public decimal Amount { get; set; }
    }

    [Serializable]
    public class InvestmentDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }

    public class SectionGroup : SchoolEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public bool IsExemption { get; set; }
		public string IsDeleted { get; set; }
    }

    public class IncomeDeclaration : SchoolEntity
    {
        public int Id { get; set; }
        public int InvestmentMethodId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public string IsDeleted { get; set; }
        public int RegimId { get; set; }
    }

    public class Quarter : SchoolEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReceiptNumber { get; set; }        
    }

    public class ITCommissionerDetails : SchoolEntity
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }

    public class TaxDeduction : SchoolEntity
    {
        public int Id { get; set; }
        public int QuarterId { get; set; }
        public int UserId { get; set; }        
        public decimal TaxDeductionAmount { get; set; }
        public decimal TaxDepositedAmount { get; set; }
        public string QuarterName { get; set; }
    }

    public class IncomeTaxDetails : SchoolEntity
    {
        public int Id { get; set; }        
        public int UserId { get; set; }
        public bool IsPublished { get; set; }
        public string Designation { get; set; }
        public string UserName { get; set; }
    }

    public class TaxDeductorDetails : SchoolEntity
    {
        public int Id { get; set; }
        public int SalutationId { get; set; }
        public int DesignationId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public int FinancialYearId { get; set; }      
    }

    public class UserAgeDetails
    {
        public int UserId { get; set; }
        public int Age { get; set; }
        public int SalutationId { get; set; }
    }

    public class TaxReliefParameters
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Amount { get; set; }
    }

    public class UserDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PanNo { get; set; }
        public string Designation { get; set; }
        public string EmployeeNo { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string FinancialYear { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsSaved { get; set; }
        public string FinancialYearEnd { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
