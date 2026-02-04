using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class AdmissionDetails
    {
        public int Id { get; set; }
        public string FormNumber { get; set; }
        public string StudentName { get; set; }
        public string CurrentStatus { get; set; }
        public int StatusId { get; set; }
        public DateTime DOB { get; set; }
        public string EmailAddress { get; set; }
        public string AcademicYear { get; set; }
        public int SalutationId { get; set; }
        public int AcademicYearId { get; set; }
        public int ReceiptNumber { get; set; }
    }

    public class AdmissionStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class EnquiryStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class AdmissionStatusDetails
    {
        public int Id { get; set; }
        public int StudentAdmissionId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public DateTime FollowUpDate { get; set; }
        public int StatusId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class StudentLivingLocation
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
    }

    public class SiblingFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EnquiryReference
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class EnquiryStatusDetails
    {
        public int Id { get; set; }
        public string EnquiryId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public DateTime FollowUpDate { get; set; }
        public int StatusId { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class EnquiryDetails
    {
        public int Id { get; set; }
        public string FormNumber { get; set; }
        public string StudentName { get; set; }
        public string CurrentStatus { get; set; }
        public int StatusId { get; set; }
        public DateTime DOB { get; set; }
        public string EmailAddress { get; set; }
        public string AcademicYear { get; set; }
        public int SalutationId { get; set; }
    }

    public class StudentRegistration
    {
        public int AdmissionId { get; set; }

        public string EnquiryNo { get; set; }
        public int AdmissinAcademicYearId { get; set; }
        public int standardId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public string BirthTaluka { get; set; }
        public string BirthDistrict { get; set; }
        public string LastSchoolName { get; set; }
        public string HouseName { get; set; }
        public string LandMark { get; set; }
        public string MainArea { get; set; }
        public string City { get; set; }
        public string Taluka { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string FFirstName { get; set; }
        public string FMiddleName { get; set; }
        public string FLastName { get; set; }
        public string MFirstName { get; set; }
        public string MMiddleName { get; set; }
        public string MLastName { get; set; }
        public string FQualification { get; set; }
        public string MQualification { get; set; }
        public int FOccupation { get; set; }
        public int MOccupation { get; set; }
        public string FOrgAddress { get; set; }
        public string MOrgAddress { get; set; }
        public string FPhoneNumber { get; set; }
        public string MPhoneNumber { get; set; }
        public string FMobNumber { get; set; }
        public string MMobNumber { get; set; }
        public string FEmail { get; set; }
        public string MEmail { get; set; }
        public string BName1 { get; set; }
        public int BAge1 { get; set; }
        public string BInstituteName1 { get; set; }
        public string BStandard1 { get; set; }
        public string BName2 { get; set; }
        public int BAge2 { get; set; }
        public string BInstituteName2 { get; set; }
        public string BStandard2 { get; set; }
        public string AadharCardNumber { get; set; }
        public int ManualReceiptNo { get; set; }
    }
}
