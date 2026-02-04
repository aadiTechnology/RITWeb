/* ------------------------------------------------------------------------------------------------
 *	Filename	: ApprovalConfigDC.cs
 *	Author		: Vishal B. Shah
 *	Date		: 8-Oct-2011
 *	Description	: This is the Data Access Layer for Approval Configuraiton in the Accounts module.
 * ------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AccountsEntities;
using MasterEntities;
using Utility;

namespace DataCommunicator
{
	public class ApprovalConfigDC
	{

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// Gets all designations configured for the school.
		/// </summary>
		/// <returns>A List of DesignationMaster entity objects.</returns>
		public static List<DesignationMaster> GetAllDesignations()
		{
			string sSqlStatemet = "SELECT Teacher_Designation_Id [DesignationId], Teacher_Designation_Name [Designation] FROM Teacher_Designation_Master WHERE Is_Deleted = N'N' ORDER BY User_Role_Id, Teacher_Designation_Id";
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatemet))
				return new GenericClass<DesignationMaster>().GetFilledObjectList(oSqlDataReader);
		}

		/// <summary>
		/// Returns all the Approval configurations in the system.
		/// </summary>
		/// <returns>A List of ApprovalConfig entity objects.</returns>
		public static List<ApprovalConfig> GetAllApprovalConfigurations()
		{
			var lstApprovalConfigs = new List<ApprovalConfig>();

			using (var oSqlDBUtility = new SQLServerDbUtility())
			using (SqlDataReader oReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAllApprovalConfig"))
			{
				var lstApprovalConfigDetails = new List<ApprovalConfigDetail>();
				while (oReader.Read())
					lstApprovalConfigDetails.Add(new ApprovalConfigDetail
						                            {
						                             	Id					= oReader["Id"].ToInt(),
														ApprovalConfig		= new ApprovalConfig { Id = oReader["ApprovalConfigId"].ToInt() },
														ApproverDesignation = new DesignationMaster
															                    {
															                      	DesignationId = oReader["ApproverDesignationId"].ToInt(),
																					Designation	  = oReader["ApproverDesignation"].ToString()
															                    },
														IsFinalApprover		= oReader["IsFinalApprover"].ToBool(),
														ApprovalOrder		= oReader["ApprovalOrder"].ToInt()
						                            });
			
				
				if (oReader.NextResult())
				{
					while (oReader.Read())
						lstApprovalConfigs.Add(new ApprovalConfig
					                       		{
					                       			Id					  = oReader["Id"].ToInt(),
													VoucherType			  = new VoucherType
												              				{
												              					Id	 = oReader["VoucherTypeId"].ToInt(),
																				Name = oReader["VoucherTypeName"].ToString()
												              				},
													CreatorDesignation	  = new DesignationMaster
												                     			{
												                     				DesignationId = oReader["CreatorDesignationId"].ToInt(),
																					Designation   = oReader["CreatorDesignation"].ToString()
												                     			},
													SchoolId			  = oReader["SchoolId"].ToInt(),
													FinancialYearId		  = oReader["FinancialYearId"].ToInt(),
													ApprovalConfigDetails = lstApprovalConfigDetails.Where(cfg => cfg.ApprovalConfig.Id == oReader["Id"].ToInt()).ToList()
					                       		});
				}		
			}

			return lstApprovalConfigs;
		}

		/// <summary>
		/// Saves the approval configuration.
		/// </summary>
		/// <param name="aoApprovalConfig"> </param>
		/// <returns>The number of rows affected by the action.</returns>
		public static int Save(ApprovalConfig aoApprovalConfig)
		{
			using (var oSqlDBUtility = new SQLServerDbUtility())
			{
				oSqlDBUtility.AddParameter("SchoolId"			 , aoApprovalConfig.SchoolId						, SqlDbType.Int);
				oSqlDBUtility.AddParameter("FinancialYearId"	 , aoApprovalConfig.FinancialYearId					, SqlDbType.Int);
				oSqlDBUtility.AddParameter("CreatorDesignationId", aoApprovalConfig.CreatorDesignation.DesignationId, SqlDbType.Int);
				oSqlDBUtility.AddParameter("VoucherTypeId"		 , aoApprovalConfig.VoucherType.Id					, SqlDbType.Int);
				oSqlDBUtility.AddParameter("ConfigXML"			 , CommonUtility.GetXMLForList(aoApprovalConfig.ApprovalConfigDetails), SqlDbType.Xml);
				oSqlDBUtility.AddParameter("UserId"				 , aoApprovalConfig.InsertedById					, SqlDbType.Int);
				// Output Parameter
				SqlParameter oSqlParam = oSqlDBUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);

				oSqlDBUtility.ExecuteStoredProcedureOnServer("Accounts.usp_InsertApprovalConfigDetails");

				return Convert.ToInt32(oSqlParam.Value);
			}
		}

		/// <summary>
		/// Deletes the given approval configuration from the db.
		/// </summary>
		/// <param name="aoApprovalConfig"> </param>
		/// <returns>The number of rows affected by the action.</returns>
		public static int DeleteApprovalConfig(ApprovalConfig aoApprovalConfig)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.ApprovalConfiguration SET IsDeleted = 1, UpdatedById = {3}, UpdateDate = GETDATE() WHERE SchoolId = {0} AND FinancialYearId = {1} AND Id = {2} AND IsDeleted = 0;" +
                                                 "UPDATE Accounts.ApprovalConfigurationDetails SET IsDeleted = 1, UpdatedById = {3}, UpdateDate = GETDATE() WHERE ApprovalConfigId = {2} AND IsDeleted = 0;",
												 aoApprovalConfig.SchoolId,
												 aoApprovalConfig.FinancialYearId,
												 aoApprovalConfig.Id,
												 aoApprovalConfig.UpdatedById);
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteTransaction(sSqlStatement);
		}

		/// <summary>
		/// Determines if there are any dependencies for the approval configuration.
		/// </summary>
		/// <param name="aoApprovalConfig"> </param>
		/// <returns>True if there are dependencies, false otherwise.</returns>
		public static bool CheckDependencyForApprovalConfig(ApprovalConfig aoApprovalConfig)
		{
			string sSqlStatement = String.Format("SELECT TOP 1 1 FROM [dbo].[udf_GetAllUsersDetails]({0}, {1}) a INNER JOIN Accounts.VoucherDetails b ON a.UserId = b.InsertedById" +
												 " INNER JOIN Accounts.ApprovalConfiguration c ON b.VoucherTypeId = c.VoucherTypeId AND a.DesignationId = c.CreatorDesignationId" +
												 " WHERE b.SchoolId = {0} AND b.FinancialYearId = {2} AND b.IsDeleted = 0 AND b.StatusId = {3} AND c.VoucherTypeId = {4} AND c.CreatorDesignationId = {5}",
												 aoApprovalConfig.SchoolId,
												 aoApprovalConfig.AcademicYearId,
												 aoApprovalConfig.FinancialYearId,
												 Constants.RequisitionStatus.Pending.ToInt(),
												 aoApprovalConfig.VoucherType.Id,
												 aoApprovalConfig.CreatorDesignation.DesignationId);
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSqlStatement) == 1;
		}

		#endregion -- PUBLIC METHOD(s) --

	}
}