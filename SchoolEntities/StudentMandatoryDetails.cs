using System;

namespace SchoolEntities
{
    public class StudentMandatoryDetails
    {
        public string FatherMobileNumber { get; set; }
        public string MotherMobileNumber { get; set; }
        public string EmergencyContact { get; set; }
        public string BloodGroup { get; set; }

        public int TransportMode { get; set; }
        public string RouteNo { get; set; }
        public string StopName { get; set; }
        public string ContractorName { get; set; }
        public string ContractorContactNo { get; set; }

        public bool IsSaved { get; set; }
        public bool IsSubmitted { get; set; }
    }
}

