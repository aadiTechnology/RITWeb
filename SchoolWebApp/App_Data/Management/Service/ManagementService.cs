using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Management.Entities;
using SchoolBusinessService;
using Utility;

namespace Management.Service
{
	public class ManagementService : IManagementService
	{
		#region -- MEMBER(s) --

		private int miSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

		#endregion -- MEMBER(s) --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		Accepts a subscription request from another school (integrates both schools with each other).
		/// </summary>
		/// <param name="asSchoolName"></param>
		/// <param name="asServiceURL"></param>
		/// <param name="asToken"></param>
		public string Subscribe(string asSchoolName, string asServiceURL, string asToken)
		{
			return Guid.NewGuid().ToString("D");

		}

		/// <summary>
		///		Unsubscribes respective school.
		/// </summary>
		/// <param name="asSchoolName"></param>
		/// <param name="asServiceURL"></param>
		/// <param name="asToken"></param>
		public string UnSubscribe(string asSchoolName, string asServiceURL, string asToken)
		{
			return Guid.NewGuid().ToString("D");
		}

		/// <summary>
		///		Gets a list of all academic years in the school.
		/// </summary>
		/// <param name="asToken"></param>
		/// <returns></returns>
		public List<AcademicYear> GetAcademicYears(string asToken)
		{
			try
			{
				ValidateToken(asToken);
				
				var oAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
				DataTable dtAcademicYearInfo = oAcademicYearBL.GetAllAcademicYearsForSchool(miSchoolId, Constants.I_ZERO, Constants.I_ZERO);
				if (dtAcademicYearInfo.Rows.Count > 0)
				{
					return (from DataRow row in dtAcademicYearInfo.Rows
							select new AcademicYear
								{
									Id		  = row["Academic_Year_Id"].ToInt(),
									StartDate = row["Start_Date"].ToDateTime(),
									EndDate	  = row["End_Date"].ToDateTime()
								}
							).ToList();
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}

			return null;
		}

		/// <summary>
		///		Gets a list of all financial years in the school.
		/// </summary>
		/// <param name="asToken"></param>
		/// <returns></returns>
		public List<FinancialYear> GetFinancialYears(string asToken)
		{
			AccountsBaseClient oAccountsBase = null;
			
			try
			{
				ValidateToken(asToken);
				
				// If accounts module is disabled, we return null.
				if (!SchoolBase.Settings.EnableAccountsModule)
					return new List<FinancialYear>();

				oAccountsBase = new AccountsBaseClient();
				oAccountsBase.Open();
				List<AccountsEntities.FinancialYear> lstFinancialYears = oAccountsBase.GetAllFinancialYears(miSchoolId);
				return (from fy in lstFinancialYears
						select new FinancialYear
							{
								Id		  = fy.FinancialYearId,
								StartDate = fy.StartDate,
								EndDate   = fy.EndDate
							}
						).ToList();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
			finally
			{
				if (oAccountsBase != null && oAccountsBase.State != CommunicationState.Faulted)
					oAccountsBase.Close();
			}

			return new List<FinancialYear>();
		}

		/// <summary>
		///		Returns student count and attendance info as per <paramref name="adtAttendanceDate"/>.
		/// </summary>
		/// <param name="asToken"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="adtAttendanceDate"></param>
		/// <returns></returns>
		public StudentMISDetails GetStudentDetails(string asToken, int aiAcademicYearId, DateTime adtAttendanceDate)
		{
			try
			{
				ValidateToken(asToken);
				
				var oStudentBL = new StudentCollectionBL(miSchoolId, aiAcademicYearId);
				return oStudentBL.GetStudentAttendanceDetails(adtAttendanceDate);
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}

			return null;
		}

		/// <summary>
		///		Returns staff count as per <paramref name="adtAttendanceDate"/>
		/// </summary>
		/// <param name="asToken"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="adtAttendanceDate"></param>
		/// <returns></returns>
		public StaffMISDetails GetStaffDetails(string asToken, int aiAcademicYearId, DateTime adtAttendanceDate)
		{
			try
			{
				ValidateToken(asToken);
				
				var oSchoolBL = new SchoolBL(miSchoolId);
				return new StaffMISDetails
					{
						TotalCount = oSchoolBL.GetStaffCount(aiAcademicYearId),
						TotalAttendanceCount = 0
					};
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}

			return null;
		}

		/// <summary>
		///		Gets Fee collection details, optionally including internal fees and caution money.
		/// </summary>
		/// <param name="asToken"></param>
		/// <param name="aiAcademicYear"></param>
		/// <param name="abIncludeInternalFees"></param>
		/// <param name="abIncludeCautionMoney"></param>
		/// <returns></returns>
		public FeeMISDetails GetFeeCollection(string asToken, int aiAcademicYear, bool abIncludeInternalFees, bool abIncludeCautionMoney)
		{
			try
			{
				ValidateToken(asToken);
				
				var oFeeDetailsBL = new StudentFeeDetailsBL();
				var oFeeDetails = oFeeDetailsBL.GetFeeCollectionDetails(miSchoolId, aiAcademicYear, abIncludeInternalFees, abIncludeCautionMoney);
				return new FeeMISDetails
					{
						Fees = oFeeDetails.Fees,
						InternalFees = oFeeDetails.InternalFees,
						CautionMoney = oFeeDetails.CautionMoney
					};
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}

			return null;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="asToken"></param>
		/// <param name="aiFinancialYear"></param>
		/// <returns></returns>
		public List<AccountsEntities.MISReportSection> GetMISReport(string asToken, int aiFinancialYear)
		{
			try
			{
				ValidateToken(asToken);

				AccountsBaseClient oAccountsBaseClient = null;
				try
				{
					oAccountsBaseClient = new AccountsBaseClient();
					oAccountsBaseClient.Open();
					return oAccountsBaseClient.GetMISReport(miSchoolId, aiFinancialYear);
				}
				catch (Exception ex)
				{
					ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : Exception occurred while binding MIS Report.");
				}
				finally
				{
					if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
						oAccountsBaseClient.Close();
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}

			return null;
		}

		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		///		Validates the given token.
		/// </summary>
		/// <param name="asToken"></param>
		/// <exception cref="ArgumentException">If the given token does not match the local token.</exception>
		private void ValidateToken(string asToken)
		{
			if (Constants.MANAGEMENT_TOKEN == String.Empty)
			{
				var oMgmtServiceConfigBL = new ManagementServiceConfigBL(miSchoolId);
				oMgmtServiceConfigBL.InitializeToken();
			}

			if (Constants.MANAGEMENT_TOKEN != asToken)
				throw new ArgumentException(String.Format("Invalid token specified - {0}", asToken));
		}

		#endregion -- PRIVATE METHOD(s) --
	}
}
