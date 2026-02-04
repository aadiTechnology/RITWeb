/* ---------------------------------------------------------------------------------
 *	Filename	: LedgerDC.cs
 *	Author		: Vishal B. Shah
 *	Date		: 5-Oct-2011
 *	Description	: This is the Data Access Layer for Ledgers in the Accounts module.
 * ---------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AccountsEntities;
using Utility;

namespace DataCommunicator
{
	/// <summary>
	/// This is the DAL class for LedgerMaster entity.
	/// </summary>
	public class LedgerDC
	{

		#region -- PROPERTIES --

		/// <summary>
		/// Gets the underlying Ledger entity object.
		/// </summary>
		public Ledger Ledger { get; set; }

		#endregion -- PROPERTIES --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///  This function is used to return a paged list of Ledgers.
		/// </summary>
		/// <returns>A List of LedgerMaster entity objects.</returns>
		public static List<Ledger> GetAll()
		{
			var lstLedgers = new List<Ledger>();
		    
			using (var oSqlDBUtility = new SQLServerDbUtility())
		    using (SqlDataReader oReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("[Accounts].[usp_GetPagedLedgers]"))
		    {
		        while (oReader.Read())
					lstLedgers.Add(new Ledger
					               	{
					               		Id				= oReader["Id"].ToInt(),
										Name			= oReader["Name"].ToString(),
										OriginalLedger	= new Ledger { Id = oReader["OriginalId"].ToInt() },
										Group			= new Group
										        			{
										        				Id			  = oReader["GroupId"].ToInt(),
																Name		  = oReader["GroupName"].ToString(),
																OriginalGroup = new Group { Id = oReader["OriginalGroupId"].ToInt() }
										        			},
										OpeningBalance	= oReader["OpeningBalance"].ToDecimal(),
										Budget			= oReader["Budget"].ToDecimal(),
										IsDebit			= oReader["IsDebit"].ToBool(),
										IsSystemDefined = oReader["IsSystemDefined"].ToBool(),
										SchoolId		= oReader["SchoolId"].ToInt(),
										FinancialYearId = oReader["FinancialYearId"].ToInt(),
                                        PanNo           = oReader["PanNo"].ToString(),
                                        FilePath        = oReader["PanAttachment"].ToString(),
                                        IsPanApplicable = oReader["IsPanRequired"].ToBool()
					               	});
		    }

		    return lstLedgers;
		}

		/// <summary>
		/// This function is used to insert a LedgerMaster object into the db.
		/// </summary>
		/// <param name="aoLedger"></param>
		/// <returns>The id of the inserted record.</returns>
		public static int Insert(Ledger aoLedger)
		{
			string sSqlStatement = String.Format("INSERT INTO Accounts.LedgerMaster" +
                                                 "		 (Name, GroupId, OpeningBalance, IsDebit, Budget, SchoolId, FinancialYearId, InsertedById, PanNo,PanAttachment,UpdateDate) " +
                                                 "VALUES ('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7} , '{8}','{9}',NULL)",
												  StringUtility.ReplaceSingleQuoteInString(aoLedger.Name, false),
												  aoLedger.Group.Id,
												  aoLedger.OpeningBalance,
												  aoLedger.IsDebit ? 1 : 0,
                                                  aoLedger.Budget,
												  aoLedger.SchoolId,
												  aoLedger.FinancialYearId,
												  aoLedger.InsertedById,
                                                  StringUtility.ReplaceSingleQuoteInString(aoLedger.PanNo,false),
                                                  StringUtility.ReplaceSingleQuoteInString(aoLedger.FilePath,false));

			using (var oSqlDBUtility = new SQLServerDbUtility())
				return oSqlDBUtility.ExecuteTransaction(sSqlStatement);
		}

		/// <summary>
		/// Creates ledgers for configured fee types for the school.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiUserId"></param>
		public static void CreateLedgersForFeeType(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId)
		{
			using (var oSqlDBUtility = new SQLServerDbUtility())
			{
				oSqlDBUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDBUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDBUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDBUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);
				
				oSqlDBUtility.ExecuteStoredProcedureOnServer("Accounts.usp_CreateLedgersForFeeType");
			}
		}

		/// <summary>
		/// Creates ledgers for the other fee types configured for the school.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="aiUserId"></param>
		public static void CreateLedgersForStudentPayables(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId)
		{
			using (var oSqlDBUtility = new SQLServerDbUtility())
			{
				oSqlDBUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDBUtility.AddParameter("AcademicYearId"	, aiAcademicYearId , SqlDbType.Int);
				oSqlDBUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDBUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);

				oSqlDBUtility.ExecuteStoredProcedureOnServer("Accounts.usp_CreateLedgersForStudentPayables");
			}
		}

		/// <summary>
		/// Creates a ledger for the given new fee type.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <param name="asFeeType"></param>
		/// <param name="aiUserId"></param>
		public static void CreateLedgerForNewFeeType(int aiSchoolId, int aiFinancialYearId, string asFeeType, int aiUserId)
		{
			using (var oSqlDBUtility = new SQLServerDbUtility())
			{
				oSqlDBUtility.AddParameter("SchoolId"		, aiSchoolId	   , SqlDbType.Int);
				oSqlDBUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				oSqlDBUtility.AddParameter("FeeType"		, asFeeType		   , SqlDbType.NVarChar);
				oSqlDBUtility.AddParameter("UserId"			, aiUserId		   , SqlDbType.Int);
				
				oSqlDBUtility.ExecuteStoredProcedureOnServer("Accounts.usp_CreateLedgerForNewFeeType");
			}
		}

		/// <summary>
		/// Bulk insert ledgers.
		/// </summary>
		/// <param name="sXML"></param>
		/// <returns>The number of rows affected by the transaction.</returns>
		public static int InsertLedgers(string sXML)
		{
			int iCount = 0;
			using (var oSqlDBUtility = new SQLServerDbUtility())
			{
				oSqlDBUtility.AddParameter("LedgersXML", sXML, SqlDbType.Xml);
			    
				using (SqlDataReader oReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_InsertUpdateLedgers"))
			        if (oReader.Read())
			            iCount = Convert.ToInt32(oReader["Count"]);
			}
			return iCount;
		}

		/// <summary>
		/// This function is used to update an existing Ledger entry in the db.
		/// </summary>
		/// <param name="aoLedger"></param>
		/// <returns>The number of rows affected by the transaction.</returns>
		public static int Update(Ledger aoLedger)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.LedgerMaster" +
                                                 "   SET Name = '{1}', GroupId = {2}, OpeningBalance = {3}, IsDebit = {4}, Budget = {5}, UpdatedById = {6}, PanNo = '{7}', PanAttachment='{8}', UpdateDate = GETDATE()" +
												 " WHERE Id = {0}",
												  aoLedger.Id,
												  StringUtility.ReplaceSingleQuoteInString(aoLedger.Name, false),
												  aoLedger.Group.Id,
												  aoLedger.OpeningBalance,
												  aoLedger.IsDebit ? 1 : 0,
                                                  aoLedger.Budget,
												  aoLedger.UpdatedById,
                                                  StringUtility.ReplaceSingleQuoteInString(aoLedger.PanNo,false),
                                                  StringUtility.ReplaceSingleQuoteInString(aoLedger.FilePath,false));

			using (var oSqlDBUtility = new SQLServerDbUtility())
				return oSqlDBUtility.ExecuteTransaction(sSqlStatement);
		}

		/// <summary>
		/// This function is used to delete a Ledger from the LedgerMaster table in the db.
		/// </summary>
		/// <param name="aiLedgerId"></param>
		/// <returns>The number of rows affected by the transaction.</returns>
		public static int Delete(int aiLedgerId, int aiUpdatedById)
		{
			string sSqlStatement = String.Format("UPDATE Accounts.LedgerMaster SET IsDeleted = 1, UpdateDate=GETDATE(), UpdatedById = {1} WHERE Id = {0} AND IsDeleted = 0",
                                                  aiLedgerId, aiUpdatedById);

			using (var oSqlDBUtility = new SQLServerDbUtility())
				return oSqlDBUtility.ExecuteTransaction(sSqlStatement);
		}

		/// <summary>
		/// This function is used to check if the given LedgerName is duplicate name.
		/// </summary>
		/// <param name="aoLedger"></param>
		/// <returns>True if the given name is duplicate, false otherwise.</returns>
		public static bool CheckDuplicateLedgerName(Ledger aoLedger)
		{
			string sSqlStatement = String.Format("SELECT TOP 1 1 FROM Accounts.LedgerMaster WHERE Name = '{0}' AND SchoolId = {1} AND FinancialYearId = {2} AND IsDeleted = 0 AND Id <> {3}",
												  StringUtility.ReplaceSingleQuoteInString(aoLedger.Name, false),
												  aoLedger.SchoolId,
												  aoLedger.FinancialYearId,
												  aoLedger.Id);

			using (var oSqlDBUtility = new SQLServerDbUtility())
				return oSqlDBUtility.PerformIntQueryOnSqlServer(sSqlStatement) != 0;
		}

		/// <summary>
		/// This function is used to check if there are any dependencies for a given Ledger.
		/// </summary>
		/// <param name="aiLedgerId">The id of the Ledger to check dependency for.</param>
		/// <returns>true if there are dependencies for the given LedgerId and false otherwise.</returns>
		public static bool CheckLedgerDependencies(int aiLedgerId)
		{
			string sSqlStatement = String.Format("SELECT TOP 1 1 FROM Accounts.VoucherParticularsDetails WHERE LedgerId = {0} AND IsDeleted = 0",
												  aiLedgerId);

			using (var oSqlDBUtility = new SQLServerDbUtility())
				return oSqlDBUtility.PerformIntQueryOnSqlServer(sSqlStatement) == 1;
		}

        /// <summary>
        /// This method is used to return all the Ledger details with their debit credit values to show the trial balance report.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="aiGroupId"></param>
        /// <returns></returns>
        public static List<Ledger> GetAllLedgerDetails(int aiSchoolId, int aiFinancialYearId, DateTime adtStartDate, DateTime adtEndDate, int aiGroupId)
        {
            List<Ledger> lstLedger = new List<Ledger>();
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSqlDBUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSqlDBUtility.AddParameter("GroupId", aiGroupId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("[Accounts].[usp_GetLedgerDetails]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstLedger.Add(new Ledger
                        {
                            Id = oSqlDataReader["LedgerId"].ToInt(),
                            Name = oSqlDataReader["LedgerName"].ToString(),
                            OriginalLedger = new Ledger { Id = (!oSqlDataReader["OriginalId"].IsNull() ? oSqlDataReader["OriginalId"].ToInt() : 0) },
                            Group = new Group
                            {
                                Id = (!oSqlDataReader["GroupId"].IsNull() ? oSqlDataReader["GroupId"].ToInt() : 0),
                                Debit = oSqlDataReader["Debit"].ToDecimal(),
                                Credit = oSqlDataReader["Credit"].ToDecimal()
                            },

                            IsSystemDefined = oSqlDataReader["IsSystemDefined"].ToBool(),
                            OpeningBalance = oSqlDataReader["OpeningBalance"].ToDecimal(),
                            ClosingBlanace = oSqlDataReader["ClosingBalance"].ToDecimal(),
                            Budget = oSqlDataReader["Budget"].ToDecimal()
                        });
                    }

                    return lstLedger;
                }
            }
        }

		#endregion -- PUBLIC METHOD(s) --

	}
}