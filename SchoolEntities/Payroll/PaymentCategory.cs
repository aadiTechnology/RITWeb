using System;
namespace PayrollEntities
{
    [Serializable]
    public class PaymentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EarningDeductionPercentage
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int EarningDeductionId { get; set; }
        public decimal Percentage { get; set; }        
        public EarningsDeductions EarnDeduct { get; set; }
    }

    [Serializable]
    public class UserPaymentCategoryAssociation
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
