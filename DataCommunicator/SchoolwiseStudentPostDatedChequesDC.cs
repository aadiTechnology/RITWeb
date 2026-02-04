// Class Name       :- SchoolwiseStudentPostDatedChequesDC
// Purpose          :- This class is used to manage SchoolwiseStudentPostDatedCheques details.
// Date Of creation :- 9/18/2008
// Author Name      :- 

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
	public class StudentPostDatedChequesDC
	{
		private StudentPostDatedChequesStruct moStudentPostDatedChequesStruct;

		public StudentPostDatedChequesDC()
		{
		}

		public StudentPostDatedChequesDC(int miPostDatedChequeId)
		{
			LoadPostDatedChequesDetails(miPostDatedChequeId);
		}

		public virtual StudentPostDatedChequesStruct StudentPostDatedChequesStructDetails
		{
			get { return moStudentPostDatedChequesStruct; }
			set { moStudentPostDatedChequesStruct = value; }
		}

		// This function is used to insert the SchoolwiseStudentPostDatedCheques Details
		public void InsertStudentPostDatedCheques()
		{
			string sInsertStatement = " INSERT " +
									  " INTO Schoolwise_Student_PostDatedCheques(" +
												"Student_Id" +
												",Cheque_Number" +
												",Cheque_Date" +
												",Bank_Id" +
												",Remarks" +
												",Cheque_Amount" +
												",Inserted_By_id" +
												", School_Id " +
												", Academic_Year_Id " +
												" , Is_PDC " +
									  ")VALUES(" +
												" " + moStudentPostDatedChequesStruct.miStudentId +
												 " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentPostDatedChequesStruct.msChequeNumber, false) + "' " +
												 " , N'" + moStudentPostDatedChequesStruct.mdtChequeDate.ToString("MM/dd/yyyy") + "' " +
												 " , " + moStudentPostDatedChequesStruct.miBankId +
												 " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentPostDatedChequesStruct.msRemarks, false) + "' " +
												 " , " + moStudentPostDatedChequesStruct.miChequeAmount +
												 " , " + moStudentPostDatedChequesStruct.miInsertedByid +
												 " , " + moStudentPostDatedChequesStruct.miSchool_Id +
												 " , " + moStudentPostDatedChequesStruct.miAcademicYr_Id +
												 " , N'" + Constants.C_YES + "'" +
												")";
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
			//return Convert.ToInt32(sReturnValue.Substring(sReturnValue.LastIndexOf(':') + 1));
		}

		// This function is used to update the SchoolwiseStudentPostDatedCheques Details
		public void UpdateStudentPostDatedCheques()
		{
			string sUpdateStatement = " UPDATE " +
									  " Schoolwise_Student_PostDatedCheques " +
									  " SET " +
												"Cheque_Number= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentPostDatedChequesStruct.msChequeNumber, false) + "' " +
                                                ",Cheque_Date= N'" + moStudentPostDatedChequesStruct.mdtChequeDate.ToString("MM/dd/yyyy") + "' " +
												",Bank_Id= " + moStudentPostDatedChequesStruct.miBankId +
												",Remarks= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentPostDatedChequesStruct.msRemarks, false) + "' " +
												",Cheque_Amount= " + moStudentPostDatedChequesStruct.miChequeAmount +
												",Updated_By_Id= " + moStudentPostDatedChequesStruct.miUpdatedById +
									" WHERE PostDated_Cheque_Id=" + moStudentPostDatedChequesStruct.miPostDatedChequeId;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
		}

		// This function is used to delete the SchoolwiseStudentPostDatedCheques Details
		public virtual void DeleteStudentPostDatedCheques()
		{
			string sDeleteStatement = "DELETE Schoolwise_Student_PostDatedCheques WHERE PostDated_Cheque_Id='" + moStudentPostDatedChequesStruct.miPostDatedChequeId + "'";
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
		}

		// This function is used to load the SchoolwiseStudentPostDatedCheques Details
		private void LoadPostDatedChequesDetails(int miPostDatedChequeId)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				string sSelectStatement = FetchStudentChequesDetails(miPostDatedChequeId);
				using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["PostDated_Cheque_Id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miPostDatedChequeId = Convert.ToInt32(oDR["PostDated_Cheque_Id"]);
                            if (oDR["Student_Id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miStudentId = Convert.ToInt32(oDR["Student_Id"]);
                            if (oDR["Cheque_Number"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.msChequeNumber = Convert.ToString(oDR["Cheque_Number"]);
                            if (oDR["Cheque_Date"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.mdtChequeDate = Convert.ToDateTime(oDR["Cheque_Date"]);
                            if (oDR["Bank_Id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miBankId = Convert.ToInt32(oDR["Bank_Id"]);
                            if (oDR["Remarks"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.msRemarks = Convert.ToString(oDR["Remarks"]);
                            if (oDR["Cheque_Amount"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miChequeAmount = Convert.ToInt32(oDR["Cheque_Amount"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miUpdatedById = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["Academic_Year_Id"] != DBNull.Value)
                                moStudentPostDatedChequesStruct.miUpdatedById = Convert.ToInt32(oDR["Academic_Year_Id"]);
                        }
                    }
				}
			}
		}

		// This function is used to fetch the SchoolwiseStudentPostDatedCheques Details
		private string FetchStudentChequesDetails(int miPostDatedChequeId)
		{
			string sSelectStatement = " SELECT  " +
			"PostDated_Cheque_Id" +
			",Student_Id" +
			",Cheque_Number" +
			",Cheque_Date" +
			",Bank_Id" +
			",Remarks" +
			",Cheque_Amount" +
			",Is_Deleted" +
			",Insert_Date" +
			",Inserted_By_id" +
			",Update_Date" +
			",Updated_By_Id" +
			", School_Id" +
			",Academic_Year_Id" +
			" FROM Schoolwise_Student_PostDatedCheques" +
			" WHERE PostDated_Cheque_Id=" + miPostDatedChequeId;
			return sSelectStatement;
		}

		/// <summary>
		/// This method is used to delete particular cheque entry logically.
		/// </summary>
		/// <param name="aiPostDatedChequeId"></param>
		public static void DeleteChequeDetails(int aiPostDatedChequeId)
		{
			string sQuery = " UPDATE " +
							" [Schoolwise_Student_PostDatedCheques] " +
							" SET " +
							" [Is_Deleted] = N'" + Constants.C_YES + "'" +
							" WHERE " +
							" PostDated_Cheque_Id=" + aiPostDatedChequeId;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sQuery);
		}

		/// <summary>
		/// This method is used to get postdated cheque details of a particular student.
		/// </summary>
		/// <param name="aiStudentId"></param>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYrId"></param>
		/// <returns></returns>
		public static DataTable GetStudentPostDatedChequeDetails(int aiStudentId, int aiSchoolId, int aiAcademicYrId)
		{
			string sQuery = " SELECT " +
							" PostDated_Cheque_Id , " +
							" Cheque_Number, " +
							" Cheque_Date, " +
							" Bank_Id, " +
							" Remarks, " +
							" Cheque_Amount " +
							" FROM " +
							" Schoolwise_Student_PostDatedCheques " +
							" WHERE " +
							" Is_Deleted = N'" + Constants.C_NO + "'" +
							" AND " +
							" School_Id = " + aiSchoolId +
							" AND " +
							" Academic_Year_Id = " + aiAcademicYrId +
							" AND " +
							" Student_Id = " + aiStudentId +
							" AND " +
							" Is_PDC =N'" + Constants.C_YES + "'";

			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
		}

		/// <summary>
		/// This method is used to get student fee details.
		/// </summary>
		/// <param name="aiStudentId"></param>
		/// <returns></returns>
		public DataSet GetStudentChequeDetails(int aiStudentId)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetChequeDetails");
			}

		}

		/// <summary>
		/// This method is used to check for duplicate cheque number.
		/// </summary>
		/// <param name="sChequeNo"></param>
		/// <returns></returns>
		public bool IsChequeNoDuplicate(string sChequeNo, int iStudentId)
		{
			string sWhere = String.Empty;
			if (moStudentPostDatedChequesStruct.miPostDatedChequeId != 0)
			{
				sWhere = " AND Postdated_Cheque_Id <> " + moStudentPostDatedChequesStruct.miPostDatedChequeId;
			}
			string sQuery = " SELECT " +
						   " COUNT(Student_Id) " +
						   " FROM " +
						   " Schoolwise_Student_PostDatedCheques " +
						   " WHERE " +
						   " Is_Deleted = N'" + Constants.C_NO + "'" +
						   " AND " +
						   " Cheque_Number = N'" + StringUtility.ReplaceSingleQuoteInString(sChequeNo, false) + "' " +
						   " AND " +
						   " Student_Id =" + iStudentId +
						   " AND " +
						   " Is_Cheque_Bounce=N'" + Constants.C_NO + "'" +
						   sWhere;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
				if (iCount == 0)
					return false;
				else
					return true;
			}
		}

		/// <summary>
		/// This method is used to check for duplicate cheque number.
		/// </summary>
		/// <param name="sChequeNo"></param>
		/// <returns></returns>
		public bool IsSwapNoDuplicate(string sSwapNo, int iStudentId)
		{

			string sQuery = " SELECT " +
						   " COUNT(Student_Id) " +
						   " FROM " +
						   " StudentCardPaymentDetails " +
						   " WHERE " +
						   " Is_Deleted = 0 " +
						   " AND " +
						   " Swap_Number = N'" + StringUtility.ReplaceSingleQuoteInString(sSwapNo, false) + "' " +
						   " AND " +
						   " Student_Id =" + iStudentId;

			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
				if (iCount == 0)
					return false;
				else
					return true;
			}
		}

		/// <summary>
		/// This method is used to check for duplicate cheque number.
		/// </summary>
		/// <param name="sChequeNo"></param>
		/// <returns></returns>
		public bool IsSwapNoDuplicate(string sSwapNo, int iStudentId, int aiReceiptNo, int aiAcademicYrId)
		{

			string sQuery = " SELECT  COUNT(Student_Id) " +
							" FROM    StudentCardPaymentDetails  " +
							" WHERE  Is_Deleted = 0  AND Swap_Number = N'" + StringUtility.ReplaceSingleQuoteInString(sSwapNo, false) + "' " +
							" AND  Student_Id =" + iStudentId +
							" AND SchoolWise_Student_Fee_Id not in (select SchoolWise_Student_Fee_Id " +
																" from Schoolwise_Student_Fee_Details " +
																" where Receipt_Number = " + aiReceiptNo + " and Academic_Year_Id = " + aiAcademicYrId + ")";

			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
				if (iCount == 0)
					return false;
				else
					return true;
			}
		}

		/// <summary>
		/// This method is used to check for duplicate cheque number.
		/// </summary>
		/// <param name="sChequeNo"></param>
		/// <returns></returns>
		public bool IsChequeNoDuplicate(string sChequeNo, int iStudentId, int aiReceiptNo, int aiAcademicYrId)
		{
			string sWhere = " AND PostDated_Cheque_Id NOT IN (SELECT  Student_Fee_PDC_Details.PDC_Id " +
							" FROM   Student_Fee_PDC_Details INNER JOIN " +
							" Schoolwise_Student_Fee_Details ON Student_Fee_PDC_Details.Schoolwise_Student_Fee_Id = Schoolwise_Student_Fee_Details.Schoolwise_Student_Fee_Id " +
							" WHERE	   Schoolwise_Student_Fee_Details.Receipt_Number = " + aiReceiptNo + " AND Schoolwise_Student_Fee_Details.Academic_Year_Id = " + aiAcademicYrId + ")";

			if (moStudentPostDatedChequesStruct.miPostDatedChequeId != 0)
			{
				sWhere += " AND Postdated_Cheque_Id <> " + moStudentPostDatedChequesStruct.miPostDatedChequeId;
			}
			string sQuery = " SELECT " +
						   " COUNT(Student_Id) " +
						   " FROM " +
						   " Schoolwise_Student_PostDatedCheques " +
						   " WHERE " +
						   " Is_Deleted = N'" + Constants.C_NO + "'" +
						   " AND " +
						   " Cheque_Number = N'" + StringUtility.ReplaceSingleQuoteInString(sChequeNo, false) + "' " +
						   " AND " +
						   " Student_Id =" + iStudentId +
						   " AND " +
						   " Is_Cheque_Bounce=N'" + Constants.C_NO + "'" +
						   sWhere;
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
				if (iCount == 0)
					return false;
				else
					return true;
			}
		}

		/// <summary>
		/// Method sets the cheque clearance/passed date to cheque.
		/// </summary>
		public void SetChequeClearance()
		{
			string sQuery = " UPDATE " +
								" schoolwise_student_postdatedcheques " +
							" SET " +
								" cheque_passed_date =N'" + moStudentPostDatedChequesStruct.mdtChequePassedDate + "'" +
								" , update_date = N'" + Constants.S_SERVER_CURRENT_DATE_TIME + "'" +
								" , updated_by_id = " + moStudentPostDatedChequesStruct.miUpdatedById +
							" WHERE " +
								" postdated_cheque_id=" + moStudentPostDatedChequesStruct.miPostDatedChequeId +
								" AND is_deleted = N'" + Constants.C_NO + "'";
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sQuery);
		}

		/// <summary>
		/// Method sets the cheque clearance/passed date to cheque.
		/// </summary>
		public void DeleteChequeClearance()
		{
			string sQuery = " UPDATE " +
								" schoolwise_student_postdatedcheques " +
							" SET " +
								" cheque_passed_date = NULL" +
								" , update_date = N'" + Constants.S_SERVER_CURRENT_DATE_TIME + "'" +
								" , updated_by_id = " + moStudentPostDatedChequesStruct.miUpdatedById +
							" WHERE " +
								" postdated_cheque_id=" + moStudentPostDatedChequesStruct.miPostDatedChequeId +
								" AND is_deleted = N'" + Constants.C_NO + "'";
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sQuery);
		}

		public string GetUpdateStaementForClearanceList()
		{
			string sQuery = " UPDATE " +
								" schoolwise_student_postdatedcheques " +
							" SET " +
								" cheque_passed_date =N'" + moStudentPostDatedChequesStruct.mdtChequePassedDate + "'" +
								" , update_date = N'" + Constants.S_SERVER_CURRENT_DATE_TIME + "'" +
								" , updated_by_id = " + moStudentPostDatedChequesStruct.miUpdatedById +
								" , Cheque_Number = " + moStudentPostDatedChequesStruct.msChequeNumber +
								" , Cheque_Date = N'" + moStudentPostDatedChequesStruct.mdtChequeDate + "'" +
							" WHERE " +
								" postdated_cheque_id=" + moStudentPostDatedChequesStruct.miPostDatedChequeId +
								" AND is_deleted = N'" + Constants.C_NO + "'";
			return sQuery;
		}

		public string GetDeleteStaementForClearanceList()
		{
			string sQuery = " UPDATE " +
								" schoolwise_student_postdatedcheques " +
							" SET " +
								" cheque_passed_date = NULL" +
								" , update_date = N'" + Constants.S_SERVER_CURRENT_DATE_TIME + "'" +
								" , updated_by_id = " + moStudentPostDatedChequesStruct.miUpdatedById +
								" , Cheque_Number = " + moStudentPostDatedChequesStruct.msChequeNumber +
							" WHERE " +
								" postdated_cheque_id=" + moStudentPostDatedChequesStruct.miPostDatedChequeId +
								" AND is_deleted = N'" + Constants.C_NO + "'";
			return sQuery;
		}

		public void SetChequeClearanceDate(ArrayList aoArrayListUpdateStatements)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListUpdateStatements.ToArray(typeof(string)));
		}

		public struct StudentPostDatedChequesStruct
		{
			public int miPostDatedChequeId;
			public int miStudentId;
			public string msChequeNumber;
			public DateTime mdtChequeDate;
			public int miBankId;
			public string msRemarks;
			public int miChequeAmount;
			public string msIsDeleted;
			public DateTime mdtInsertDate;
			public int miInsertedByid;
			public DateTime mdtUpdateDate;
			public int miUpdatedById;
			public int miSchool_Id;
			public int miAcademicYr_Id;
			public DateTime mdtChequePassedDate;
			public string miEnrolment_Number;
		}

        public static DataTable IsDuplicateChequeNo(string asXML, bool abIsInternalFee)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StudentXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DuplicateChequeForClearance");
			}
		}

        public void UpdateStudentPostDatedChequeDetails(string asXML, bool abIsInternalFee)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StudentXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentPostDatedChequeDetails");
			}
		}

		public void UpdateStudentCautionMoneyChequeDetails(string asXML)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("StudentXML", asXML, SqlDbType.Xml);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentCautionMoneyChequeDetails");
			}
		}
	}

	public class StudentChequesCollectionDC
	{
		// This function is used to Fetch the SchoolwiseStudentPostDatedCheques Details
        public static DataTable FetchChequesDetails(string asFilter, int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney, bool abSearchByChequeNo, out int aiTotalAmount, bool abIsInternalFee)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeAllCheques", abIncludeAllCheques, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsCautionMoney", abCautionMoney, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("QueryFilter", Convert.ToInt32(QueryType.StringFilter), SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SearchByChequeNo", abSearchByChequeNo, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("DepositBankId", 0, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                DataSet oDataSet = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_FetchChequeDetails");
                aiTotalAmount = Convert.ToInt32(oDataSet.Tables[1].Rows[0][0]);
                return oDataSet.Tables[0];
			}
		}

		// This function is used to Fetch the SchoolwiseStudentPostDatedCheques Details
        public static DataTable FetchChequesDetails(DateTime adtStartDate, DateTime adtEndDate, int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney, bool abIsPaymentDate,  out int aiTotalAmount, bool abIsInternalFee, int aiBankId)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeAllCheques", abIncludeAllCheques, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsCautionMoney", abCautionMoney, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("QueryFilter", Convert.ToInt32(QueryType.DateFilter), SqlDbType.Int);
               

                if (adtStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.NVarChar);

                if (adtEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsPaymentDate", abIsPaymentDate, SqlDbType.Bit);

                oSQLServerDbUtility.AddParameter("DepositBankId", aiBankId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);

                DataSet oDataSet = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_FetchChequeDetails");
                aiTotalAmount = Convert.ToInt32(oDataSet.Tables[1].Rows[0][0]);
                return oDataSet.Tables[0];
			}
		}

        public static DataTable FetchChequesDetails(int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney,  out int aiTotalAmount, bool abIsInternalFee)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
                oSQLServerDbUtility.AddParameter("SchoolId",aiSchoolId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId",aiAcademicYrId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeAllCheques",abIncludeAllCheques,SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsCautionMoney",abCautionMoney,SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("QueryFilter", Convert.ToInt32(QueryType.NoFilter), SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                DataSet oDataSet = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_FetchChequeDetails");
                aiTotalAmount = Convert.ToInt32(oDataSet.Tables[1].Rows[0][0]);
                return oDataSet.Tables[0];
			}
		}

        //public static DataTable FetchChequesDetails(int aiSchoolId, int aiAcademicYrId, bool abIncludeAllCheques, bool abCautionMoney,int aiDepositBankId, out int aiTotalAmount)
        //{
        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYrId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("IncludeAllCheques", abIncludeAllCheques, SqlDbType.Bit);
        //        oSQLServerDbUtility.AddParameter("IsCautionMoney", abCautionMoney, SqlDbType.Bit);
        //        oSQLServerDbUtility.AddParameter("QueryFilter", Convert.ToInt32(QueryType.NoFilter), SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("DepositBankId", aiDepositBankId, SqlDbType.Int);
        //        DataSet oDataSet = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_FetchChequeDetails");
        //        aiTotalAmount = Convert.ToInt32(oDataSet.Tables[1].Rows[0][0]);
        //        return oDataSet.Tables[0];
        //    }
        //}

        enum QueryType
        {
            NoFilter = 0,
            StringFilter = 1,
            DateFilter = 2
        }
	}
}
