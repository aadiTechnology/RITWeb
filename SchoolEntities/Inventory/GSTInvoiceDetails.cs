using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
   public class GSTInvoiceDetails
    {

       public string InvoiceNo { get; set; }
       public int GSTInvoiceId { get; set; }
       public int ServiceReceiverId { get; set; }
       public int SchoolId { get; set; }
       //public int Amount { get; set; }
       //public string Description { get; set; }
       public int ReceiverId { get; set; }
       public DateTime InvoiceDate { get; set; }
       public decimal TotalAmount { get; set; }
       public int Id { get; set; }
       public int GSTCategoryId { get; set; }
       public int UpdatedById { get; set; }
       public int TotalRows { get; set; }
       public string ReceiverName { get; set; }
       public List<GSTInvoiceDescription> Descriptions { get; set; }
       public decimal CGST { get; set; }
       public decimal SGST { get; set; }
       public decimal FinalAmount { get; set; }
       public string AdditionalRemark { get; set; }
    }

   public class GSTInvoiceDescription
   {
       public int Id { get; set; }
       public int GSTInvoiceId { get; set; }
       public string Description { get; set; }
       public int Amount { get; set; }
   }

   public class ReceiverName : SchoolEntity
   {
       public string Name { get; set; }
       public int ReceiverId { get; set; }
   }

   [Serializable]
   public class GSTCategory : SchoolEntity
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public decimal Percentage { get; set; }
   }
}
