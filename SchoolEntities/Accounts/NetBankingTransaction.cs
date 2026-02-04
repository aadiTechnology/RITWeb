// File Name  : NetBankingTransaction.cs
// Created By : Pravin 
// Date       : 28/05/2013
//Description : This class is used to handle the online transactions. e.g.Feepayment / Admission

namespace SchoolEntities.Accounts
{
    using Utility;
    /// <summary>
    /// This class is used to intialize the online transaction details.
    /// </summary>
    public class NetBankingTransaction
    {
        public int NetBankingPaymentTransactionID;
        public string PaymentReferenceNumber;
        public string PaymentITCParameter;
        public double TransactionAMT;
        public string TransactionBankID;
        public Constants.TransactionStatus TransactionStatus;
        public string TPSLTransactionID;
        public bool IsNextAcademicYear;
        public int TransactionFor;
        public int GatewayId;
        public int ConcessionAmount;
        public string StatusCode;
    }

    public class PaymentGateWayDetails
    {
        public int GatewayId { get; set; }
        public string PaymentGateway { get; set; }
        public string NetBankingUrl { get; set; }
        public string Version { get; set; }
        public string Command { get; set; }
        public string AccessCode { get; set; }
        public string MerchantTxnRefNumber { get; set; }
        public string MerchantId { get; set; }        
        public string Locale { get; set; }
        public string Hash { get; set; }
        public string Sequence { get; set; }
        public string SuccessCode { get; set; }
        public bool HasBankSelection { get; set; }

        public string ProductInfo { get; set; }
    }

    public class OnlinePaymentType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class StudentNetBankingDetails
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPreprimaryStudent { get; set; }
        public string SchoolEmailAddress { get; set; }
        public string RegNoOrFormNo { get; set; }
    }
}
