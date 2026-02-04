using SchoolEntities;
using System;

namespace TransportEntities
{
    [Serializable]
    public class VehicleDocument
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string DocumentName { get; set; }
    }

    public class Documents
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
    }

    public class GetVehicleDocumentDetails
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int DocumentId { get; set; }
        public string PolicyNo { get; set; }
        public int TotalRows { get; set; }
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
    }

    public class VehicleDocumentList
    {
        public string Invoice { get; set; }
        public string PermitArea { get; set; }
        public string Insurance { get; set; }
        public string RCBook { get; set; }
        public string PUC { get; set; }
        public string FitnessCertificate { get; set; }
        public string SpeedGovernorCertificate { get; set; }
        public string GPSCertificate { get; set; }
        public string MVTaxCertificate { get; set; }
        public string GreenTaxCertificate { get; set; }
        public string FireExtinguishCertificate { get; set; }
        public string BarcodeCertificate { get; set; }
    }

    public class VehicleDocumentDetails
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FileName { get; set; }
        public string PolicyNo { get; set; }
        public int Amount { get; set; }
    }

}
    