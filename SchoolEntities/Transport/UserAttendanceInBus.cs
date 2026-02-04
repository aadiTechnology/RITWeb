using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportEntities;

namespace SchoolEntities.Transport
{
   public class UserAttendanceInBus
    {
       public int Id { get; set; }
       public string StudentName { get; set; }
       public string Standard_Name { get; set; }
       public string Division_Name { get; set; }
       public string RouteName { get; set; }
       public string JourneyName { get; set; }
       public string VehicleNo { get; set; }
       public string JourneyType { get; set; }
       public string PunchingDateTime { get; set; }
       public string Comment { get; set; }
       public string Location { get; set; }
       public int TotalRows { get; set; }
       public string LocationURL { get; set; }
       public string IsOnBoardingNotificationSent { get; set; }
       public string IsGeofenceNotificationSent { get; set; }
       public string IsOffBoardingNotificationSent { get; set; }
       public bool IsVehicleChanged { get; set; }
       public bool IsJourneyChanged { get; set; }
    }

   public class Vehicle : SchoolEntity
   {
       public int Value_Member { get; set; }
       public string Display_Member { get; set; }
   }

   public class Journey : SchoolEntity
   {
       public int Value_Member { get; set; }
       public string Display_Member { get; set; }
   }
}
