using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities.Transport
{
    public class TransportOverrideConfigDetails
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SourceRoute { get; set; }
        public string SourceVehicle { get; set; }
        public string SourceJourney { get; set; }
        public int TotalRows { get; set; }
        public int RowNo { get; set; }
        public int Id { get; set; }
        public string WeekdayIds { get; set; }
    }
}
