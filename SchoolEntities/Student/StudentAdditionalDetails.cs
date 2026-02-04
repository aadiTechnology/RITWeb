using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StudentAdditionalDetails
    {
        public int SchoolwiseStudentId { get; set; }
        public string StudentStatus { get; set; }
        public string AdmissionAcademicYear { get; set; }
        public string AdmissionStandard { get; set; }
        public bool IsHandicapped { get; set; }
        public string CurrentAcademicYear { get; set; }
        public string CurrentStandard { get; set; }
        public int PreviousYearMarksObtained { get; set; }
        public int PreviousYearMarksOutOff { get; set; }
        public string PreviousYearOfPassing { get; set; }
        public string StubjectNames { get; set; }
        public string Religion {get; set;}
	    public string BirthTaluka {get;set;}
	    public string BirthDistrict {get;set;}
	    public string HouseNoPlotNo {get;set;}
	    public string MainArea {get; set;}
	    public string SubareaName {get; set;}
	    public string  Landmark {get; set;}
	    public string Taluka {get; set;}
	    public string District {get;set;}
	    public int FeeAreaName {get; set;}
	    public string FatherOccupation {get;set;}
	    public string FatherQualification {get;set;}
	    public string FatherEmail {get;set;}
	    public string FatherOfficeName {get;set;}
	    public string FatherOfficeAddress {get;set;}
	    public string MotherOccupation {get;set;}
	    public string MotherQualification {get;set;}
	    public string MotherEmail {get; set;}
	    public string MotherOfficeName {get; set;}
        public string MotherOfficeAddress { get; set; }
        public DateTime FatherDOB { get; set; }
        public DateTime MotherDOB { get; set; }
        public string FatherDesignation { get; set; }
        public string MotherDesignation { get; set; }
        public string FatherPhoto { get; set; }
        public string MotherPhoto { get; set; }
        public string MotherAadharCardPhoto { get; set; }
        public string FatherAadharCardPhoto { get; set; }
        public DateTime MarriageAnniversaryDate { get; set; }
        public string GuardianPhoto { get; set; }        
        public string RelativeName { get; set; }
        public byte[] FatherBinaryPhoto { get; set; }
        public byte[] MotherBinaryPhoto { get; set; }
        public byte[] ParentBinaryPhoto { get; set; }
        public Boolean IsPhotosSubmitted { get; set; }
        public int FatherWeight { get; set;}
        public int MotherWeight { get; set; }
        public int FatherHeight { get; set; }
        public int MotherHeight { get; set; }
        public string FatherBloodGroup { get; set; }
        public string MotherBloodGroup { get; set; }
        public string FatherAadharCardNo { get; set; }
        public string MotherAadharCardNo { get; set; }
        public Decimal FamilyMonthlyIncome { get; set; }
        public string CWSN { get; set; }
        public string TransportPickUpPersonName { get; set; }
        public string TransportPickUpPersonPhoto { get; set; }
        public byte[] TransportPickUpPersonBinartPhoto { get; set; }
        //public int FatherAnnualIncome { get; set; }
        //public int MotherAnnualIncome { get; set; }
        public Decimal FatherAnnualIncome { get; set; }
        public Decimal MotherAnnualIncome { get; set; }
        public string BirthState { get; set; }
        public string Name1 { get; set; }
        public int Age1 { get; set; }
        public string Institute1 { get; set; }
        public string Standard1 { get; set; }
        public string Name2 { get; set; }
        public int Age2 { get; set; }
        public string Institute2 { get; set; }
        public string Standard2 { get; set; }
        public int ResisdenceTypeId { get; set; }
        public string RFID { get; set; }
        public string PenNo { get; set; }
        public string BirthCertificateFileName { get; set; }
        public DateTime StudentTransferDate { get; set; }
        public string ApaarId { get; set; }
    }
}
