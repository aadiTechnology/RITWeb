using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Transport
{
    public class TransportCapacityDetails
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int RouteNo { get; set; }
        public string RouteName { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleCapacity { get; set; }
        public int PickUpCount_A { get; set; }
        public int PickUpCount_B { get; set; }
        public int PickUpCount_C { get; set; }
        public int DropCount_A { get; set; }
        public int DropCount_B { get; set; }
        public int DropCount_C { get; set; }
    }

    public class StandardwiseCapacityDetails
    {
        public string StandardName { get; set; }
        public int Count { get; set; }
        public string VehicleNumber { get; set; }
        public int JourneyTypeId { get; set; }
        public string JourneyName { get; set; }
    }
}
