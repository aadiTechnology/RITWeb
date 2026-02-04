using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.Transport
{
   public class NotificationDetailsForScreen
    {
       public int VehicleId { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public int TypeId { get; set; }
       public int RouteId { get; set; }
       public int JourneyId { get; set; }

       public int Id { get; set; }
       public string StudentName { get; set; }
       public string Standard_Name { get; set; }
       public string Division_Name { get; set; }
       public string VehicleNumber { get; set; }
       public DateTime CreateDate { get; set; }
       public string MessageString { get; set; }
    }

   public class Route
   {
       public int RouteId { get; set; }
       public string RouteName { get; set; }
   }

   public class JourneyDetails
   {
       public int TransportShiftId { get; set; }
       public string TransportShiftName { get; set; }
   }
}
