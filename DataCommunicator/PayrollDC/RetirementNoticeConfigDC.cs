// -----------------------------------------------------------------------
// File Name : RetirementNoticeDC.cs
// Creator : Sunny
// Created Date : 12-June-2013
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using MasterEntities;

namespace DataCommunicator
{

	/// <summary>
	///This class is used to communicate with database to insert,update and select retirement notice configuration.
	/// </summary>
	public class RetirementNoticeConfigDC
	{
		#region Data Member(s)

		private int miSchoolId;
		private int miFinYearId;
		private int miAcademicYearId;
		private int miUpdatedById;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public RetirementNoticeConfigDC()
		{
		}

		/// <summary>
		/// Initializes member variables.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinYearId"></param>
		/// <param name="aiUpdatedById"></param>
		/// <param name="aiAcademicYearId"></param>
		public RetirementNoticeConfigDC(int aiSchoolId, int aiFinYearId, int aiAcademicYearId, int aiUpdatedById)
		{
			this.miSchoolId = aiSchoolId;
			this.miFinYearId = aiFinYearId;
			this.miAcademicYearId = aiAcademicYearId;
			this.miUpdatedById = aiUpdatedById;
		}

		#endregion

		#region Public Method(s)

		/// <summary>
		/// This method is used to return all retirement Notices.
		/// </summary>
		/// <returns></returns>
		public List<RetirementNoticeConfiguration> GetAll()
		{
			List<RetirementNoticeConfiguration> lstRetirementNoticeConfig = new List<RetirementNoticeConfiguration>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStaffsRetirementConfigs"))
				{
					while (oSqlDataReader.Read())											
						lstRetirementNoticeConfig.Add(ReadObjectFromReader(oSqlDataReader));					
				}
			}
			return lstRetirementNoticeConfig;
		}

		/// <summary>
		/// This method is used to retrive retirement notice details for particular ID.
		/// </summary>
		/// <param name="aiIncomeTaxRangeId"></param>
		/// <returns></returns>
		public RetirementNoticeConfiguration Get(int aiRetNoticeConfigId)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("RetirementNoticeConfigId", aiRetNoticeConfigId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSingleStaffRetirementNoticeConfig"))
				{
					if (oSqlDataReader.Read())					
						return ReadObjectFromReader(oSqlDataReader);					
				}
			}
			return null;
		}

		/// <summary>
		/// This method is used to insert/update retirement notice configuration. 
		/// </summary>
		/// <param name="aoIncomeTaxSlab"></param>
		public void Save(RetirementNoticeConfiguration aoRetirementNotice)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("ConfigId", aoRetirementNotice.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserRoleId", aoRetirementNotice.UserRole.Id, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("RetirementAge", aoRetirementNotice.RetirementAge, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("ReminderDays", aoRetirementNotice.ReminderDays, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveRetirementNoticeConfig");
			}
		}

		/// <summary>
		/// This method is used to return all the staff members retirement notices if any.
		/// </summary>
		/// <returns></returns>
		public List<StaffMemberRetirementNotice> GetAllStaffsRetirementNotices()
		{
			List<StaffMemberRetirementNotice> lstStaffRetirementNotice = new List<StaffMemberRetirementNotice>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllUserRetirementDetails"))
				{
					while (oSqlDataReader.Read())
					{
						StaffMemberRetirementNotice oRetirementNotice = new StaffMemberRetirementNotice
						{
							UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
							Name = Convert.ToString(oSqlDataReader["Name"]),
							RetirementDate = Convert.ToDateTime(oSqlDataReader["RetirementDate"]),
							RemainingDays = Convert.ToInt32(oSqlDataReader["RemainingDays"]),
						};
						lstStaffRetirementNotice.Add(oRetirementNotice);
					}
				}
			}
			return lstStaffRetirementNotice;
		}

		/// <summary>
		/// This method is used to populate object of retirement notice config.
		/// </summary>
		/// <param name="oSqlDataReader"></param>
		/// <returns></returns>
		private RetirementNoticeConfiguration ReadObjectFromReader(SqlDataReader aoSqlDataReader)
		{
			RetirementNoticeConfiguration oRetirementNoticeConfig = new RetirementNoticeConfiguration
			{
				Id = Convert.ToInt32(aoSqlDataReader["Id"]),
				UserRole = new UserRoleMaster
				{
					Id = Convert.ToInt32(aoSqlDataReader["UserRoleId"]),
					Name = Convert.ToString(aoSqlDataReader["User_Role_Name"])
				},
				RetirementAge = Convert.ToInt32(aoSqlDataReader["RetirementAge"]),
				ReminderDays = Convert.ToInt32(aoSqlDataReader["ReminderDays"]),
			};

			return oRetirementNoticeConfig;
		}

		#endregion
	}
}
