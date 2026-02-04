using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SchoolEntities
{
    public class ExternalStudentFee 
    {
        public int Id { get; set; }
        public int FinancialYearId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime Date { get; set; }        
        public string StudentName { get; set; }
        public string FeeType { get; set; }
        public int Amount { get; set; }
        public string MobileNo { get; set; }
        public int PaymentModeId { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; } 
        public DateTime ChequeDate { get; set; }
        public int? ChequeNo { get; set; }
        public int FeeId { get; set; }
        public int BankId { get; set; }
        public int TotalRowCount { get; set; }
        public int ReceiptNumber { get; set; }
        public int AccountHeaderId { get; set; }
        public int TypeId { get; set;}
        public string TransactionNo  { get; set;}
        public string ElectronicDetails { get; set; }
    }
}
