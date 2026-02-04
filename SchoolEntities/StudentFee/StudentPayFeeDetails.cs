// Class Name       :- StudentPayFeeDetails
// Purpose          :- This is a entiity classes used to save and update fee details
// Date Of creation :- 26-Apr-2013
// Author Name      :- Pravin Shinde

namespace SchoolEntities.StudentFee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using SchoolEntities.Accounts;
    
    /// <summary>
    /// This class is used to save the fee details of student.
    /// </summary>
    [Serializable]
    public class StudentPayFeeDetails
    {
        public int StudentId { get; set; }
        public int AmountToBePaid { get; set; }
        public int ActualAmount { get; set; }
        public string Remarks { get; set; }
        public DateTime PaymentDate { get; set; }
        public int ConcessionAmount { get; set; }
        public int SchoolwiseStudentFeeId { get; set; }
        public int ActualLateFeeAmount { get; set; }
        //changed location from StudentFeeList to this
        public bool IsDirectlyDeposited { get; set; }
        public int BankId { get; set; }
        public int DepositeBankId { get; set; }
        public int ReceiptNumberOutput { get; set; }         
        public string ChallanNumber { get; set; }
        public List<StudentFeeDetails> lstStudentFeeList { get; set; }
        public StudentLateFeeDetails oLateFeeDetails { get; set; }
        public int FinancialYearId { get; set; }
        public string AdditionalRemark { get; set; }
        public bool IsCautionMoneyAdjusted { get; set; }
        public int RemainingCautionMoney { get; set; }
        public int JournalVoucherLedgerId { get; set; }
        public string FileName { get; set; }
    }

    /// <summary>
    /// This class is used to get all the fee details of student.
    /// </summary>
    [Serializable]    
    public class StudentPaidFeeDetails
    {
        public int SchoolwiseStudentFeeId { get; set; }
        public int StandardwiseFeeTypeId { get; set; }
        public string FeeType { get; set; }
        public string PayableFor { get; set; }
        public int Amount { get; set; }
        public string DebitOrCredit { get; set; }
        public int AmountPayable { get; set; }       
        public string SerialNumber { get; set; }
        public int LateFeeAmount { get; set; }
        public int ConcessionAmount { get; set; }
        public int AccountHeaderId { get; set; }
        public string FileName { get; set; }
    }

    /// <summary>
    /// This class is used to get all the PDC cheque details on PDC fee payment.
    /// </summary>
    [Serializable]
    public class ChequeDetails
    {
        public int ChequeId { get; set; }        
        public string Status { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public int BankId { get; set; }
        public string Remarks { get; set; }
        public int ChequeAmount { get; set; }
        public bool IsPDC { get; set; }
        public bool IsDeleted { get; set; }        
        public DateTime ChequePassedDate { get; set; }
    }

    /// <summary>
    /// This class is used to generate XML for pay fee, containing all the selected fee types with their late fee amount.
    /// </summary>
    [Serializable]
    //public class StudentFeeList
    public class StudentFeeDetails
    {
        public int StudentFeeId { get; set; }
        public int PaybleAmount { get; set; }
        public int ActualAmount { get; set; }
        public int LateFee { get; set; }        
    }

    /// <summary>
    /// This class is used to get the Total latefee amount with description for selected fee types.
    /// </summary>
    [Serializable]
    public class StudentLateFeeDetails
    {
        public int TotalLateFeeAmount { get; set; }
        public string LateFeeDescription { get; set; }
    }
    
    /// <summary>
    /// This class is used for the fee payment by swap card.
    /// </summary>
    [Serializable]
    public class SwapCardDetails
    {
        public string SwapNo { get; set; }
        public int CardTypeId { get; set; }     
    }

    /// <summary>
    /// This class is used for the newly added credit entries.(If paid extra fee for new/existing fee type.)
    /// </summary>
    [Serializable]
    public class CreditDetails
    {
        public int StdFeeTypeId { get; set; }
        public string FeeType { get; set; }
        public string PayableFor { get; set; }
        public DateTime ChequeDate { get; set; }
        public int CreditedAmount { get; set; }
        public bool IsNewlyAdded { get; set; }
    }

    /// <summary>
    /// This class is specifically used to fill the paid details on editing transaction.
    /// </summary>
    public class EditFeeDetails
    {
        public int AmountPaid { get; set; }
        public int PaidLateFee { get; set; }
        public int Payble { get; set; }
        public int ApplicableLateFee { get; set; }
        public int Concession { get; set; }
        public StudentPayFeeDetails oStudentPayFeeDetails { get; set; }
        public bool IsCautionMoneyAdjusted { get; set; }
        public string FileName { get; set; }
     }

    /// <summary>
    /// This class is used for the fee payment types of Electronic transactions.
    /// </summary>
    [Serializable]
    public class ElectronicPaymentType
    {
        public int TypeId { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// This class is used for the fee payment by Paperless / Electronic transaction.
    /// </summary>
    [Serializable]
    public class ElectronicPaymentDetails
    {
        public string TxnNo { get; set; }
        public ElectronicPaymentType oElectronicPaymentType { get; set; }
    }

    public class StudentFeeClearanceDetails
    {
        public int StudentElectronicPaymentId { get; set; }
        public string StudentName { get; set; }
        public string RegNo { get; set; }
        public string Class { get; set; }     
        public FeeClearanceFilters oFeeClearanceFilters { get; set; }
        public StudentPayFeeDetails oStudentPayFeeDetails { get; set; }
        public int Receipt_Number { get; set; }
        public string TransactionNumber { get; set; }
        public int IsCautionMoneyPayment { get; set; }
    }

    public class FeeClearanceFilters
    {
        public bool IncludeAll { get; set; }
        public string TransactionNumber { get; set; }
        public string RegNo { get; set; }
        public DateTime ClearanceStartDate { get; set; }
        public DateTime ClearanceEndDate { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public DateTime PaymentEndDate { get; set; }
        public int TypeId { get; set; }
        public int DepositedBankId { get; set; }
        public bool IncludeCautionMoney { get; set; }
    }

    public class IncompleteTransaction :SchoolEntity
    {
        public int NetBankingPaymentTransactionId { get; set; }
        public string EnrolmentNumber { get; set; }
        public string StudentName { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal TransactionAMT { get; set; }
        public string TransactionType { get; set; }
        public int TransactionTypeId { get; set; }
        public int GatewayId { get; set; }
        public int StudentId { get; set; }
    }
}
