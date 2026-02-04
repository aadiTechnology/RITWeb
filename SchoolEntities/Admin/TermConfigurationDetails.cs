using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities; 

namespace TermEntities
{  
    [Serializable]
    public class TermConfigurationDetails
    {
        public int TermId { get; set; }
        public int SchoolwiseTermId { get; set; }
        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }
        public bool Is_Deleted { get; set; }
    }

    [Serializable]
    public class SchoolwiseTermConfigurationDetails
    {

        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public TermConfigurationDetails TermIInfo { get; set; }
        public TermConfigurationDetails TermIIInfo { get; set; }
    }

    [Serializable]
    public class StandardwiseAcademicYearDates
    {
        public int StandardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

