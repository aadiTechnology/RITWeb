using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class ServiceReceiverDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string MobileNo { get; set; }
        public string GSTIN { get; set; }
        public int TotalRows { get; set; }
    }
}
