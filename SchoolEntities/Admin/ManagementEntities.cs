// -----------------------------------------------------------------------
//	FileName	: ManagementEntities.cs
//	Author		: Vishal Shah
//	Date		: 8-Nov-2012
//	Description	: Defines the entities which are used in the Management
//				  service and client.
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Management.Entities
{
    [Serializable]
		public class AcademicYear
		{
			public int Id { get; set; }
			public DateTime StartDate { get; set; }
			public DateTime EndDate { get; set; }
			public string DisplayValue { get { return String.Format("{0} - {1}", StartDate.Year, EndDate.Year); }}
			public bool IsCurrent { get; set; }
		}
    [Serializable]
		public class FinancialYear
		{
			public int Id { get; set; }
			public DateTime StartDate { get; set; }
			public DateTime EndDate { get; set; }
			public string DisplayValue { get { return String.Format("{0} - {1}", StartDate.Year, EndDate.Year); }}
			public bool IsCurrent { get; set; }
		}
    [Serializable]
		public class StudentMISDetails
		{
			public int TotalCount { get; set; }
			public int TotalAttendanceCount { get; set; }
		}
    [Serializable]
		public class StaffMISDetails
		{
			public int TotalCount { get; set; }
			public int TotalAttendanceCount { get; set; }
		}
     [Serializable]
		public class FeeMISDetails
		{
			public int Fees { get; set; }
			public int InternalFees { get; set; }
			public int CautionMoney { get; set; }
		}

		/// <summary>
		///		Represents a School entity and its details from the management service context.
		/// </summary>
      [Serializable]
		public class SchoolMISDetails
		{
			public int Id { get; set; }
			public int SchoolId { get; set; }
			public string SchoolName { get; set; }
			public string SchoolShortName { get; set; }
			public string ServiceURL { get; set; }
			public string Token { get; set; }
			public bool IsDefault { get; set; }
			public bool IsDeleted { get; set; }

			public List<AcademicYear> AcademicYears { get; set; }
			public List<FinancialYear> FinancialYears { get; set; }
			public StudentMISDetails StudentDetails { get; set; }
			public StaffMISDetails StaffDetails { get; set; }
			public FeeMISDetails FeeCollection { get; set; }
		}
}
