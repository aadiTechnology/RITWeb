using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Transport
{
    /// <summary>
    /// 
    /// </summary>
  public class TransportReadingAllocationDetails : SchoolEntity
    {
        public int VehicleReadingAllocationId { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime ReadingDate { get; set; }
        public double ReadingFrom { get; set; }
        public double ReadingTo { get; set; }
        public string ReceiptNumber{ get; set;}
        public decimal Litters { get; set; }
        public decimal PerLitterCost { get; set; }
        public decimal TotalCost { get; set; }
        public string FuelStationName { get; set; }
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }
        public int InsertedById { get; set; }
    }
}
