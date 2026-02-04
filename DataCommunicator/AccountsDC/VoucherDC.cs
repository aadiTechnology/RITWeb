/* ----------------------------------------------------------------------------------
 *	Filename	: VoucherDC.cs
 *	Author		: Vishal B. Shah
 *	Date		: 8-Oct-2011
 *	Description	: This is the Data Access Layer for Vouchers in the Accounts module.
 * ----------------------------------------------------------------------------------
 */

/* ----------------------------------------------------------------------------------
 *  MODIFICATION LOG
 * ----------------------------------------------------------------------------------
 *	Author		: Vishal B. Shah
 *	Date		: 27-Jan-2012
 *	Purpose		: Added methods required for Caution Money payments.
 * ----------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using AccountsEntities;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
	public class VoucherDC
	{

		#region -- PUBLIC METHOD(s) --

		#region -- BASIC METHOD(s) --

		/// <summary>
		/// Returns all the Types of Vouchers configured for the school.
		/// </summary>
		/// <returns></returns>
		public static List<VoucherType> GetVoucherTypes()
		{
			List<VoucherType> lstVoucherTypes;
			string sSqlStatement = "SELECT Id, Name, RequiresApproval FROM Accounts.VoucherTypeMaster WHERE IsDeleted = 0";
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
			{
				var oGenricClass = new GenericClass<VoucherType>();
				lstVoucherTypes = oGenricClass.GetFilledObjectList(oReader);
			}
			
			return lstVoucherTypes;
		}

		/// <summary>
		/// Gets applicable Status for the Accounts module.
		/// </summary>
		/// <returns></returns>
		public static List<VoucherStatus> GetVoucherStatus()
		{
			var lstVoucherStatus = new List<VoucherStatus>();
			string sSqlStatement = "SELECT StatusId, StatusName FROM StatusMaster ORDER BY Sort_Order";

			using (var oSqlDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
			{
				while (oReader.Read())
					lstVoucherStatus.Add(new VoucherStatus
											{
												Id   = oReader["StatusId"].ToInt(),
												Name = oReader["StatusName"].ToString()
											});
			}

			return lstVoucherStatus;
		}

		#endregion -- BASIC METHOD(s) --

		#region -- GENERAL METHOD(s) --

		/// <summary>
		/// Determines if the given Ledger is referenced in any Voucher.
		/// </summary>
		/// <param name="aiLedgerId"></param>
		/// <returns></returns>
		public static bool CheckVoucherDependency(int aiLedgerId)
		{
			string sSqlStatement = String.Format("SELECT TOP 1 1 FROM Accounts.VoucherParticularsDetails WHERE IsDeleted = 0 AND LedgerId = {0}", aiLedgerId);

			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSqlStatement) == 1;
		}

		/// <summary>
		/// Gets all vouchers for the given filters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiUserId"></param>
		/// <param name="aiStatusId"></param>
		/// <param name="asSortExpression"></param>
		/// <param name="aiStartIndex"></param>
		/// <param name="aiEndIndex"></param>
		/// <returns></returns>
		public static List<Voucher> GetAllVouchers(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId, int aiStatusId, string asSortExpression, int aiStartIndex, int aiEndIndex)
		{
			List<Voucher> lstVouchers;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("StatusId"		, aiStatusId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("SortExp"		, asSortExpression , SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("StartIndex"		, aiStartIndex	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("EndIndex"		, aiEndIndex	   , SqlDbType.Int);

				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAllVouchers"))
				{
					lstVouchers = new List<Voucher>();
					while (oReader.Read())
					{
						lstVouchers.Add(new Voucher
						                {
						                    VoucherId			= oReader["VoucherId"].ToInt(),
						                    SerialNumber		= oReader["SerialNumber"].ToString(),
						                    Date				= oReader["Date"].ToString().ToDateTime(),
						                    CreatedBy			= oReader["CreatedBy"].ToString(),
						                    VoucherType			= new VoucherType { Name = oReader["VoucherType"].ToString() },
						                    Amount				= oReader["Amount"].ToDecimal(),
											IsSubmitted			= oReader["IsSubmitted"].ToBool(),
						                    NextApproverDesigId = oReader["NextApproverDesigId"].ToInt(),
						                    NextApprover		= oReader["NextApprover"].ToString(),
											Status				= (Constants.RequisitionStatus)aiStatusId
						                });
					}
				}
			}
			return lstVouchers;
		}

		/// <summary>
		/// Returns the count of vouchers for the given filters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiUserId"></param>
		/// <param name="aiStatusId"></param>
		/// <returns></returns>
		public static int GetCount(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId, int aiStatusId)
		{
			int iCount = 0;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("StatusId"		, aiStatusId	   , SqlDbType.Int);
				
				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAllVouchersCount"))
					if (oReader.Read())
						iCount = oReader["Count"].ToInt();
			}
			return iCount;
		}

		/// <summary>
		/// Gets all vouchers for the given filters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="asEndDate"> </param>
		/// <param name="asSortExpression"></param>
		/// <param name="aiStartIndex"></param>
		/// <param name="aiEndIndex"></param>
		/// <param name="aiLedgerId"> </param>
		/// <param name="asStartDate"> </param>
		/// <returns></returns>
		public static List<VoucherParticular> GetAllVouchers(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiLedgerId, string asStartDate, string asEndDate, string asSortExpression, int aiStartIndex, int aiEndIndex)
		{
			var lstVoucherParticular = new List<VoucherParticular>();

			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("LedgerId"		, aiLedgerId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("StartDate"		, asStartDate	   , SqlDbType.DateTime);
				oSqlDbUtility.AddParameter("EndDate"		, asEndDate		   , SqlDbType.DateTime);
				oSqlDbUtility.AddParameter("SortExp"		, asSortExpression , SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("StartIndex"		, aiStartIndex	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("EndIndex"		, aiEndIndex	   , SqlDbType.Int);

				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAllVouchersForLedger"))
				{
					while (oReader.Read())
					{
						lstVoucherParticular.Add(new VoucherParticular
													{
														Ledger  = new Ledger
																	{
																		Id	 = oReader["LedgerId"].ToInt(),
																		Name = oReader["Particular"].ToString()
																	},
														IsDebit = oReader["IsDebit"].ToBool(),
														Amount  = oReader["Amount"].ToDecimal(),
														Voucher = new Voucher
														{
															VoucherId	 = oReader["VoucherId"].ToInt(),
															Date		 = oReader["Date"].ToString().ToDateTime(),
															SerialNumber = oReader["SerialNumber"].ToString(),
															VoucherType  = new VoucherType { Name = oReader["VoucherType"].ToString() },
															Status		 = Constants.RequisitionStatus.Approved
														}
													});
					}
				}

			}

			return lstVoucherParticular;
		}

		/// <summary>
		/// Returns the count of vouchers for the given filters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiLedgerId"> </param>
		/// <param name="asStartDate"> </param>
		/// <param name="asEndDate"> </param>
		/// <returns></returns>
		public static int GetCount(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiLedgerId,string asStartDate,string asEndDate)
		{
			int iCount = 0;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("LedgerId"		, aiLedgerId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("StartDate"		, asStartDate	   , SqlDbType.DateTime);
				oSqlDbUtility.AddParameter("EndDate"		, asEndDate		   , SqlDbType.DateTime);
				
				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAllVouchersCountForLedger"))
				{
					if (oReader.Read())
						iCount = oReader["Count"].ToInt();
				}
			}

			return iCount;
		}

		/// <summary>
		/// Saves the voucher to db.
		/// </summary>
		/// <param name="aoVoucher">The Voucher entity object to be saved.</param>
		/// <returns>An updated copy of the Voucher entity object.</returns>
		public static Voucher Save(Voucher aoVoucher)
		{
			Voucher oVoucher = null;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aoVoucher.SchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId" , aoVoucher.AcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aoVoucher.FinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			, aoVoucher.InsertedById   , SqlDbType.Int);
				if (aoVoucher.VoucherId != Constants.I_ZERO)
					oSqlDbUtility.AddParameter("VoucherId"	, aoVoucher.VoucherId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("VoucherTypeId"	, aoVoucher.VoucherType.Id , SqlDbType.Int);
				oSqlDbUtility.AddParameter("Date"			, aoVoucher.Date		   , SqlDbType.SmallDateTime);
				if (!String.IsNullOrEmpty(aoVoucher.Narration))
					oSqlDbUtility.AddParameter("Narration"	, aoVoucher.Narration	   , SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("TotalAmount"	, aoVoucher.Amount		   , SqlDbType.Decimal);
				oSqlDbUtility.AddParameter("IsSubmitted"	, aoVoucher.IsSubmitted	   , SqlDbType.Bit);
				oSqlDbUtility.AddParameter("StatusId"		, aoVoucher.Status.ToInt() , SqlDbType.Int);
				oSqlDbUtility.AddParameter("DetailsXML"		, CommonUtility.GetXMLForList(aoVoucher.VoucherParticulars), SqlDbType.Xml);

				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_InsertUpdateVoucherDetails"))
					if (oReader.Read())
						oVoucher = new Voucher
									{
										VoucherId	  = oReader["Id"].ToInt(),
										SerialNumber  = oReader["SerialNumber"].ToString(),
										Status		  = (Constants.RequisitionStatus)oReader["StatusId"].ToInt(),
										ApprovalOrder = oReader["ApprovalOrder"].ToInt()
									};
			}

			return oVoucher;
		}

		/// <summary>
		/// Deletes a Voucher from the db.
		/// </summary>
		/// <param name="aoVoucher">The Voucher to be deleted</param>
		/// <returns>true if deleted successfully, false otherwise.</returns>
		public static bool Delete(Voucher aoVoucher)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.VoucherParticularsDetails SET IsDeleted = 1, UpdatedById = {3}, UpdateDate = GETDATE()" +
												 " WHERE VoucherId = {2} AND IsDeleted = 0;" +
												 "UPDATE Accounts.VoucherDetails SET IsDeleted = 1, UpdatedById = {3}, UpdateDate = GETDATE()" +
												 " WHERE SchoolId = {0} AND FinancialYearId = {1} AND Id = {2} AND IsDeleted = 0",
												 aoVoucher.SchoolId,
												 aoVoucher.FinancialYearId,
												 aoVoucher.VoucherId,
												 aoVoucher.UpdatedById);
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
				return oSqlDbUtility.ExecuteTransaction(sSqlStatement) > 0;
		}

		/// <summary>
		/// Submits a voucher for approval.
		/// </summary>
		/// <param name="aoVoucherDetails"></param>
		/// <returns>true if submitted successfully, false otherwise.</returns>
		public static bool SubmitVoucherForApproval(Voucher aoVoucherDetails)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.VoucherDetails SET IsSubmitted = 1, UpdatedById = {3}, UpdateDate = GETDATE() WHERE SchoolId = {0} AND FinancialYearId = {1} AND IsDeleted = 0 AND IsSubmitted = 0 AND Id = {2}",
												 aoVoucherDetails.SchoolId,
												 aoVoucherDetails.FinancialYearId,
												 aoVoucherDetails.VoucherId,
												 aoVoucherDetails.UpdatedById);
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
				return oSqlDbUtility.ExecuteTransaction(sSqlStatement) > 0;
		}

		/// <summary>
		/// Gets voucher details.
		/// </summary>
		/// <param name="miSchoolId"></param>
		/// <param name="miFinancialYearId"></param>
		/// <param name="miVoucherId"></param>
		/// <param name="miUserId"></param>
		/// <returns>A Voucher entity object.</returns>
		public static Voucher GetVoucherDetails(int miSchoolId, int miFinancialYearId, int miVoucherId, int miUserId)
		{
			Voucher oVoucher = null;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, miSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", miFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("VoucherId"		, miVoucherId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			, miUserId		   , SqlDbType.Int);
				
				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetVoucherDetails"))
				{
					if (oReader.Read())
					{
						oVoucher = new Voucher
											{
												VoucherType		   = new VoucherType
																		{
																			Id   = oReader["VoucherTypeId"].ToInt(),
																			Name = oReader["VoucherTypeName"].ToString()
																		},
												SerialNumber	   = oReader["SerialNumber"].ToString(),
												Date			   = oReader["VoucherDate"].ToDateTime(),
												Narration		   = oReader["Narration"].ToString(),
												Status			   = (Constants.RequisitionStatus)oReader["StatusId"].ToInt(),
												IsSubmitted		   = oReader["IsSubmitted"].ToBool(),
												Amount			   = oReader["TotalAmount"].ToDecimal(),
												InsertedById	   = oReader["InsertedById"].ToInt(),
												CreatedBy		   = oReader["CreatedBy"].ToString(),
												CurrentUserDesigId = oReader["CurrentUserDesigId"].ToInt(),
												IsFinalApprover	   = oReader["IsFinalApprover"].ToBool(),
												IsFeeVoucher	   = oReader["IsFeeVoucher"].ToBool()
											};

						if (oReader.NextResult())
						{
							oVoucher.VoucherParticulars = new List<VoucherParticular>();
							VoucherParticular oVoucherParticular;
							while (oReader.Read())
							{
								oVoucherParticular = new VoucherParticular
								                     	{
								                     		Id		  = oReader["VoucherParticularsId"].ToInt(),
															IsDebit   = oReader["IsDebit"].ToBool(),
															Amount	  = oReader["Amount"].ToDecimal(),
															IsDeleted = oReader["IsDeleted"].ToBool()
								                     	};
								if (oReader["BankId"].ToInt() != 0)
									oVoucherParticular.Ledger = new BankAccount
									                            	{
																		Id	  = oReader["LedgerId"].ToInt(),
																		Name  = oReader["LedgerName"].ToString(),
																		Group = new Group { Id = oReader["GroupId"].ToInt() },
																		Bank  = new Bank { Id = oReader["BankId"].ToInt() }
									                            	};
								else
									oVoucherParticular.Ledger = new Ledger
									                            	{
																		Id	  = oReader["LedgerId"].ToInt(),
																		Name  = oReader["LedgerName"].ToString(),
																		Group = new Group { Id = oReader["GroupId"].ToInt() }
									                            	};
							
								oVoucher.VoucherParticulars.Add(oVoucherParticular);
							}
						}
					}
				}
			}

			return oVoucher;
		}
 
		/// <summary>
		/// Gets action details for the given Voucher.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiVoucherId"></param>
		/// <returns></returns>
		public static List<VoucherAction> GetVoucherActionDetails(int aiSchoolId, int aiFinancialYearId, int aiVoucherId)
		{
			List<VoucherAction> lstVoucherActions = null;
			string sSqlStatement = String.Format("SELECT b.InsertDate, CASE WHEN d.User_Id IS NULL THEN N'System' ELSE [dbo].[Udf_GetUserName](b.InsertedById, d.User_Role_Id) END AS [UserName], b.Comment, c.StatusID" +
												 "	FROM Accounts.VoucherDetails a INNER JOIN Accounts.VoucherActionDetails b ON a.Id = b.VoucherId INNER JOIN StatusMaster c ON b.StatusId = c.StatusID LEFT OUTER JOIN User_Master d ON b.InsertedById = d.User_Id" +
												 " WHERE a.Id = {0} AND a.IsDeleted = 0 AND a.SchoolId = {1} AND a.FinancialYearId = {2} AND b.IsDeleted = 0",
												 aiVoucherId,
												 aiSchoolId,
												 aiFinancialYearId);
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oReader = oSqlDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
			{
				if (oReader.HasRows)
				{
					lstVoucherActions = new List<VoucherAction>();
					while (oReader.Read())
					{
						lstVoucherActions.Add(new VoucherAction
							{
								UserName   = oReader["UserName"].ToString(),
								Comment	   = oReader["Comment"].ToString(),
								Status	   = (Constants.RequisitionStatus)oReader["StatusID"].ToInt(),
								InsertDate = oReader["InsertDate"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
							});
					}
				}
			}

			return lstVoucherActions;
		}

		/// <summary>
		/// Performs the given VoucherAction on the Voucher.
		/// </summary>
		/// <param name="aoVoucherAction"></param>
		/// <returns>true if the Action is successful, false otherwise.</returns>
		public static bool PerformActionOnVoucher(VoucherAction aoVoucherAction)
		{
			bool bResult = false;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aoVoucherAction.SchoolId			, SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aoVoucherAction.AcademicYearId	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aoVoucherAction.FinancialYearId	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("VoucherId"		, aoVoucherAction.Voucher.VoucherId	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			, aoVoucherAction.InsertedById		, SqlDbType.Int);
				oSqlDbUtility.AddParameter("Comment"		, aoVoucherAction.Comment			, SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("StatusId"		, aoVoucherAction.Status.ToInt()	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinalApprove"	, aoVoucherAction.FinalApprove		, SqlDbType.Bit);
				
				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_PerformActionOnVoucher"))
					if (oReader.HasRows && oReader.Read())
					{
						bResult = oReader["Count"].ToInt() != 0;
						aoVoucherAction.Voucher.Status = (Constants.RequisitionStatus)oReader["StatusId"].ToInt();
					}
			}

			return bResult;
		}

		/// <summary>
		/// Returns the count of Vouchers waiting for approval for a particular user.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiUserId">UserId of the user to check against.</param>
		/// <returns></returns>
		public static int GetVoucherCountForApproval(int aiSchoolId, int aiFinancialYearId, int aiUserId)
		{
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);

				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetVoucherCountForApproval"))
					if (oReader.HasRows && oReader.Read())
						return oReader["Count"].ToInt();
			}

			return 0;
		}
		
		#endregion -- GENERAL METHOD(s) --

		#region -- DAY BOOK METHOD(s) --

		/// <summary>
		/// Gets vouchers to be displayed on the DayBook screen for the given filters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="asDateTimeFilter"></param>
		/// <param name="abIncludePending"></param>
		/// <param name="asSortExpression"></param>
		/// <param name="aiStartIndex"></param>
		/// <param name="aiEndIndex"></param>
		/// <returns></returns>
		public static List<Voucher> GetAllVouchersForDayBook(int aiSchoolId, int aiFinancialYearId, string asDateTimeFilter, bool abIncludePending, string asSortExpression, int aiStartIndex, int aiEndIndex)
		{
			List<Voucher> lstVoucher = null;
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("DateTimeFilter"	, asDateTimeFilter , SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("IncludePending"	, abIncludePending , SqlDbType.Bit);
				oSqlDbUtility.AddParameter("SortExp"		, asSortExpression , SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("StartIndex"		, aiStartIndex	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("EndIndex"		, aiEndIndex	   , SqlDbType.Int);
				
				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetPagedVouchersForDayBook"))
				{
					if (oReader.HasRows)
					{
						lstVoucher = new List<Voucher>();
						while (oReader.Read())
							lstVoucher.Add(new Voucher
											{
												VoucherId	 = oReader["VoucherId"].ToInt(),
												SerialNumber = oReader["SerialNumber"].ToString(),
												VoucherType  = new VoucherType { Name = oReader["VoucherType"].ToString() },
												CreatedBy	 = oReader["CreatedBy"].ToString(),
												Narration	 = oReader["Narration"].ToString(),
												Amount		 = oReader["TotalAmount"].ToDecimal(),
												Date		 = oReader["VoucherDate"].ToDateTime(),
												IsFeeVoucher = oReader["IsFeeVoucher"].ToBool(),
												Status		 = (Constants.RequisitionStatus)oReader["StatusId"].ToInt()
											});
					}
				}
			}

			return lstVoucher;
		}

        /// <summary>
		/// Returns the count of vouchers fetched for the given filters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="asDateTimeFilter"></param>
		/// <param name="abIncludePending"></param>
		/// <returns></returns>
		public static int GetAllVouchersForDayBookCount(int aiSchoolId, int aiFinancialYearId, string asDateTimeFilter, bool abIncludePending)
		{
			string sSqlStatement = String.Format("SELECT COUNT(a.Id) FROM Accounts.VoucherDetails a INNER JOIN Accounts.VoucherTypeMaster b ON a.VoucherTypeId = b.Id" +
												 " WHERE a.SchoolId = {0} AND a.FinancialYearId = {1} AND a.IsDeleted = 0 AND (a.StatusId = {2}{3}) AND CAST(a.Date AS DATE) {4}",
												 aiSchoolId,
												 aiFinancialYearId,
												 Constants.RequisitionStatus.Approved.ToInt(),
												 abIncludePending ? String.Format(" OR (a.StatusId = {0} AND a.IsSubmitted = 1)", Constants.RequisitionStatus.Pending.ToInt()) : String.Empty,
												 asDateTimeFilter);

			using (var oSqlDbUtility = new SQLServerDbUtility())
				return oSqlDbUtility.PerformIntQueryOnSqlServer(sSqlStatement);
		}

		#endregion -- DAY BOOK METHOD(s) --

        #region -- IMPORT VOUCHERS METHOD(s) --
        
        /// <summary>
        /// This method is used to get voucher details to export.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="asDateTimeFilter"></param>
        /// <param name="abIncludePending"></param>
        /// <param name="aiVoucherId"></param>
        /// <returns></returns>
        public static List<Voucher> GetVouchersToExport(int aiSchoolId, int aiFinancialYearId, string asDateTimeFilter, bool abIncludePending, int aiVoucherId)
        {
            List<Voucher> lstVoucher = null;
            List<VoucherParticular> lstVoucherParticular = null;

            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("DateTimeFilter", asDateTimeFilter, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("IncludePending", abIncludePending, SqlDbType.Bit);
                oSqlDbUtility.AddParameter("VoucherId", aiVoucherId, SqlDbType.Int);

                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetVouchersToExport"))
                {
                    if (oReader.HasRows)
                    {
                        lstVoucher = new List<Voucher>();
                        lstVoucherParticular = new List<VoucherParticular>();
                        while (oReader.Read())
                            lstVoucherParticular.Add(new VoucherParticular()
                            {
                                Amount = oReader["Amount"].ToDecimal(),
                                Ledger = new Ledger { Name = oReader["Name"].ToString() },
                                IsDebit = oReader["IsDebit"].ToBool(),
                                Voucher = new Voucher
                                {
                                    VoucherId = oReader["VoucherId"].ToInt(),
                                    Status = Constants.RequisitionStatus.My_Requisition
                                },
                            });

                        if (oReader.NextResult())
                            while (oReader.Read())
                                lstVoucher.Add(new Voucher
                                {
                                    VoucherId = oReader["VoucherId"].ToInt(),
                                    SerialNumber = oReader["SerialNumber"].ToString(),
                                    VoucherType = new VoucherType { Name = oReader["VoucherType"].ToString() },
                                    Narration = oReader["Narration"].ToString(),
                                    Date = oReader["VoucherDate"].ToDateTime(),
                                    VoucherParticulars = lstVoucherParticular.Where(oVoucherParticular => oVoucherParticular.Voucher.VoucherId == oReader["VoucherId"].ToInt()).ToList<VoucherParticular>(),
                                    Status = Constants.RequisitionStatus.My_Requisition,
                                });
                    }
                }
            }

            return lstVoucher;
        }

        #endregion -- IMPORT VOUCHERS METHOD(s) --


        #region -- FEE VOUCHER METHOD(s) --

        /// <summary>
		/// This function is used to create a Fee Voucher for payments made in cash.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asReceiptNo"></param>
		/// <param name="aiUserId"></param>
		public static void CreateFeeVoucherForCashPayment(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiStudentId, string asReceiptNo, int aiUserId)
		{
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("StudentId"		, aiStudentId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("ReceiptNo"		, asReceiptNo	   , SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);

				oSqlDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_CreateFeeVoucherForCashPayment");
			}
		}

        /// <summary>
        /// This function is used to create admission form fee voucher on transaction clearance.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asClearanceInfoXML"></param>
        public static void CreateAdmissionFormVoucher(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId, string asClearanceInfoXML)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);                                
                oSqlDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("ClearanceInfoXML", asClearanceInfoXML, SqlDbType.Xml);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_CreateAdmissionFormVoucher");
            }
        }

		/// <summary>
		/// This function is used to Create/Update a Fee Voucher.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiUserId"></param>
		/// <param name="asClearanceInfoXML"></param>
		/// <param name="ePaymentMode"></param>
		public static void CreateFeeVoucher(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId, string asClearanceInfoXML, Constants.PaymentMode ePaymentMode)
		{
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		 , aiSchoolId			, SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	 , aiAcademicYearId		, SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId" , aiFinancialYearId	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("UserId"			 , aiUserId				, SqlDbType.Int);
				oSqlDbUtility.AddParameter("ClearanceInfoXML", asClearanceInfoXML	, SqlDbType.Xml);
				oSqlDbUtility.AddParameter("PaymentMode"	 , ePaymentMode.ToInt() , SqlDbType.Int);

				oSqlDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_CreateFeeVoucher");
			}
		}

		/// <summary>
		/// Gets student and payment details for the given Fee Voucher and Fee head.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiVoucherId"></param>
		/// <param name="aiLedgerId"></param>
		/// <returns></returns>
        public static List<FeeVoucherDetails> GetFeeVoucherDetails(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiVoucherId, int aiLedgerId, ref List<FeeReceiptDetails> lstFeeReceiptDetails)
		{
			var lstFeeVoucherDetails = new List<FeeVoucherDetails>();
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("VoucherId"		, aiVoucherId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("LedgerId"		, aiLedgerId	   , SqlDbType.Int);

                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetFeeVoucherDetails"))
                {
                    if (oReader.HasRows)
                        while (oReader.Read())
                            lstFeeVoucherDetails.Add(new FeeVoucherDetails
                                                        {
                                                            StudentName = oReader["StudentName"].ToString(),
                                                            RegNo = oReader["RegNo"].ToString(),
                                                            Class = oReader["Class"].ToString(),
                                                            PaymentMode = (Constants.PaymentMode)oReader["PaymentMode"].ToInt(),
                                                            PaymentDetails = oReader["PaymentDetails"].ToString(),
                                                            Amount = oReader["Amount"].ToDecimal(),
                                                            PayableFor = oReader["PayableFor"].ToString(),
                                                            DepositLedger = new Ledger
                                                                                {
                                                                                    Id = oReader["DepositedLedgerId"].ToInt(),
                                                                                    Name = oReader["DepositedLedgerName"].ToString()
                                                                                },
                                                            AcademicYear = oReader["AcademicYear"].ToString(),
                                                            ReceiptNumber = oReader["ReceiptNumber"].ToString()                 
                                                        });

                    lstFeeReceiptDetails = FillReceiptDetails(oReader);
                }   
			}

			return lstFeeVoucherDetails;
		}

        /// <summary>
        /// This method is used to fill the receipt details.
        /// </summary>
        /// <param name="aoReader"></param>
        /// <returns></returns>
        public static List<FeeReceiptDetails> FillReceiptDetails(SqlDataReader aoReader)
        {
            List<FeeReceiptDetails> lstFeeReceiptDetails = new List<FeeReceiptDetails>();
            if (aoReader.NextResult())
            {
                while (aoReader.Read())
                {
                    lstFeeReceiptDetails.Add(new FeeReceiptDetails
                    {
                        FeeType = aoReader["FeeType"].ToString(),
                        Amount = aoReader["Amount"].ToInt(),
                        ReceiptNumber = aoReader["ReceiptNumber"].ToString()
                    });
                }
            }

            return lstFeeReceiptDetails;
        }

		/// <summary>
		/// Deletes particulars from a Fee Voucher for the given Student & Receipt number.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="asReceiptNo"> </param>
		/// <param name="asStudentFeeIdsXML"></param>
		/// <param name="aiUserId"></param>
		/// <param name="aiStudentId"> </param>
		public static void DeleteFeeVoucher(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiStudentId, string asReceiptNo, string asStudentFeeIdsXML, int aiUserId)
		{
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId		, SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId	, SqlDbType.Int);
				oSqlDbUtility.AddParameter("StudentId"		, aiStudentId		, SqlDbType.Int);
				oSqlDbUtility.AddParameter("ReceiptNo"		, asReceiptNo		, SqlDbType.NVarChar);
				oSqlDbUtility.AddParameter("StudenFeeIdsXML", asStudentFeeIdsXML, SqlDbType.Xml);
				oSqlDbUtility.AddParameter("UserId"			, aiUserId			, SqlDbType.Int);

				oSqlDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_DeleteFeeVoucher");
			}
		}

		/// <summary>
		/// Returns fee particulars for the given StudentId and ReceiptNo.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="asReceiptNo"></param>
		/// <returns></returns>
		public static List<FeeVoucherParticulars> GetFeePaymentParticulars(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiStudentId, string asReceiptNo)
		{
			var lstFeeParticulars = new List<FeeVoucherParticulars>();
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
			{
				oSqlDbUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDbUtility.AddParameter("StudentId"		, aiStudentId	   , SqlDbType.Int);
				oSqlDbUtility.AddParameter("ReceiptNo"		, asReceiptNo	   , SqlDbType.NVarChar);

				using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetFeePaymentParticulars"))
				{
					if (oReader.HasRows)
					{
						var oGenricClass = new GenericClass<FeeVoucherParticulars>();
						lstFeeParticulars = oGenricClass.GetFilledObjectList(oReader);
					}
				}
			}

			return lstFeeParticulars;
		}

		#endregion -- FEE VOUCHER METHOD(s) --

		#region -- CAUTION MONEY METHOD(s) --

		/// <summary>
		/// Records caution money payment details in the accounts module.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="aiUserId"></param>
		public static void RecordCautionMoneyPayment(int aiSchoolId, int aiFinancialYearId, int aiStudentId, int aiUserId)
		{
			using (SQLServerDbUtility oSqlServerDbUtility = GetSqlDbUtilityObj(aiSchoolId, aiFinancialYearId, aiStudentId, aiUserId))
				oSqlServerDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_RecordCautionMoneyPayment");
		}

		/// <summary>
		/// Records caution money return payment details in the accounts module.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="aiUserId"></param>
		public static void RecordCautionMoneyReturnPayment(int aiSchoolId, int aiFinancialYearId, int aiStudentId, int aiUserId)
		{
			using (SQLServerDbUtility oSqlServerDbUtility = GetSqlDbUtilityObj(aiSchoolId, aiFinancialYearId, aiStudentId, aiUserId))
				oSqlServerDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_RecordCautionMoneyReturnPayment");
		}

		/// <summary>
		/// Deletes caution money payment details from the accounts module.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="aiAmount"></param>
		/// <param name="aiUserId"></param>
		public static void DeleteCautionMoneyPayment(int aiSchoolId, int aiFinancialYearId, int aiStudentId, int aiAmount, int aiUserId)
		{
			using (SQLServerDbUtility oSqlServerDbUtility = GetSqlDbUtilityObj(aiSchoolId, aiFinancialYearId, aiStudentId, aiUserId))
			{
				oSqlServerDbUtility.AddParameter("Amount", aiAmount, SqlDbType.Int);
				oSqlServerDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_DeleteCautionMoneyPayment");
			}
		}

		/// <summary>
		/// Deletes caution money return payment details from the accounts module.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="aiUserId"></param>
		public static void DeleteCautionMoneyReturnPayment(int aiSchoolId, int aiFinancialYearId, int aiStudentId, int aiUserId)
		{
			using (SQLServerDbUtility oSqlServerDbUtility = GetSqlDbUtilityObj(aiSchoolId, aiFinancialYearId, aiStudentId, aiUserId))
				oSqlServerDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_DeleteCautionMoneyReturnPayment");
		}

		#endregion -- CAUTION MONEY METHOD(s) --

		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		/// Returns an SQLServerDbUtility object with the arguments added as parameters.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiStudentId"></param>
		/// <param name="aiUserId"></param>
		/// <returns>An instance of SQLServerDbUtility class.</returns>
		private static SQLServerDbUtility GetSqlDbUtilityObj(int aiSchoolId, int aiFinancialYearId, int aiStudentId, int aiUserId)
		{
			var oSqlServerDbUtility = new SQLServerDbUtility();
			oSqlServerDbUtility.AddParameter("SchoolId"		  , aiSchoolId		 , SqlDbType.Int);
			oSqlServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
			oSqlServerDbUtility.AddParameter("StudentId"	  , aiStudentId		 , SqlDbType.Int);
			oSqlServerDbUtility.AddParameter("UserId"		  , aiUserId		 , SqlDbType.Int);
			return oSqlServerDbUtility;
		}

		#endregion -- PRIVATE METHOD(s) --

	}
}