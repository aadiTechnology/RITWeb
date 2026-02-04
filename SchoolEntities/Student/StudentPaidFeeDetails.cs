using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities.StudentPaidFeeDetails
{
    class StudentPaidFeeDetails
    {
    }

    public class StudentDetails
    {
        public int YearwiseStudentId { get; set; }
        public int RollNo { get; set; }
        public string StudentName { get; set; }
        public string EnrolmentNumber { get; set; }
        public string ClassName { get; set; }
    }

    public class PayableForDetails
    {
        public string PayableFor { get; set; }
    }

    public class PaidFeeDetails
    {
        public int StudentId { get; set; }
        public string FeeType { get; set; }
        public string PayableFor { get; set; }
        public DateTime PaidDate { get; set; }
        public int Amount { get; set; }        
        public string ChequeNumber { get; set; }
    }

    public class StudentFeeDetails
    {
        public int StudentId { get; set; }
        public int PaidAmount { get; set; }
        public int PedingAmount { get; set; }
    }
}
