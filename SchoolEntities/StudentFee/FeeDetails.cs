using System;
using SchoolEntities;
using System.Collections.Generic;

namespace FeeEntities
{
	[Serializable]
	public class FeeDetails : SchoolEntity
	{
		public int Schoolwise_Student_Fee_Id { get; set; }
		public int Student_Id { get; set; }
		public string Payable_For { get; set; }
		public int Standard_Div_Id { get; set; }
		public int Std_FeeType_Id { get; set; }
		public string Fee_Type { get; set; }
		public int AmtPaid { get; set; }
		public int AmtPayable { get; set; }
		public int TotalAmtToPay { get; set; }
		//Credit/Debit
		public string FeeMode { get; set; }
		public string PaymentMode { get; set; }
		public DateTime Paid_Date { get; set; }
        public DateTime DueDate { get; set; }
		public string Receipt_Number { get; set; }
		public string Remarks { get; set; }
		public int Student_Fee_Id { get; set; }
		public int Serial_Number { get; set; }
		public int Is_Cheque_Bounce { get; set; }
		public int RefundFeeDetailsID { get; set; }
		public int NetBankingPaymentTransactionID { get; set; }
		public int IsReceiptConsidered { get; set; }
		public int Is_Completed { get; set; }
		public int IsCardPayment { get; set; }
		public int Is_Concession_Fee { get; set; }
		public int DebitStudentFee_Id { get; set; }
		//Cheque details
		public string Cheque_Number { get; set; }
		public DateTime Cheque_Date { get; set; }
		public string BankName { get; set; }
		public int Bank_Id { get; set; }
		public char Is_PDC { get; set; }
		public int Cheque_Amount { get; set; }
		public int LateFee { get; set; }
        public int OriginalLateFee { get; set; }
		public string LateFeeRemark { get; set; }
		public int ConcessionAmt { get; set; }
		public int DepositBankId { get; set; }
		public bool IsDirectlyDeposited { get; set; }
		public string ChallanNo { get; set; }
	}

	public class BankDetails : SchoolEntity
	{
		public int Schoolwise_Bank_Id { get; set; }
		public string Bank_Name { get; set; }
	}
    public class RegBankDetails : SchoolEntity
    {
        public int NetBankingBankId { get; set; }
        public string RegisterdBankName { get; set; }
    }

	public class CardTypeDetails : SchoolEntity
	{
		public int CardTypeId { get; set; }
		public string CardType { get; set; }
	}

	public class FeeTypeDetails : SchoolEntity
	{
		public int SchoolWise_Standard_FeeType_Id { get; set; }
		public string Fee_Type { get; set; }
	}

	public class PayableForDetails : SchoolEntity
	{
        public int Id { get; set; }
		public int Amount { get; set; }
		public string PayableFor { get; set; }
        public int StudentId { get; set; }
        public int SortOrder { get; set; }
        public string FeeType { get; set; }
	}

	public class CardPaymentDetails : SchoolEntity
	{
		public int CardTypeId { get; set; }
		public string Swap_Number { get; set; }
		public int Bank_Id { get; set; }
	}

	public class LateFeeDeactivationSettings : SchoolEntity
	{
		public int StandardId { get; set; }
		public int FeeTypeId { get; set; }
		public bool DeactivateUser { get; set; }
		public int ThresholdMonths { get; set; }
		public int ThresholdDays { get; set; }
		public int ReminderDays { get; set; }
		public int ReminderInterval { get; set; }
		public int ReminderSMS { get; set; }
	}

	public class FeeDefaulter : SchoolEntity
	{
		public int UserId { get; set; }
		public DateTime DeactivationDate { get; set; }
		public int ReminderDays { get; set; }
		public int ReminderInterval { get; set; }
		public string MobileNumber { get; set; }
		public string DisplayText { get; set; }
		public int ReminderSMS { get; set; }
	}

    public class DisbaleBankDetails : SchoolEntity
    {
        public RegBankDetails RegBankDetails { get; set; }
        public int DisabledBankId { get; set; }
        public DateTime StartDateTime { get; set; }
        //public string StartTime { get; set; }
        public DateTime EndDateTime { get; set; }
        //public string EndTime { get; set; }
        public string RuleStatus { get; set; }
    }

	public class FeeCollection
	{
		public int Fees { get; set; }
		public int InternalFees { get; set; }
		public int CautionMoney { get; set; }
	}

    public class InternalFeeDetails
    {
        public string FeeType { get; set; }
        public string PayableForDisplayText { get; set; }
        public string PayableFor { get; set; }
        public bool IsSelected { get; set; }
        public int Amount { get; set; }
        public string Remark { get; set; }
        public int InternalFeeDetailsId { get; set; }        
    }

    public class InternalFeeDebitDetails
    {
        public string FeeType { get; set; }        
        public string PayableFor { get; set; }        
        public int TotalAmount { get; set; }
        public int Amount { get; set; }
        public int PendingAmount { get; set; }
        public string DebitCredit { get; set; }
        public DateTime PaidDate { get; set; }
        public string Remarks { get; set; }
        public int StandardDivId { get; set; }
        public int StandardId { get; set; }
        public int DivisionId { get; set; }
        public int InternalFeeDetailsId { get; set; }
        public int FeeDetailsId { get; set; }
        public int SerialNumber { get; set; }
        public int ReceiptNo { get; set; }
        public int SchoolId { get; set; }
        public int InternalFeeMasterId { get; set; }
        public int IsLastCredit { get; set; }
        public bool IsCleared { get; set; }
        public int FrequentlyUsedBankId { get; set; }
        public string IsChequeBounced { get; set; }
        public bool IsDueDateApplicable { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public int NextAcademicYearId { get; set; }
        public int YearwiseStudentId { get; set; }
        public int NetBankingPaymentTransactionId { get; set; }
        public int AccountHeaderId { get; set; }
        public DateTime PaymentDoneDate { get; set; }
    }

    public class FeeDetailsToExport
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public bool IsCredit { get; set; }
        public string StudentName { get; set; }
        public int ParentId { get; set; }
        public int SerialNo { get; set; }
        public int RowNo { get; set; }
        public string TransactionNumber { get; set; }
    }

    public class FeeLedger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }

    public class InternalFeeChequeDetails
    {
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }        
        public int BankId { get; set; }
        public int DepositedBankId { get; set; }
    }

    public class FeeStandards
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public int OriginalStandardId { get; set; }
        public string FeeType { get; set; }
        public string PayableFor { get; set; }
        public int Count { get; set; }
        public int PayableAmount { get; set; }
        public int OriginalFeeTypeId { get; set; }
        public int Type { get; set; }
        public int HeaderId { get; set; }
        public string HeaderName { get; set; }
        public char IsPrePrimary { get; set; }
    }
    public class InternalFeeElectronicDetails
    {
        public int BankId { get; set; }
        public int DepositedBankId { get; set; }
        public string TransactionNumber { get; set; }
        public int TypeId { get; set; }
    }

    public class StudentDetailsList
    {
        public int StudentId { get; set; }
        public int SchoolwiseStudentId { get; set; }
        public int OriginalStandardId { get; set; }
        public int OriginalDivisionId { get; set; }
        public int RollNo { get; set; }
        public string Class { get; set; }
        public string StudentName { get; set; }
    }

    public class StudentFeeDetailsList
    {
        public int StudentId { get; set; }
        public DateTime PaidDate { get; set; }
        public string ReceiptNumber { get; set; }
        public string FeeType { get; set; }
        public int Amount { get; set; }
        public string TransactionNumber { get; set; }
        public string PaymentMode { get; set; }
    }

    public class StudentCautionMoneyDetailsList
    {
        public int SchoolwiseStudentId { get; set; }
        public int CautionMoneyAmount { get; set; }
    }

    public class StudentFeeTypeConfigurationDetailsList
    {
        public int FeeTypeId { get; set; }
        public string FeeType { get; set; }
    }

    public class PaidFeeDetails
    {
        public List<StudentDetailsList> StudentDetailsList { get; set; }
        public List<StudentFeeDetailsList> StudentFeeDetailsList { get; set; }
        public List<StudentCautionMoneyDetailsList> StudentCautionMoneyDetailsList { get; set; }
        public List<StudentFeeTypeConfigurationDetailsList> StudentFeeTypeConfigurationDetailsList { get; set; }
    }

    public class TransactionStatusDetails
    {
        public int NetbankingTransactionId { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public int Amount { get; set; }
    }
}
