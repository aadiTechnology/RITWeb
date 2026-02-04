
using SchoolEntities;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System;
namespace PayrollEntities
{	
	public class IncomeTaxSlab : SchoolEntity
	{
		public int Id { get; set; }		
		public ITSlabCategory Category { get; set; }
		public int FromAmount { get; set; }
		public int ToAmount { get; set; }
		public int FixedAmount { get; set; }
		public double Percentage { get; set; }
		public int FinancialYearId { get; set; }
		public int IsDeleted { get; set; }
	}
	
	public class ITSlabCategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public int FromAge { get; set; }
        public int UptoAge { get; set; }
	}
}
