using System;
using System.Collections.Generic;

namespace SchoolEntities.eStore
{

    public class StoreItemStockMaster
    {
        public int TotalRows { get; set; }        
        public decimal NetPrice { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }        
        public decimal AdjustedAmount { get; set; }
        public decimal TransportAmount { get; set; }
        public decimal TotalAmount { get; set; }        
        public int StockMasterId { get; set; }
        public string StockDetails { get; set; }
    }

    [Serializable]
    public class StoreItemStockDetails
    {
        public int Id { get; set; }
        public int TotalRows { get; set; }
        public int ItemMasterId { get; set; }
        public int ItemVariationDetailId { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal NewQuantity { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ItemCode { get; set; }
        public string Title { get; set; }
        public string UOM { get; set; }
        public int GSTCategoryId { get; set; }
        public string GST { get; set; }   
    }

    public class StoreItemStock
    {
        public StoreItemStockMaster StockMaster { get; set; }
        public List<StoreItemStockDetails> StockDetails { get; set; }
    }
}
