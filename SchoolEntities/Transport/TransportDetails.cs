// Class Name       :- TransportDetails
// Purpose          :- This class used to display,save and update transport charges.
// Date Of creation :- 8-Nov-2013
// Author Name      :- Pravin Shinde

namespace SchoolEntities.Transport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using StudentEntities;
    using SchoolEntities.Transport;
using SchoolEntities.StudentFee;

    /// <summary>
    /// This class is used to get and save transport details.
    /// </summary>
    public class TransportDetails
    {
        public int TravelerTransportId { get; set; }
        public int RowID { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public int TransportTypeId { get; set; }
        public string Address { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public bool IsHistoryExists { get; set; }
        public string TransportPickUpPersonBinaryPhoto { get; set; }
        public string ClassName { get; set; }
    }
    
    public class TransportRoutes
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }        
    }

    public class ShiftDetails
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public int JourneyTypeId { get; set; }
    }

    public class VehicleDetails
    {
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public string Name { get; set; }
    }
    
    public class PayTransportCharges
    {
        public int TransportFeeDetailsId { get; set; }
        public int UserId { get; set; }
        public string MonthName { get; set; }       
        public bool IsRefund { get; set; }
        public bool IsAutoRefund { get; set; }
        public bool IsArrears { get; set; }
        public bool IsConcession { get; set; }
        public bool IsLastCredit { get; set; }
        public StudentPayFeeDetails oStudentPayFeeDetails { get; set; }
        public StudentPaidFeeDetails oStudentPaidFeeDetails { get; set; }
    }

    public class TransportFeeDetails
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int PendingAmount { get; set; }
        public int TotalAmount { get; set; }
        public bool HasRefund { get; set; }
        public bool IsDeactivated { get;set;}
    }

    public class VehiclePurchaseOrHireDetails
    {
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
    }

    public class VehiclePassingDetails
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime PassingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int NotificationDays { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
        public int TotalRows { get; set; }
        public string VehicleNumber { get; set; }
        public bool IsAttachmentPresent { get; set; }
        public bool IsLocked { get; set; }
        public bool IsOldRecord { get; set; }
    }

    public class VehicleOptionDate
    {
        public int VehicleId { get; set; }
        public string PassingDate { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class TransportHistoryDetails
    {
        public int SrNo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }        
        public string Route { get; set; }
        public string Stop { get; set; }
        public string Shift { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime LeftDate { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
    }

    public class NotificationRequest
    {
        public string event_type { get; set; }
        public string thing_id { get; set; }
        public string journey_type { get; set; }
        public string journey_id { get; set; }
        public string message { get; set; }
        public string updated_at { get; set; }
    }

    public class StopMasterToSync
    {
        public int stopId { get; set; }
        public string stopName { get; set; }
        public string stopTime { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string landmark { get; set; }
        public int JourneyId { get; set; }
    }

    public class ShiftMasterToSync
    {
        public int journeyId { get; set; }
        public string journeyName { get; set; }
        public string journeyType { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string[] workingDays { get; set; }
        public bool disabled { get; set; }
        public List<StopMasterToSync> stops { get; set; }
    }

    public class VehicleMasterToSync
    {
        public string thingId { get; set; }
        public string thingName { get; set; }
        public List<ShiftMasterToSync> journeys { get; set; }
    }

    public class TrackingURLDetails
    {
        public string thing_id { get; set; }
        public string tracking_url { get; set; }
    }

    public class TransportOverrideDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserIds { get; set; }
        public int SourceRouteId { get; set; }
        public int SourceVehicleId { get; set; }
        public int SourceJourneyId { get; set; }
        public int TargetRouteId { get; set; }
        public int TargetVehicleId { get; set; }
        public int TargetJourneyId { get; set; }
        public int CategoryId { get; set; }
    }

    public class OverrideDetails
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SourceRoute { get; set; }
        public string SourceVehicle { get; set; }
        public string SourceJourney { get; set; }
        public string TargetRoute { get; set; }
        public string TargetVehicle { get; set; }
        public string TargetJourney { get; set; }
        public string Category { get; set; }
        public int TotalRows { get; set; }
        public int RowNo { get; set; }
        public int Id { get; set; }
    }

    public class NotificationDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MessageString { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class RouteShiftTimingOverrideDetails
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VehicleId { get; set; }
        public int RouteId { get; set; }
        public int JourneyTypeId { get; set; }
        public int JourneyId { get; set; }
        public string WeekdayIds { get; set; }
        public int TypeId { get; set; }
    }

    public class TransportServiceDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
