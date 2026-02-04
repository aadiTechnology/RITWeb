
using SchoolEntities;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System;
using MasterEntities;

namespace PayrollReportingUserEntities
{
    public class ReportingUserConfiguration : SchoolEntity
	{
        public int AcademicYearId { get; set; }
        public int ReportingId { get; set; }
        public int InsertedById { get; set; }
        public int ReportingTypeId { get; set; }
        public int RoleId { get; set; }
        public int SchoolId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ReportingPrameterId { get; set; }
        public string ReportingParameterName { get; set; }
	}   

   

    public class ReportingParameter
    {
        public int ReportingPrameterId { get; set; }
        public string ReportingParameterName { get; set; }
     
    }
}
