using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class HealthDetails
    {
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public int Status { get; set; }
        public int StudentId { get; set; }
        public int IsSubmited { get; set; }
        public int IsLeft { get; set; }
    }

    public class StudentHealthDetails
    {
        public int StudentId { get; set; }
        public int ComponentId { get; set; }
        public string Component { get; set; }
        public int ParameterId { get; set; }
        public string Parameter { get; set; }
        public string Answer { get; set; }
        public string StudentName { get; set; }        
        public string EnrolmentNo { get; set; }
        public int RollNo { get; set; }
        public string ClassName { get; set; }
        public bool SubmitStatus { get; set; }
        public int IsDataSaved { get; set; }
    }

    public class ImportHealthDetails
    {
        public int StudentId { get; set; }
        public int RollNo { get; set; }
        public string EnrolmentNo { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public int FatherWeight { get; set; }
        public int MotherWeight { get; set; }
        public int FatherHeight { get; set; }
        public int MotherHeight { get; set; }
        public string FatherBG { get; set; }
        public string MotherBG { get; set; }
        public string FatherAadharCardNo { get; set; }
        public string MotherAadharCardNo { get; set; }
        public Decimal FamilyMonthlyIncome { get; set; }
        public string CWSN { get; set; }
        public int TotalRows { get; set; }
    }

    public class SiblingStudentDetails
    {
        public int YearwiseStudentId { get; set; }
        public int SiblingStudentId { get; set; }
        public string EnrolmentNumber { get; set; }
        public string SiblingEnrolmentNumber { get; set; }
    }
}
