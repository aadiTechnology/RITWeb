/* ---------------------------------------------------------------------
 *	FileName	: GroupMasterDC.cs
 *	Author		: Rohini V. Ghule
 *	Date		: 5-Oct-2011
 *	Description : This class is used to add, edit and remove the groups
 * ---------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AccountsEntities;
using Utility;

namespace DataCommunicator
{
	public class GroupMasterDC
	{

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// This method is used to get all group nature.
		/// </summary>
		/// <returns></returns>
		public static List<GroupNature> GetAllNatures()
		{
			var lstGroupNatures = new List<GroupNature>();
			String sSelectStmt = "SELECT Id, Name FROM Accounts.GroupNatureMaster WHERE IsDeleted = 0";
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
			{
				while (oSqlDataReader.Read())
					lstGroupNatures.Add(new GroupNature
					                    	{
					                    		Id	 = oSqlDataReader["Id"].ToInt(),
												Name = oSqlDataReader["Name"].ToString()
					                    	});
			}

			return lstGroupNatures;
		}

		/// <summary>
		/// This method is used to save group details.
		/// </summary>
		/// <param name="oGroupMaster"> </param>
		public static void Save(Group oGroupMaster)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("GroupId"		  , oGroupMaster.Id							, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("GroupName"	  , oGroupMaster.Name						, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("ParentGroupId"  , oGroupMaster.ParentGroup.Id				, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("IsPrimary"	  , oGroupMaster.IsPrimary					, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("GroupNatureId"  , oGroupMaster.GroupNature.Id				, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("ForTrailBalance", oGroupMaster.IsConsideredForTrialBalance, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsPANDetailsRequired", oGroupMaster.IsPANDetailsRequired, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("InsertedBy"	  , oGroupMaster.InsertedById				, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SchoolId"		  , oGroupMaster.SchoolId					, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("FinancialYearId", oGroupMaster.FinancialYearId			, SqlDbType.Int);
				
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Accounts].[usp_InsertGroupDetails]");
			}
		}

		/// <summary>
		/// This method is used to get all group details.
		/// </summary>
		/// <returns></returns>
		public static List<Group> GetAll()
		{
			var lstGroups = new List<Group>();
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAllGroupDetails"))
			{
				while (oSqlDataReader.Read())
					lstGroups.Add(new Group
					              	{
					              		Id							= oSqlDataReader["Id"].ToInt(),
										OriginalGroup				= new Group
										                				{
										                					Id = oSqlDataReader["OriginalId"].ToInt()
										                				},
										Name						= oSqlDataReader["Name"].ToString(),
										ParentGroup					= new Group
										              					{
										              						Id   = oSqlDataReader["ParentId"].ToInt(),
																			Name = oSqlDataReader["ParentName"].ToString()
										              					},
										IsPrimary					= oSqlDataReader["IsPrimary"].ToBool(),
										GroupNature					= new GroupNature
										              					{
										              						Id   = oSqlDataReader["GroupNatureId"].ToInt(),
																			Name = oSqlDataReader["GroupNatureName"].ToString()
										              					},
										IsConsideredForTrialBalance = oSqlDataReader["IsConsideredForTrialBalance"].ToBool(),
										IsSystemDefined				= oSqlDataReader["IsSystemDefined"].ToBool(),
										SchoolId					= oSqlDataReader["SchoolId"].ToInt(),
										FinancialYearId				= oSqlDataReader["FinancialYearId"].ToInt(),
                                        IsPANDetailsRequired = oSqlDataReader["IsPANRequired"].ToBool()
					              	});
			}

			return lstGroups;
		}

		/// <summary>
		/// This method is used to delete group.
		/// </summary>
		/// <param name="aiGroupId"></param>
		public static void Delete(int aiGroupId, int aiUpdatedById)
		{
            string sDeleteStmt = String.Format("UPDATE Accounts.GroupMaster SET IsDeleted = 1, UpdatedById={1}, UpdateDate = GETDATE() WHERE Id = {0}", aiGroupId, aiUpdatedById);
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sDeleteStmt);
		}

		/// <summary>
		/// This method is used to check dependency of groups in other tables.
		/// </summary>
		/// <param name="aiGroupId"></param>
		/// <returns></returns>
		public static bool CheckDepndencyForGroup(int aiGroupId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
				
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_CheckDependencyForGroup"))
					return oSqlDataReader.HasRows;
			}
		}

        /// <summary>
        /// This method is used to return all the group details with their debit credit values to show the trial balance report.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public static List<Group> GetAllGroupDetails(int aiSchoolId, int aiFinancialYearId, DateTime adtStartDate, DateTime adtEndDate, int aiGroupId = 0)
        {
            List<Group> lstGroup = new List<Group>();
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSqlDBUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSqlDBUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("[Accounts].[usp_GetGroupDetails]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstGroup.Add(new Group
                        {
                            Id = oSqlDataReader["GroupId"].ToInt(),
                            Name = oSqlDataReader["GroupName"].ToString(),
                            OriginalGroup = new Group { Id = (!oSqlDataReader["OriginalId"].IsNull() ? oSqlDataReader["OriginalId"].ToInt() : 0) },
                            ParentGroup = new Group { Id = oSqlDataReader["ParentId"].ToInt() },
                            IsPrimary = oSqlDataReader["IsPrimary"].ToBool(),
                            IsConsideredForTrialBalance = oSqlDataReader["IsConsideredForTrialBalance"].ToBool(),
                            IsSystemDefined = oSqlDataReader["IsSystemDefined"].ToBool(),
                            GroupNature = new GroupNature { Id = oSqlDataReader["GroupNatureId"].ToInt() },
                            Debit = oSqlDataReader["DebitAmount"].ToDecimal(),
                            Credit = oSqlDataReader["CreditAmount"].ToDecimal()
                        });
                    }

                    return lstGroup;
                }
            }
        }
		#endregion -- PUBLIC METHOD(s) --

	}
}