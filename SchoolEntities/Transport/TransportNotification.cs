using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Transport
{
    public class TransportNotification
    {
        public string VehicleNumber { get; set; }
        public string ExpiryDate { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public int DetailsId { get; set; }
    }
}
