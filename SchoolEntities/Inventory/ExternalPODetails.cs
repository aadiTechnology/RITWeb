using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
   public class ExternalPODetails
    {
       public string PONo { get; set; }
       public int PODId { get; set; }
       public int ExternalPOUserId { get; set; }
       public int SchoolId { get; set; }
       //public int Amount { get; set; }
       //public string Description { get; set; }
       public int ReceiverId { get; set; }
       public DateTime PODate { get; set; }
       public decimal TotalAmount { get; set; }
       public int Id { get; set; }
       public int GSTCategoryId { get; set; }
       public int UpdatedById { get; set; }
       public int TotalRows { get; set; }
       public string ReceiverName { get; set; }
       public List<ExternalPODescription> Descriptions { get; set; }
       public decimal GST { get; set; }
       public decimal GrandTotal { get; set; }
       public int Quantity { get; set; }
       public int Rate { get; set; }
       public string Subject { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public string InstructionIds { get; set; }
       public int ExternalPOMasterId { get; set; }
       public int ExternalPOInstructionId { get; set; }
       public string PreparedBy { get; set; }
       public bool IsPO { get; set; }
       public string AdditionalRemarks { get; set; }

       public List<int> InstructionList { get; set; }
       public string Status { get; set; }
       public int StatusId { get; set; }
       public string Comment { get; set; }
       public decimal TotalPaidAmount { get; set; }
     }

   public class ExternalPODescription
   {
       public int Id { get; set; }
       public int PODId { get; set; }
       public string Description { get; set; }
       public decimal Amount { get; set; }
       public int Quantity { get; set; }
       public decimal Rate { get; set; }
       public string Name { get; set; }
       public int GSTCategoryId { get; set; }
       public decimal GST { get; set; }
   }

   public class POExternalUser : SchoolEntity
   {
       public string Name { get; set; }
       public int ReceiverId { get; set; }
   }

    public class Instruction : SchoolEntity
   {
       public int Id { get; set; }
       public string InstructionName { get; set; }
       public int InstCategoryId { get; set; }
   }

    public class POExternalCategory : SchoolEntity
    {
        public int Id { get; set; }
        public string Category { get; set; }
    }

    public class POInstructionDetails
    {
        public List<Instruction> Instructions { get; set; }
        public List<POExternalCategory> Categories { get; set; }
    }

    public class ExternalOrderPrefix
    {
        public string POPrefix { get; set; }
        public string WOPrefix { get; set; }
    }

    public class POFeePayment
    {
        public string PaymentMode { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string TxnNo { get; set; }
        public string Type { get; set; }
        public string BankName { get; set; }

        public int BankId { get; set; }
        public int PaymentModeId { get; set; }
        public int TypeId { get; set; }
        public int Id { get; set; }
        public DateTime ChequeDate { get; set; }
        public int POMasterId { get; set; }
        public string Remark { get; set; }
    }
}
