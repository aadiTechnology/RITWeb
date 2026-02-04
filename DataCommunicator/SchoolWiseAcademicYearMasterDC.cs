using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Utility;
using StandardwiseAcademicYear;
using MasterEntities;
using System.Globalization;
using SchoolEntities;

namespace DataCommunicator
{
	public class SchoolWiseAcademicYearMasterDC : DataCommunicatorBaseDC
	{
		#region structure

		public struct SchoolWiseAcademicYearMasterStruct
		{
			public int miSchoolWiseAcademicYearId;
			public int miSchoolId;
			//public int miStartMonthId;
			//public int miStartYear;
			//public int miEndMonthId;
			//public int miEndYearId;
			public DateTime mdtStartdate;
			public DateTime mdtEndDate;

			public DateTime mdtSchoolReOpenDate;
			public string msIsCurrentYear;
			public string msIsCloseYear;
			public string msIs_NewlyCreated;
			public string msIs_FinalYear_Generated;
			public string msIsDeleted;
			public DateTime mdtInsertDate;
			public int miInsertedByid;
			public DateTime mdtUpdateDate;
			public int miUpdatedById;
		}

		#endregion

		#region Data members

		private SchoolWiseAcademicYearMasterStruct moSchoolWiseAcademicYearMasterStruct;

		#endregion

		#region Properties

		public SchoolWiseAcademicYearMasterStruct SchoolWiseAcademicYearMasterStructDetails
		{
			get
			{
				return moSchoolWiseAcademicYearMasterStruct;
			}
			set
			{
				moSchoolWiseAcademicYearMasterStruct = value;
			}
		}

		#endregion

		#region Constructors

		public SchoolWiseAcademicYearMasterDC()
		{
		}
		public SchoolWiseAcademicYearMasterDC(Int32 aiSchoolId, Int32 aiAcademicYearId)
		{
			LoadSchoolWiseAcademicYearMasterDetails(aiSchoolId, aiAcademicYearId);
		}

		#endregion

		#region Private Methods

		public void LoadSchoolWiseAcademicYearMasterDetails(Int32 aiSchoolId, Int32 aiAcademicYearId)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				string sSelectStatement = FetchSchoolWiseAcademicYearMasterDataFromDatabase(aiAcademicYearId);
				using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Academic_Year_ID"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId = Convert.ToInt32(oDR["Academic_Year_ID"].ToString());
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                            if (oDR["Start_date"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.mdtStartdate = Convert.ToDateTime(oDR["Start_date"].ToString());
                            if (oDR["End_Date"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.mdtEndDate = Convert.ToDateTime(oDR["End_Date"].ToString());
                            if (oDR["School_ReOpen_Date"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.mdtSchoolReOpenDate = Convert.ToDateTime(oDR["School_ReOpen_Date"].ToString());
                            if (oDR["Is_Current_Year"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.msIsCurrentYear = oDR["Is_Current_Year"].ToString();
                            if (oDR["Is_Close_Year"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.msIsCloseYear = oDR["Is_Close_Year"].ToString();
                            if (oDR["Is_NewlyCreated"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.msIs_NewlyCreated = oDR["Is_NewlyCreated"].ToString();
                            if (oDR["Is_FinalYear_Generated"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.msIs_FinalYear_Generated = oDR["Is_FinalYear_Generated"].ToString();
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"].ToString());
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolWiseAcademicYearMasterStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());
                        }
                    }
				}
			}
		}

		public string FetchSchoolWiseAcademicYearMasterDataFromDatabase(Int32 aiAcademicYearId)
		{

			string sSelectStatement = " SELECT  " +
				 "Academic_Year_ID" +
				 " , school_id" +
				 " , start_date" +
				 " , end_date" +
				 " , school_reopen_date" +
				 " , is_current_year" +
				 " , Is_Close_Year" +
				 " , Is_NewlyCreated" +
				 " , Is_FinalYear_Generated" +
				 " , is_deleted" +
				 " , insert_date" +
				 " , inserted_by_id" +
				 " , update_date" +
				 " , updated_by_id" +
			 " FROM  " +
				 "SchoolWise_Academic_Year_Master " +
			 " WHERE  " +
				 " Academic_Year_ID = " + aiAcademicYearId +
				 " AND is_deleted = N'" + Constants.C_NO + "'";
			return sSelectStatement;
		}

		#endregion

		#region Public Methods

		public static DataTable GetAllAcademicYearsForSchool(int aiSchoolId, int aiUserId, int aiUserRoleId, bool abIsServiceCall = false)
		{
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, aiUserId, abIsServiceCall))
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("User_Id", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("User_Role_Id", aiUserRoleId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllAcademicYearsForSchool");
			}
		}

        /// <summary>
        /// This method is used to get academic year details by giving school id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetAllAcademicYearsForSchool(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSchoolwiseAcademicYearDetails");
            }
        }

		public static DataTable GetAllAcademicYearsForSuperAdmin(int aiSchoolId, int aiUserId)
		{
			DataTable oDataTable = null;
			string sSqlStatement = "SELECT		DISTINCT "+
												"f.Academic_Year_Id, " +
												"CAST(YEAR(a.Start_date) AS NVARCHAR) + '-' + CAST(YEAR(a.End_Date) AS NVARCHAR) YearValue " +
									"FROM		FileUploadDetails f " +
												"INNER JOIN FileUploadUserDetails s " +
												"ON f.UploadId = s.UploadId " +
												"INNER JOIN SchoolWise_Academic_Year_Master a " +
												"ON f.Academic_Year_Id = a.Academic_Year_ID " +
									"WHERE		f.Is_Deleted = 0 AND s.IsDeleted = 0 AND a.School_Id = " + aiSchoolId.ToString() + " AND s.UserId = " + aiUserId.ToString();

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oDataTable = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSqlStatement);
			}
			
			return oDataTable;
		}

		public Int32 InsertSchoolWiseAcademicYearMaster()
		{
			string sInsertStatement = "INSERT INTO SchoolWise_Academic_Year_Master ( " +
				"  school_id" +
				" , start_date" +
				" , end_date" +
				// " , school_reopen_date" +
				" , is_current_year" +
				" , is_close_year" +
				" , Is_NewlyCreated" +
				" , is_deleted" +
				" , inserted_by_id" +
				" , updated_by_id" +

			") VALUES (" +
				 "  " + moSchoolWiseAcademicYearMasterStruct.miSchoolId +
				 " , N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate + "' " +
				 " , N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate + "' " +
				//     " , '" + moSchoolWiseAcademicYearMasterStruct.mdtSchoolReOpenDate + "' " +
				 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAcademicYearMasterStruct.msIsCurrentYear, false) + "' " +
				 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAcademicYearMasterStruct.msIsCloseYear, false) + "' " +
				 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAcademicYearMasterStruct.msIs_NewlyCreated, false) + "' " +
				 " , N'" + Constants.C_NO + "' " +
				 " , " + moSchoolWiseAcademicYearMasterStruct.miInsertedByid +
				 " , " + moSchoolWiseAcademicYearMasterStruct.miUpdatedById +
			" ) ";
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
		}

		public void UpdateSchoolWiseAcademicYearMaster()
		{

			string sUpdateStatement;
			string[] sArr = new string[2];
			int iCount = 0;
			if(moSchoolWiseAcademicYearMasterStruct.msIsCurrentYear.Equals(Constants.C_YES.ToString()))
			{
				sUpdateStatement = " UPDATE SchoolWise_Academic_Year_Master SET " +
				 "  is_current_year =  N'" + Constants.C_NO + "' " +
				 " , is_close_year =  N'" + Constants.C_YES + "' " +
				 " , updated_by_id =  " + moSchoolWiseAcademicYearMasterStruct.miUpdatedById +
				 " WHERE " +
				 " is_deleted = N'" + Constants.C_NO + "'" +
				 " AND school_Id = N'" + moSchoolWiseAcademicYearMasterStruct.miSchoolId + "'" +
				 " AND is_newlycreated = N'" + Constants.C_NO + "' ";
				sArr[iCount++] = sUpdateStatement;
			}

			sUpdateStatement = " UPDATE SchoolWise_Academic_Year_Master SET " +
				"  school_id =  " + moSchoolWiseAcademicYearMasterStruct.miSchoolId +
				" , start_date =  N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate.ToString("MM/dd/yyyy") + "' " +
				" , end_date =  N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate.ToString("MM/dd/yyyy") + "' " +
				" , is_current_year =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAcademicYearMasterStruct.msIsCurrentYear, false) + "' " +
				" , is_close_year =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAcademicYearMasterStruct.msIsCloseYear, false) + "'" +
				" , Is_NewlyCreated =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseAcademicYearMasterStruct.msIs_NewlyCreated, false) + "'" +
				" , updated_by_id =  " + moSchoolWiseAcademicYearMasterStruct.miUpdatedById +
			 " WHERE " +
				" is_deleted = N'" + Constants.C_NO + "'" +
				 " AND Academic_Year_ID =  " + moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId;
			sArr[iCount] = sUpdateStatement;

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sArr);
		}

		public DataTable GetAllSchoolwiseAcademicYearInfo(Int32 aiSchoolId)
		{
			string sSelectStatement = " SELECT  " +
				" Academic_Year_ID" +
				" , school_id" +
				" , start_date" +
				" , end_date" +
				//     " , school_reopen_date" +
				" , is_current_year" +
				" , is_close_year" +
			 " FROM  " +
				"SchoolWise_Academic_Year_Master " +
			 " WHERE  " +
				 " School_Id = " + aiSchoolId +
				 " AND is_deleted = N'" + Constants.C_NO + "'" +
			 " ORDER  BY start_date";
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
		}

		public void UpdateIsCurrentFlag(Int32 aiSchoolId)
		{
			string sUpdateStatement = " UPDATE SchoolWise_Academic_Year_Master" +
					 " SET Is_Current_Year =N'" + Constants.C_NO + "' " +
				" WHERE School_Id= " + aiSchoolId + " " +
					" AND Is_Current_Year=N'" + Constants.C_YES + "'" +
					" AND is_deleted = N'" + Constants.C_NO + "'";
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.PerformIntQueryOnSqlServer(sUpdateStatement);
		}

		/// <summary>
		/// This method check whether start and end date
		/// is persent in SchoolWise_Academic_Year_Master or not
		/// </summary>        
		/// <returns>Int32</returns>
		public Int32 IsAcademicYrStartAndEndDtPredefined()
		{
			StringBuilder sFilter = new StringBuilder();
			sFilter.Append(" WHERE " +
                                                            "( ( N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate.ToString("yyyy-MM-dd") + "' " +
													   "BETWEEN Start_Date AND End_Date ) " +
															" OR " +
                                                            "( N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate.ToString("yyyy-MM-dd") + "' " +
													   "BETWEEN Start_Date AND End_Date) " +
															" OR " +
                                                            "( Start_Date BETWEEN N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate.ToString("yyyy-MM-dd") + "' " +
                                                       " AND  N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate.ToString("yyyy-MM-dd") + "')" +
															" OR " +
                                                            "( End_Date BETWEEN N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate.ToString("yyyy-MM-dd") + "' " +
                                                       " AND  N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate.ToString("yyyy-MM-dd") + "') )" +
													   " AND ( School_Id = " + moSchoolWiseAcademicYearMasterStruct.miSchoolId + " ) " +
													   " AND Is_Deleted= N'" + Constants.C_NO + "'");

			if(moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId != Constants.I_ZERO)
			{
				sFilter.Append(" AND  Academic_Year_ID  <> " + moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId + "");
			}

			string sSelectStatment = "SELECT " +
										 " COUNT(Academic_Year_ID) " +
									 " FROM SchoolWise_Academic_Year_Master " +
									 sFilter.ToString();

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment);
		}

		/// <summary>
		/// This method is used to check the availability of academic years before current academic year.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <returns></returns>
        public static DataTable GetPassedAcademicYears(int aiSchoolId, int aiStudentId, bool abIncludeCurrentYear)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeCurrentYear", abIncludeCurrentYear, SqlDbType.Bit);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPassedAcademicYears");
			}
		}

        public static DataSet GetPendingFeeAcademicYears(int aiSchoolId, int aiStudentId, int aiAcademicYearId, bool bIsInternalFee, bool abIsAdvanceFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", bIsInternalFee, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsAdvanceFee", abIsAdvanceFee, SqlDbType.Bit);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetPendingFeeAcademicYears");
            }
        }
        /// <summary>
        /// This method is used to return mid or current academic year depending on ShowAdmissionForCurrentYear flag.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static string GetAcademicYearForOnlineAdmission(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);               
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("AcademicYear", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 10);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetAcademicYearForAdmission");
                return Convert.ToString(oSqlParameter.Value);
            }
        }


		/// <summary>
		/// This method check whether start and end date
		/// is persent in SchoolWise_Academic_Year_Master or not
		/// </summary>        
		/// <returns>Int32</returns>
		public Int32 IsAcademicYrOtherThanNewDtPredefined()
		{
			StringBuilder sFilter = new StringBuilder();
			sFilter.Append(" WHERE " +
														  "( ( N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate + "' " +
													 "BETWEEN Start_Date AND End_Date ) " +
														  " OR " +
														  "( N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate + "' " +
													 "BETWEEN Start_Date AND End_Date) " +
														  " OR " +
														  "( Start_Date BETWEEN N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate + "' " +
													 " AND  N'" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate + "')" +
														  " OR " +
														  "( End_Date BETWEEN N'" + moSchoolWiseAcademicYearMasterStruct.mdtStartdate + "' " +
													 " AND  '" + moSchoolWiseAcademicYearMasterStruct.mdtEndDate + "') )" +
													 " AND ( School_Id = " + moSchoolWiseAcademicYearMasterStruct.miSchoolId + " ) " +
													 " AND Is_Deleted= N'" + Constants.C_NO + "'" +
													 " AND Is_NewlyCreated= N'" + Constants.C_NO + "'");

			if(moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId != Constants.I_ZERO)
			{
				sFilter.Append(" AND  Academic_Year_ID  <> " + moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId + "");
			}

			string sSelectStatment = "SELECT " +
										 " COUNT(Academic_Year_ID) " +
									 " FROM SchoolWise_Academic_Year_Master " +
									 sFilter.ToString();

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment);
		}

		/// <summary>
		/// This method is used to get next configured year from database.
		/// </summary>
		/// <returns></returns>
        public DataSet GetNextConfiguredAcademicYear(int miSchoolId, string acAdmissionForCurrentYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AdmissionForCurrentYear", acAdmissionForCurrentYear, SqlDbType.VarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetNextNotConfiguredAcademicYear");
            }
        }

		/// <summary>
		/// This method is used to get academic year as well school organisation name.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public DataTable GetSchoolInfo(int aiSchoolId, int aiAcademicYearId)
		{
			string sSelectStatement = " SELECT " +
									  " CAST(YEAR(SchoolWise_Academic_Year_Master.Start_date) AS varchar) + '-' + CAST(YEAR(SchoolWise_Academic_Year_Master.End_Date) " +
									  " AS varchar) AS Year " +
									  " , " +
									  " School_Master.School_Orgn_Name " +
									  " FROM " +
									  " SchoolWise_Academic_Year_Master " +
									  " INNER JOIN " +
									  " School_Master " +
									  " ON " +
									  " SchoolWise_Academic_Year_Master.School_Id = School_Master.School_Id " +
									  " WHERE " +
									  " SchoolWise_Academic_Year_Master.School_Id = " + aiSchoolId +
									  " AND " +
									  " SchoolWise_Academic_Year_Master.Academic_Year_ID = " + aiAcademicYearId;
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
		}

		/// <summary>
		/// This method is used to generate next year data.
		/// </summary>
		/// <param name="asConfIds"></param>
		public void GenerateNextYearData(string asConfIds, DateTime oStartDate, DateTime oEndDate, Boolean bGenerateRollNos, Boolean bGenerateRegNos, Boolean bGenerateDebitEntries, Boolean bGenerateTransportData, Boolean bIsOnlyInMidAcademic)
		{
			int iGenerateRollNos = bGenerateRollNos ? 1 : 0;
			int iGenerateRegNos = bGenerateRegNos ? 1 : 0;
			int iGenerateDebitEntries = bGenerateDebitEntries ? 1 : 0;
			int iGenerateTransportData = bGenerateTransportData ? 1 : 0;

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SelectedConfigXML", asConfIds, SqlDbType.Xml);
				oSQLServerDbUtility.AddParameter("iSchoolId", moSchoolWiseAcademicYearMasterStruct.miSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("iAcadmicYearId", moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StartDate", oStartDate, SqlDbType.DateTime);
				oSQLServerDbUtility.AddParameter("EndDate", oEndDate, SqlDbType.DateTime);
				oSQLServerDbUtility.AddParameter("bGenerateRollNos", iGenerateRollNos, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("bGenerateRegNos", bGenerateRegNos, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("bGenerateDebitEntries", iGenerateDebitEntries, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("bGenerateTransportData", iGenerateTransportData, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("IsOnlyInMidAcademic", bIsOnlyInMidAcademic ? "Y" : "N", SqlDbType.NVarChar);
				oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GenerateNextAcademicData",true);
			}
		}

       
		#endregion

		public static List<StandardwiseAcademicYearEntity> GetStandardwiseAcademicYear(int iSchoolId, int iAcademicYearId)
		{
			List<StandardwiseAcademicYearEntity> lstStandardwiseAcademicYear = new List<StandardwiseAcademicYearEntity>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_Id", iAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardwiseAcademicYear"))
                {
                    StandardwiseAcademicYearEntity oStandardwiseAcademicYearEntity;
                    while (oSqlDataReader.Read())
                    {
                        oStandardwiseAcademicYearEntity = new StandardwiseAcademicYearEntity
                        {
                            StandardwiseAcademicYearId = Convert.ToInt32(oSqlDataReader["StandardwiseAcademicYearId"]),
                            StandardId = Convert.ToInt32(oSqlDataReader["StandardID"]),
                            StandardName = Convert.ToString(oSqlDataReader["StandardName"]),
                            StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
                            EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]),
                            SchoolReopeningDate = Convert.ToDateTime(oSqlDataReader["SchoolReopningDate"]),
                        };
                        lstStandardwiseAcademicYear.Add(oStandardwiseAcademicYearEntity);
                    }
                    return lstStandardwiseAcademicYear;
                }
            }
		}

		public void SaveStandardwiseAcademicYear(string StandardwiseAcademicYearXML)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StandardwiseAcademicYearXML", StandardwiseAcademicYearXML, SqlDbType.Xml);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStandardwiseAcademicYear");
			}
		}

		public void CheckOverlappingofStandardwiseAcademicYear(string StandardwiseAcademicYearXML, out string Message)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StandardwiseAcademicYearXML", StandardwiseAcademicYearXML, SqlDbType.Xml);
				SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Message", null, SqlDbType.NVarChar, ParameterDirection.Output, 4000);

				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CheckOverlappedAcademicYear");
				Message = Convert.ToString(oSqlParameter.Value);
			}
		}

        public DataTable GetAcademicYearsforStudentFeeChallan(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicYearsForChallanImport");
            }
        }

        /// <summary>
        /// this method is used for get the Student id and Standard id for Challan import.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataTable GetStudentIdAndStandardIdForChallan(int aiSchoolId, int aiAcademicYearId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NewAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentIdAndStandardIdForChallan");
            }
        }

		public static DataTable GetAcademicDatesForStandard(int iSchoolID, int iAcademicYearID, int StandardId)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolID", iSchoolID, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearID", iAcademicYearID, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StandardId", StandardId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicDatesForStandard");
			}

		}

		public static DataTable GetAcademicDatesForStudent(int iSchoolID, int iAcademicYearID, string RegNo)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolID", iSchoolID, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearID", iAcademicYearID, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("RegNo", RegNo, SqlDbType.NVarChar);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicDatesForStudent");
			}
		}

		public static DataTable GetAcademicYearForStudent(int iSchoolID, int iAcademicYearID, int StudentId)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolID", iSchoolID, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearID", iAcademicYearID, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StudentId", StudentId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicYearForStudent");
			}
		}

		public static DataTable GetAcademicDatesForStandardDivision(int iSchoolId, int iAcademicYearId, int StandardDivisionId)
		{
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolID", iSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearID", iAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("StandardDivisionId", StandardDivisionId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAcademicDatesForStandardDivision");
			}
		}

        public static List<string> GetYearsForAnnualPalanner(int aiSchoolId)
        {
            List<string> oLstYear = new List<string>();
            string sSelectStmt = " SELECT YEAR(Start_date) as Year FROM SchoolWise_Academic_Year_Master where School_Id=+" + aiSchoolId +
                                 " AND Is_NewlyCreated='N'"+
                                 " UNION "+
                                 " SELECT YEAR(End_Date) as Year FROM SchoolWise_Academic_Year_Master where School_Id=" + aiSchoolId +
                                 " AND Is_NewlyCreated='N' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oLstYear.Add(oSqlDataReader["Year"].ToString());
                        }
                    }
                }
                return oLstYear;
            }
        }

        public static List<MonthMaster> GetAllMonth()
        {
            List<MonthMaster> oLstMonths = new List<MonthMaster>();
            MonthMaster oMonthMaster;
            string sSelectStmt = "SELECT MonthID, Month,MonthAbbreviation from MonthsOfYear";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oMonthMaster = new MonthMaster
                            {
                                MonthId = Convert.ToInt32(oSqlDataReader["MonthID"]),
                                Month = Convert.ToString(oSqlDataReader["Month"]),
                                MonthAbbreviation = Convert.ToString(oSqlDataReader["MonthAbbreviation"])
                            };
                            oLstMonths.Add(oMonthMaster);
                        }
                    }
                }
                return oLstMonths;
            }

        }

        public string IsNewlyCreated(int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = "SELECT Is_NewlyCreated FROM SchoolWise_Academic_Year_Master" +
                                      " WHERE Academic_Year_ID = " + aiAcademicYearId +
                                      " AND School_Id = " + aiSchoolId +
                                      " AND Is_Deleted = N'" + Constants.S_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    while (oSqlDataReader.Read())
                    {
                        return oSqlDataReader["Is_NewlyCreated"].ToString();
                    }
                }
            };
            return Constants.S_NO;
        }

        /// <summary>
        /// This method is used to check report is empty or not.
        /// </summary>
        /// <param name="asStandardwiseAcademicYearXML"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcadYearID"></param>
        /// <returns></returns>
        public static bool IsReportEmpty(string asStandardwiseAcademicYearXML, int aiSchoolID, int aiAcadYearID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcadYearID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearXML", asStandardwiseAcademicYearXML, SqlDbType.Xml);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_IsOutofAcademicYearStudentListEmpty"))
                {
                    if (oSqlDataReader.Read())
                        return oSqlDataReader[0].ToBool();
                    else return false;
                }
            }
        }

        public static List<AcademicYear> GetAllYears(int aiSchoolId)
		{
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllYears"))
                {
                    List<AcademicYear> lstYears = new List<AcademicYear>();

                    while (oSqlDataReader.Read())
                    {
                        lstYears.Add(new AcademicYear { Id = Convert.ToInt32(oSqlDataReader["Academic_Year_Id"]), Year = oSqlDataReader["Year"].ToString() });
                    }

                    return lstYears;
                }
            }
		}
    }
}
