using System;
using System.Collections.Generic;

namespace SchoolEntities.StudentFee.FeeReport
{
    public class FeeReport
    {
        public List<FeeType> FeeTypes { get; set; }
        public List<SchooolwiseStudentFeeDetailss> SchooolwiseStudentFeeDetailss { get; set; }
        public List<PaidFeeDetails> PaidFeeDetails { get; set; }
        public List<TransportDetails> TransportDetails { get; set; }
        public List<StudentInfo> StudentInfo { get; set; }
        public List<PayableSummary> PayableSummaryDetails { get; set; }
    }

    public class FeeType
    {
        public int OrgFeeTypeId { get; set; }
        public string Name { get; set; }
    }

    public class SchooolwiseStudentFeeDetailss
    {
        public int SchoolwiseStudentFeeId { get; set; }
        public int YearwiseStudentId { get; set; }
        public string FeeType { get; set; }
        public string PayableFor { get; set; }
        public int Amount { get; set; }
    }

    public class PaidFeeDetails
    {
        public int YearwiseStudentId { get; set; }
        public int StudentFeeId { get; set; }
        public int Amount { get; set; }
        public DateTime PaidDate { get; set; }
        public string ReceiptNumber { get; set; }
        public string AdditionalRemark { get; set; }
        public string TransactionId { get; set; }
        public DateTime ChequeDate { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string CreatedBy { get; set; }
        public string FeeType { get; set; }
        public string PayableFor { get; set; }
        public int ConcessionAmount { get; set; }
    }

    public class TransportDetails
    {
        public int UserId { get; set; }
        public string PickupRoute { get; set; }
        public string PickupStop { get; set; }
        public string DropRoute { get; set; }
        public string DropStop { get; set; }
    }

    public class StudentInfo
    {
        public int UserId { get; set; }
        public int YearwiseStudentId { get; set; }
        public int OrgStdId { get; set; }
        public int OrdDivId { get; set; }
        public string Class { get; set; }
        public string EnrolmentNo { get; set; }
        public string StudentName { get; set; }
        public int RollNo { get; set; }
        public string FeeCategory { get; set; }
        public string Status { get; set; }
    }

    public class PayableSummary
    {
        public int YearwiseStudentId { get; set; }
        public string PayableFor { get; set; }
        public int TotalAmount { get; set; }
    }
}
