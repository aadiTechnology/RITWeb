using DataCommunicator;
using System.Collections.Generic;
using AccountsEntities;
namespace BusinessLogic
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class FinancialYearBL
	{
		FinancialYearDC moFinancialYearDC=new FinancialYearDC();

		/// <summary>
		/// This method is used to get all financial years.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public List<FinancialYear> GetAllFinancialYears(int aiSchoolId)
		{
			return AccountsDC.GetFinancialYears();
		}

		/// <summary>
		/// This method is used to create next financial year.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public bool CreateFinancialYear(int aiSchoolId, int aiUserId, bool abMarkAsCurrent)
		{
				return moFinancialYearDC.CreateFinancialYear(aiSchoolId,aiUserId,abMarkAsCurrent);
		}

		/// <summary>
		/// This method is used to update financial year details.
		/// </summary>
		/// <param name="asXml"></param>
		/// <returns></returns>
		public void UpdateFinancialYearDetails(string asXml, int aiUserId)
		{
			moFinancialYearDC.UpdateFinancialYearDetails(asXml, aiUserId);
		}

		
	}
}
