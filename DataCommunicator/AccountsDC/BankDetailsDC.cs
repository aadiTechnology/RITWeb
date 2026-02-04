using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AccountsEntities;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
	public class BankDetailsDC
	{

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// This method is used to save bank details.
		/// </summary>
		public static void Save(BankAccount aoBankAccountDetails)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId"				  , aoBankAccountDetails.SchoolId				, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("FinancialYearId"		  , aoBankAccountDetails.FinancialYearId		, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("LedgerId"				  , aoBankAccountDetails.Id						, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("BankId"				  , aoBankAccountDetails.Bank.Id				, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("BankName"				  , aoBankAccountDetails.Name					, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("Alias"				  , aoBankAccountDetails.Alias					, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("BankAcNo"				  , aoBankAccountDetails.AccountNumber			, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("OpeningBal"			  , aoBankAccountDetails.OpeningBalance			, SqlDbType.Decimal);
				oSQLServerDbUtility.AddParameter("IsDebit"				  , aoBankAccountDetails.IsDebit				, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("BankAddress"			  , aoBankAccountDetails.Address				, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("IsForOnlineTransactions", aoBankAccountDetails.IsForOnlineTransactions, SqlDbType.Bit);
				oSQLServerDbUtility.AddParameter("InsertedBy"			  , aoBankAccountDetails.InsertedById			, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsDefault", aoBankAccountDetails.IsDefault, SqlDbType.Bit);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Accounts.usp_InsertBankDetails");
			}
		}

		/// <summary>
		/// This method is used to get bank details to edit.
		/// </summary>
		/// <returns>A List of BankAccountDetails entity objects.</returns>
		public static List<BankAccount> GetBankDetails()
		{
			var lstBanks = new List<BankAccount>();

			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetBankDetalisForEdit"))
			{
				while (oSqlDataReader.Read())
					lstBanks.Add(new BankAccount
									{
										Id				= oSqlDataReader["LedgerId"].ToInt(),
										Name			= oSqlDataReader["LedgerName"].ToString(),
										OriginalLedger	= new Ledger { Id = oSqlDataReader["OriginalId"].ToInt() },
										IsDebit			= oSqlDataReader["IsDebit"].ToBool(),
										OpeningBalance	= oSqlDataReader["OpeningBalance"].ToDecimal(),
										Bank			= new Bank
										       				{
																Id	 = oSqlDataReader["BankId"].ToInt(),
																Name = oSqlDataReader["BankName"].ToString()
										       				},	
										Alias			= oSqlDataReader["Alias"].ToString(),
										AccountNumber	= oSqlDataReader["BankAccountNumber"].ToString(),
										Address			= oSqlDataReader["BankAddress"].ToString(),
										IsForOnlineTransactions = oSqlDataReader["IsForOnlineTransactions"].ToBool(),
										SchoolId		= oSqlDataReader["SchoolId"].ToInt(),
										FinancialYearId = oSqlDataReader["FinancialYearId"].ToInt(),
                                        IsDefault = oSqlDataReader["IsDefault"].ToBool()
									});
			}

			return lstBanks;
		}

		/// <summary>
		/// Gets all the Banks configured for the school.
		/// </summary>
		/// <returns>A List of Bank entity objects.</returns>
		public static List<Bank> GetAllBanks()
		{
			var lstBanks = new List<Bank>();
			string sSelectStatement = "SELECT Schoolwise_Bank_Id [BankId], Bank_Name [BankName], School_Id [SchoolId] FROM Schoolwise_Bank_Master WHERE Is_Deleted = N'N' ORDER BY Bank_Name";
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
			{
				while (oSqlDataReader.Read())
					lstBanks.Add(new Bank
					             	{
					             		Id		 = oSqlDataReader["BankId"].ToInt(),
										Name	 = oSqlDataReader["BankName"].ToString(),
										SchoolId = oSqlDataReader["SchoolId"].ToInt()
					             	});
			}

			return lstBanks;
		}

		/// <summary>
		/// This method is used to delete Bank details.
		/// </summary>
		public static void Delete(int aiSchoolId, int aiFinancialYearId, int aiLedgerId, int aiUserId)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.BankAccountDetails SET IsDeleted = 1, UpdatedById = {0}, UpdateDate = GETDATE() WHERE LedgerId = {1} AND SchoolId = {2} AND FinancialYearId = {3};" +
												 "UPDATE Accounts.LedgerMaster SET IsDeleted = 1, UpdatedById = {0}, UpdateDate = GETDATE() WHERE Id = {1} AND SchoolId = {2} AND FinancialYearId = {3}",
												  aiUserId,
												  aiLedgerId,
												  aiSchoolId,
												  aiFinancialYearId);
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSqlStatement);
		}
	
		/// <summary>
		/// Returns all Cheque configurations in the system.
		/// </summary>
		/// <returns>A List of ChequeConfiguration entity objects</returns>
		public static List<ChequeConfiguration> GetAllChequeConfigurations()
		{
			var lstChqConfigurations = new List<ChequeConfiguration>();
			string sSqlStatement = "SELECT Id, Name, BankId, ConfigXML, SchoolId FROM Accounts.ChequeConfiguration WHERE IsDeleted = 0";

			using (var oSqlServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oReader = oSqlServerDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
			{
				if (oReader.HasRows)
					while (oReader.Read())
						lstChqConfigurations.Add(new ChequeConfiguration
						                         	{
						                         		Id		  = oReader["Id"].ToInt(),
														Name	  = oReader["Name"].ToString(),
														Bank	  = new Bank { Id = oReader["BankId"].ToInt() },
														ConfigXML = oReader["ConfigXML"].ToString(),
														SchoolId  = oReader["SchoolId"].ToInt()
						                         	});
			}

			return lstChqConfigurations;
		}

		/// <summary>
		/// Saves a Cheque configuration to db.
		/// </summary>
		/// <param name="aoChqConfiguration"></param>
		/// <returns>Number of records affected by the transaction.</returns>
		public static int SaveChequeConfiguration(ChequeConfiguration aoChqConfiguration)
		{
			
			string sSqlStatement;
			if (aoChqConfiguration.Id == 0)
				sSqlStatement = String.Format("INSERT Accounts.ChequeConfiguration" +
											  "		 (BankId, Name, ConfigXML, SchoolId, InsertedById, InsertDate)"+
											  "VALUES ({0}, N'{1}', '{2}', {3}, {4}, GETDATE())",
											   aoChqConfiguration.Bank.Id,
											   StringUtility.ReplaceSingleQuoteInString(aoChqConfiguration.Name, false),
											   StringUtility.ReplaceSingleQuoteInString(aoChqConfiguration.ConfigXML, false),
											   aoChqConfiguration.SchoolId,
											   aoChqConfiguration.InsertedById);
			else
				sSqlStatement = String.Format("UPDATE Accounts.ChequeConfiguration" +
				                              "   SET BankId = {1}, Name = N'{2}', ConfigXML = '{3}', UpdatedById = {4}, UpdateDate = GETDATE()" +
				                              " WHERE Id = {0} AND SchoolId = {5} AND IsDeleted = 0",
											   aoChqConfiguration.Id,
											   aoChqConfiguration.Bank.Id,
											   StringUtility.ReplaceSingleQuoteInString(aoChqConfiguration.Name, false),
											   StringUtility.ReplaceSingleQuoteInString(aoChqConfiguration.ConfigXML, false),
											   aoChqConfiguration.InsertedById,
											   aoChqConfiguration.SchoolId);

			using (var oSqlServerDbUtility = new SQLServerDbUtility())
				return oSqlServerDbUtility.ExecuteTransaction(sSqlStatement);
		}

		/// <summary>
		/// Deletes a Cheque Configuration from the db.
		/// </summary>
		/// <param name="aoChqConfiguration"> </param>
		/// <returns>Number of records affected by the transaction.</returns>
		public static int DeleteChequeConfiguration(ChequeConfiguration aoChqConfiguration)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.ChequeConfiguration SET IsDeleted = 1, UpdatedById = {2}, UpdateDate = GETDATE() WHERE Id = {0} AND SchoolId = {1}",
												  aoChqConfiguration.Id,
												  aoChqConfiguration.SchoolId,
												  aoChqConfiguration.InsertedById);

			using (var oSqlServerDbUtility = new SQLServerDbUtility())
				return oSqlServerDbUtility.ExecuteTransaction(sSqlStatement);
		}

		/// <summary>
		/// Determines if given ChqConfigName already exists for the School.
		/// </summary>
		/// <param name="aoChqConfiguration"> </param>
		/// <returns>true if it already exists, false otherwise.</returns>
		public static bool CheckDuplicateChqConfigName(ChequeConfiguration aoChqConfiguration)
		{
			string sSqlStatement = String.Format("SELECT TOP 1 1 FROM Accounts.ChequeConfiguration WHERE IsDeleted = 0 AND Name = N'{0}' AND Id <> {1} AND SchoolId = {2}",
												  StringUtility.ReplaceSingleQuoteInString(aoChqConfiguration.Name, false),
												  aoChqConfiguration.Id,
												  aoChqConfiguration.SchoolId);

			using (var oSqlServerDbUtility = new SQLServerDbUtility())
				return oSqlServerDbUtility.PerformIntQueryOnSqlServer(sSqlStatement) == 1;
		}
		
		#endregion -- PUBLIC METHOD(s) --
	}
}