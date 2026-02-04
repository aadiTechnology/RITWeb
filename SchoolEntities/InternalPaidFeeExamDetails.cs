using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
   
    public class InternalPaidFeeExamDetails
{
       

    public List<StudentItem> StudentList { get; set; }
    public List<PayableItem> DebitPayables { get; set; }
    public List<CreditItem> CreditEntries { get; set; }
}

public class StudentItem
{
    public int SchoolwiseStudentId { get; set; }
    public int RollNo { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public string MobileNumber { get; set; }
    public string ClassName { get; set; }
}

public class PayableItem
{
    public string PayableFor { get; set; }
}

public class CreditItem
{
    public int SchoolwiseStudentId { get; set; }
    public string PayableFor { get; set; }
}
}