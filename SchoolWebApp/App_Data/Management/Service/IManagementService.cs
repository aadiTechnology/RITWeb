using System;
using System.Collections.Generic;
using System.ServiceModel;
using Management.Entities;

namespace Management.Service
{
	[ServiceContract(Namespace="http://www.riteschool.com")]
	public interface IManagementService
	{
		[OperationContract]
		string Subscribe(string asSchoolName, string asServiceURL, string asToken);

		[OperationContract]
		string UnSubscribe(string asSchoolName, string asServiceURL, string asToken);

		[OperationContract]
		List<AcademicYear> GetAcademicYears(string asToken);

		[OperationContract]
		List<FinancialYear> GetFinancialYears(string asToken);

		[OperationContract]
		StudentMISDetails GetStudentDetails(string asToken, int aiAcademicYearId, DateTime adtAttendanceDate);

		[OperationContract]
		StaffMISDetails GetStaffDetails(string asToken, int aiAcademicYearId, DateTime adtAttendanceDate);

		[OperationContract]
		FeeMISDetails GetFeeCollection(string asToken, int aiAcademicYear, bool abIncludeInternalFees, bool abIncludeCautionMoney);

		[OperationContract]
		List<AccountsEntities.MISReportSection> GetMISReport(string asToken, int aiFinancialYearId);
	}
}
