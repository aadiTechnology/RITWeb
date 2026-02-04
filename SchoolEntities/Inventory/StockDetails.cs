using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace NewStockDetails
{
    public class StockDetails : SchoolEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal price { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal OriginalItemQuantity { get; set; }
        public int ConsiderInUnitQuanity { get; set; }
        public string ItemQuantityWithUnits { get; set; }
        public int UOMPieceCount { get; set; }
        public int VendorId { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class StockItemDetails : SchoolEntity
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string CurrentStockUOM { get; set; }
        public decimal CurrentQuantity { get; set; }
        public string ItemQuantityWithUnits { get; set; }
    }
}
