using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Transport
{
    public class VehicleServicingDetails
    {
        public int VehicalId { get; set; }
        public string VehicalNumber { get; set; }
        public int SerialNumber { get; set; }
        public DateTime ServicingDate { get; set; }
        public DateTime NextServicingDate { get; set; }
        public int NotificationDays { get; set; }
        public string DocumnetPhoto { get; set; }
        public string ServicingNote { get; set; }
        public int TotalRows { get; set; }
        public int VehicleServicingId { get; set; }
        public bool IsFileExists { get; set; }
        public bool IsLocked { get; set; }
        public bool IsOldRecord { get; set; }
    }

    public class TransportNotificationDetails
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string MobileNos { get; set; }
        public string UserNames { get; set; }
        public int AcademicYearId { get; set; }
    }
}
