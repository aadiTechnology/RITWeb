using SchoolEntities;
using System;
using System.Collections.Generic;

namespace TransportEntities
{
    public class GetBulkDocumentDetails
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public string PolicyNo { get; set; }
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public string DocumentName { get; set; } 
        public int DocumentId { get; set; }
        //public List<BulkDocumentDetails> DocumentId { get; set; }
    }

    public class BulkDocumentDetails
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public string PolicyNo { get; set; }
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public int DocumentId { get; set; }
        public string DocumentFilePath { get; set; }
        public int ActionId { get; set; }
        //public List<BulkDocumentDetails> Id { get; set; }
    }

    [Serializable]
    public class Vehicles : SchoolEntity
    {
        public int Value_Member { get; set; }
        public string Display_Member { get; set; }
    }
}
