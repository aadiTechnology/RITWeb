using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace SchoolEntities.Transport
{
    public class VehicleMaintenanceExpenses : SchoolEntity
    {
        public int VehicleMaintenanceExpensesId { get; set; }
        public string MaintenanceDate { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public decimal MeterReading { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string WorkshopName{ get; set; }
        public string WorkDetails{ get; set; }
        public decimal Labour{ get; set; }
        public decimal TotalAmount{ get; set; }
        public string ExpiryDate { get; set; }
        public int MaintenanceTypeId { get; set; }
        public string MaintenanceType { get; set; }
        public string BillFileName { get; set; }
        public int SchoolId { get; set; }
        public int InsertedById { get; set; }
        public int AcademicYearId { get; set; }
        
    }

    public class VehicleMaintenancePartsUsed : SchoolEntity
    {
        public int VehicleMaintenancePartsUsedId { get; set; }
        public int VehicleMaintenanceExpensesId { get; set; }
        public string PartsUsed { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int InsertedById { get; set; }
    }

    public class Maintanance : SchoolEntity
    {
        public int MaintenanceTypeId { get; set; }
        public string MaintenanceType { get; set; }
    }
}
