// File Name : RetirementNoticeBL.cs
// Creator : Sunny
// Created Date : 12-June-2013
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using PayrollEntities;

namespace BusinessLogic
{
	/// <summary>
	///  This class is used for processing business logic and communicate with data access layer.
	/// </summary>
	public class RetirementNoticeConfigBL
	{
		#region Data Member(s)

		private RetirementNoticeConfigDC moRetirementNoticeConfigDC;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Initializes a new instance of the <see cref="RetirementNoticeBL" /> class. 
		/// </summary>
		public RetirementNoticeConfigBL()
		{
			this.moRetirementNoticeConfigDC = new RetirementNoticeConfigDC();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RetirementNoticeBL" /> class. 
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinYearId"></param>
		/// <param name="aiUpdatedById"></param>
		/// <param name="aiAcademicYearId"></param>
		public RetirementNoticeConfigBL(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
		{
			this.moRetirementNoticeConfigDC = new RetirementNoticeConfigDC(aiSchoolId, aiFinYearId, aiAcademicYearId, aiUpdatedById);
		}

		#endregion

		#region Method(s)

		/// <summary>
		/// This method is used to return all retirement notices.
		/// </summary>
		/// <returns></returns>
		public List<RetirementNoticeConfiguration> GetAll()
		{
			return this.moRetirementNoticeConfigDC.GetAll();
		}

		/// <summary>
		/// This method is used to retrive retirement notice details for particular ID.
		/// </summary>
		/// <param name="aiIncomeTaxRangeId"></param>
		/// <returns></returns>
		public RetirementNoticeConfiguration Get(int aiRetNoticeConfigId)
		{
			return this.moRetirementNoticeConfigDC.Get(aiRetNoticeConfigId);
		}

		/// <summary>
		/// This method is used to insert/update retirement details. 
		/// </summary>		
		public void Save(RetirementNoticeConfiguration aoRetirementNotice)
		{
			this.moRetirementNoticeConfigDC.Save(aoRetirementNotice);
		}

		/// <summary>
		/// This method will be used to return users’ retirement details.
		/// </summary>
		/// <returns></returns>
		public List<StaffMemberRetirementNotice> GetAllStaffsRetirementNotices()
		{
			return moRetirementNoticeConfigDC.GetAllStaffsRetirementNotices();
		}

		#endregion
	}
}
