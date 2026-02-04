using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;


namespace StandardwiseAcademicYear
{
    [Serializable]
   public class StandardwiseAcademicYearEntity :SchoolEntity
    {       
        public int StandardwiseAcademicYearId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime SchoolReopeningDate { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
       
    }
   
}
