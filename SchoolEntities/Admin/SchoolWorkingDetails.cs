using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class SchoolWorkingStandardDetails
    {        
      public int StandardId { get; set; }      
      public string StandardName { get; set; }      
    }

    public class SchoolWorkinDivisionDetails
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }        
    }

    public class SchoolWorkingStdDivDetails
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public int OriginalStandardId { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public int OriginalDivisionId { get; set; }
        public int StandardDivisionId { get; set; }
    }

    public class SchoolWorkingDetails
    {
        public int HalfDayDetailsId { get; set; }
        public int StandardId { get; set; }
        public int OriginalStandardId { get; set; }
        public int DivisionID { get; set; }
        public int OriginalDivisionId { get; set; }
        public DateTime HalfDayDate { get; set; }
        public string ClassName { get; set; }
    }

    public class WorkinghrsDetails 
    {

        public string StandardName { get; set; }
        public string DivisionName { get; set; }
        public int DivisionId { get; set; }
        public int WeekdayNumber { get; set; }
        public decimal FullHours { get; set; }
        public decimal HalfHours { get; set; }

    }
}
