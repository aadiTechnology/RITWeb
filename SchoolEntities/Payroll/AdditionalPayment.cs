using System;

namespace PayrollEntities
{
    public class AdditionalPaymentDetails
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public int ParameterId { get; set; }
        public string Parameter { get; set; }
        public int UserId { get; set; }
        public long Amount { get; set; }
        public string UserName { get; set; }
        public int StaffGroupId { get; set; }
        public int BankId { get; set; }        
        public int BankDetailsId { get; set; }
    }

    public class PaymentParameter
    {
        public int Id { get; set; }
        public string Parameter { get; set; }
    }
}
