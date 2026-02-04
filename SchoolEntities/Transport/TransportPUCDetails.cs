using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Transport
{
    public class TransportPUCDetails
    {
        public int VehicalId { get; set; }
        public string VehicalNumber { get; set; }
        public string SerialNumber { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int NoticicationDays { get; set; }
        public string DocumnetPhoto { get; set; }
        public string PUCNote { get; set; }
        public int TotalRows { get; set; }
        public int VehiclePUCId { get; set; }
        public bool IsFileExists { get; set; }
        public bool IsLocked { get; set; }
        public bool IsOldRecord { get; set; }
    }

    public class TransportOptionImages
    {
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int VehicleId { get; set; }
        public string Vehicle { get; set; }
        public string Images { get; set; }
        public int DetailId { get; set; }
    }
}
