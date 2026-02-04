using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;

namespace SchoolEntities
{
    public class CancellationForm
    {
        public int SchoolWiseStudentId { get; set; }
        public string Reason { get; set; }
        public string RefundChequeInFavourOf { get; set; }
        public string Cell { get; set; }
        public string StudentName { get; set; }
        public int Id { get; set; }
    }

    public class SearchStudentDetails
    {
        public string EnrolmentNumber { get; set; }
        public int RollNo { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public int TotalRows { get; set; }
        public int SchoolWiseStudentId { get; set; }
        public int Id { get; set; }
    }

    public class CancellationFormStudentDetails
    {
        public string Enrolment_Number { get; set; }
        public int Roll_No { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int SchoolWiseStudentId { get; set; }
        public int StandardId { get; set; }
        public int DivisionId { get; set; }
        public int StudentId { get; set; }
        public int SubmittedBy { get; set; }
    }
}
