// -----------------------------------------------------------------------
//	FileName	: ManagementServiceConfigBL.cs
//	Author		: Vishal Shah
//	Date		: 8-Nov-2012
//	Description	: The BusinesLogic layer for Management service.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DataCommunicator;
using Management.Entities;
using Utility;
using System.Data;

namespace BusinessLogic
{

	/// <summary>
	///		BusinessLogic layer for the management service.
	/// </summary>
	public class ManagementServiceConfigBL
	{
		#region -- MEMBER(s) --

		private int miSchoolId;
		private ManagementServiceConfigDC moManagementDC;

		#endregion -- MEMBER(s) --

		#region -- CONSTRUCTOR(s) --

		/// <summary>
		///		Initializes the DC object.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		public ManagementServiceConfigBL(int aiSchoolId)
		{
			miSchoolId = aiSchoolId;
			moManagementDC = new ManagementServiceConfigDC(miSchoolId);
		}

		#endregion -- CONSTRUCTOR(s) --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		Initializes the token so incoming requests on the management service can be validated.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		public void InitializeToken()
		{
			try
			{
				if (Constants.MANAGEMENT_TOKEN.IsNullOrEmpty())
					Constants.MANAGEMENT_TOKEN = moManagementDC.GetToken() ?? String.Empty;
			}
			catch (Exception)
			{
				Constants.MANAGEMENT_TOKEN = String.Empty;
			}
		}

		/// <summary>
		///		Fetches all associated schools.
		/// </summary>
		/// <returns></returns>
		public List<SchoolMISDetails> GetAssociatedSchools()
		{
			return moManagementDC.GetAssociatedSchools();
		}

		#endregion -- PUBLIC METHOD(s) --

        public static DataTable GetManagementUserInfo(int aiSchoolId, int aiUserId)
        {
            return ManagementServiceConfigDC.GetManagementUserInfo(aiSchoolId, aiUserId);
        }
    }
}
