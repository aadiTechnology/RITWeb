using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.eStore
{
    public class StoreItemDetails : StoreItemBasic
    {
       public int StoreCategoryId { get; set; } 
       public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string AssociatedStandards { get; set; }
        public bool AvailabilitySetting { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ReOrderQuantity { get; set; }
        public string ImageFileNames { get; set; }
        public bool IsVariation { get; set; }
        public List<int> StandardList { get; set; }
        public List<Attachment> AttachmentsDetails { get; set; }
        public string FileIdsToDelete { get; set; }
        public bool AreVariationExists { get; set; }
        public int StoreItemVariationId { get; set; }
    }

   [Serializable]
   public class StoreItemBasic
   {
       public string ItemCode { get; set; }
       public int UOMId { get; set; }
       public int GSTCategoryId { get; set; }
       public string HSNCode { get; set; }
       public decimal MRP { get; set; }
       public decimal Discount { get; set; }
   }

   public class StoreItemCategory
   {
       public int Id { get; set; }
       public string Name { get; set; }
   }

   public class StandardList
   {
       public int Original_Standard_Id { get; set; }
       public string Standard_Name { get; set; }
   }

   public class ItemVariationDetails
   {
       public int Id { get; set; }
       public Boolean Select { get; set; }
       public string Title { get; set; }
       public decimal Price { get; set; }
       public int Quantity { get; set; }
       public string ImageFileName { get; set; }
       public int ReOrderQuantity { get; set; }
       public int ItemVariationId { get; set; }
       public bool IsDeleted { get; set; }
   }

   [Serializable]
   public class Attachment
   {
       public int Id { get; set; }
       public string ImageFileName { get; set; }
   }
        
}
