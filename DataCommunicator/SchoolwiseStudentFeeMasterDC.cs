using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Utility;
using System.Collections.Generic;

namespace DataCommunicator
{

    public class SchoolwiseStudentFeeMasterDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct SchoolwiseStudentFeeMasterStruct
        {
            public int miSchoolwiseStudentFeeId;
            public DateTime mdtPaymentdate;
            public int miYearwisestudentId;
            public int miStandardFeeTypeId;
            public int miDueAmount;
            public int miConcessionAmount;
            public int miLateFeeAmount;
            public int miTotalFeeAmount;
            public string msReceiptNumber;
            public string msChequeNumber;
            public DateTime mdtChequeDate;
            public string msBankName;
            public string msRemarks;
            public string msDescription;
            public int miInterval;
            public int miSchoolId;
            public int miAcademicYearId;
            public int miInsertedById;
            public DateTime mdtInsertDate;
            public int miUpdatedById;
            public DateTime mdtUpdateDate;
            public string msIsdeleted;
            public string msGUID;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private SchoolwiseStudentFeeMasterStruct moSchoolwiseStudentFeeMasterStruct;

        #endregion
        #region Properties

        public SchoolwiseStudentFeeMasterStruct SchoolwiseStudentFeeMasterStructDetails
        {

            get { return moSchoolwiseStudentFeeMasterStruct; }
            set { moSchoolwiseStudentFeeMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SchoolwiseStudentFeeMasterDC()
        {
        }
        public SchoolwiseStudentFeeMasterDC(int aiId)
        {
            LoadSchoolwiseStudentFeeMasterDetails(aiId);
        }
        #endregion

        #region Private Methods

        public void LoadSchoolwiseStudentFeeMasterDetails(int aiId)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseStudentFeeMasterDataFromDatabase(aiId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {

                            if (oDR["Schoolwise_Student_Fee_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miSchoolwiseStudentFeeId = Convert.ToInt32(oDR["Schoolwise_Student_Fee_Id"].ToString());
                            if (oDR["Payment_date"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.mdtPaymentdate = Convert.ToDateTime(oDR["Payment_date"].ToString());
                            if (oDR["Yearwise_student_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId = Convert.ToInt32(oDR["Yearwise_student_Id"].ToString());
                            if (oDR["Standard_Fee_Type_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId = Convert.ToInt32(oDR["Standard_Fee_Type_Id"].ToString());
                            if (oDR["Due_Amount"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miDueAmount = Convert.ToInt32(oDR["Due_Amount"].ToString());
                            if (oDR["Late_Fee_Amount"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miLateFeeAmount = Convert.ToInt32(oDR["Late_Fee_Amount"].ToString());
                            if (oDR["Total_Fee_Amount"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miTotalFeeAmount = Convert.ToInt32(oDR["Total_Fee_Amount"].ToString());
                            if (oDR["Receipt_Number"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.msReceiptNumber = oDR["Receipt_Number"].ToString();
                            if (oDR["Interval"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miInterval = Convert.ToInt32(oDR["Interval"].ToString());
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                            if (oDR["Academic_Year_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miAcademicYearId = Convert.ToInt32(oDR["Academic_Year_Id"].ToString());
                            if (oDR["Inserted_By_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"].ToString());
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                            if (oDR["Is_deleted"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.msIsdeleted = oDR["Is_deleted"].ToString();
                            if (oDR["GUID"] != DBNull.Value)
                                moSchoolwiseStudentFeeMasterStruct.msIsdeleted = oDR["GUID"].ToString();
                        }
                    }
                }
            }
        }
        public string FetchSchoolwiseStudentFeeMasterDataFromDatabase(int aiId)
        {

            string sSelectStatement = " SELECT  " +
                "schoolwise_student_fee_id" +
                " , payment_date" +
                " , yearwise_student_id" +
                " , standard_fee_type_id" +
                " , due_amount" +
                " , late_fee_amount" +
                " , total_fee_amount" +
                " , receipt_number" +
                " , interval" +
                " , school_id" +
                " , academic_year_id" +
                " , inserted_by_id" +
                " , insert_date" +
                " , updated_by_id" +
                " , update_date" +
                " , is_deleted" +

            " FROM  " +
                "Schoolwise_Student_Fee_Master " +
            " WHERE  " +
                 "schoolwise_student_fee_id = " + aiId +
                " AND is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        #endregion

        #region Public Methods
   
        public DataSet GetIntervals()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", moSchoolwiseStudentFeeMasterStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", moSchoolwiseStudentFeeMasterStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Fee_Id", moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Yearwise_Student_Id", moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_Get_Intervals");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoFees"></param>
        /// <param name="aoLateFee"></param>
        /// <returns></returns>
        private ArrayList InsertForEveryInterval( Hashtable aoLateFee)
        {
            ArrayList sArrInsertStmt = new ArrayList();
            int iCount = aoLateFee.Count;
            int iAmount = moSchoolwiseStudentFeeMasterStruct.miDueAmount / moSchoolwiseStudentFeeMasterStruct.miInterval;
            //  aoLateFee.
            int i = 0;
            int[] iArrConcession = new int[aoLateFee.Count];
            int iAmt =  moSchoolwiseStudentFeeMasterStruct.miConcessionAmount / moSchoolwiseStudentFeeMasterStruct.miInterval;
            for(i=0;i<iArrConcession.Length; i++)
            {
                iArrConcession[i] = iAmt;
            }
            if (moSchoolwiseStudentFeeMasterStruct.miConcessionAmount % moSchoolwiseStudentFeeMasterStruct.miInterval != 0)
            {
                iArrConcession[i - 1] += moSchoolwiseStudentFeeMasterStruct.miConcessionAmount % moSchoolwiseStudentFeeMasterStruct.miInterval; 
            }
            i = 0;
            foreach (DictionaryEntry oLateFees in aoLateFee)
            {
                int iTot = iAmount + Convert.ToInt32(oLateFees.Value);
                string sInsertStmt = "";
                sInsertStmt = "INSERT INTO Schoolwise_Student_Fees_ForReport  (" +
                         "  payment_date" +
                " , yearwise_student_id" +
                " , standard_fee_type_id" +
                " , Concession_Amount " +
                " , due_amount" +
                " , late_fee_amount" +
                " , total_fee_amount" +
                " , receipt_number" +
                " , interval" +
                " , Description" +
                " , school_id" +
                " , academic_year_id" +
                " , inserted_by_id" +
                " , insert_date" +
                " , updated_by_id" +
                " , is_deleted" +
                 ") VALUES (" + "  " +
                 "  '" + moSchoolwiseStudentFeeMasterStruct.mdtPaymentdate + "' " +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId +
                 " , " +iArrConcession[i] +
                 " , " + iAmount +
                 " , " + oLateFees.Value +
                 " , " + iTot +
                 " , " + "dbo.Udf_NextReceiptNo(" + moSchoolwiseStudentFeeMasterStruct.miSchoolId + "," +
                                     moSchoolwiseStudentFeeMasterStruct.miAcademicYearId + ")" +
                 " , 1" +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msDescription, true) + "' " +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miSchoolId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miAcademicYearId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miInsertedById +
                 " , N'" + moSchoolwiseStudentFeeMasterStruct.mdtInsertDate + "' " +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miUpdatedById +
                 " , N'" + Constants.C_NO + "' " +
                      " ) ";
                sArrInsertStmt.Add(sInsertStmt);
                i = i + 1;

            }
            return sArrInsertStmt;

        }
        public void InsertSchoolwiseStudentFeeMaster( Hashtable aoLateFee)
        {
            ArrayList sArrInsert = new ArrayList(); //contains query strings to form transactions
            //statement to get value of last inserted key (student id)


            sArrInsert = InsertForEveryInterval( aoLateFee);


            //sArrInsert = oInsertStmts.ToArray(System.String);


                        
           string str = "INSERT INTO Schoolwise_Student_Fee_Master ( " +
                "  payment_date" +
                " , yearwise_student_id" +
                " , standard_fee_type_id" +
                " , due_amount" +
                " , Concession_Amount " +
                " , late_fee_amount" +
                " , total_fee_amount" +
                " , receipt_number" +
                " , interval" +
                " , Description" +
                " , school_id" +
                " , academic_year_id" +
                " , inserted_by_id" +
                " , insert_date" +
                " , updated_by_id" +
                " , is_deleted" +
                " , Cheque_Number" +
                " , Bank_Name " +
                " , Cheque_Date " +
                " , Remarks " +
                " , GUID " +
            ") VALUES (" + "  " +
                 "  '" + moSchoolwiseStudentFeeMasterStruct.mdtPaymentdate + "' " +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miDueAmount +
                  " , " + moSchoolwiseStudentFeeMasterStruct.miConcessionAmount +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miLateFeeAmount +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miTotalFeeAmount +
                 " , " + "dbo.Udf_NextReceiptNo("+ moSchoolwiseStudentFeeMasterStruct.miSchoolId+","+
                                     moSchoolwiseStudentFeeMasterStruct.miAcademicYearId + ")"+
                 " , " + moSchoolwiseStudentFeeMasterStruct.miInterval +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msDescription,true) + "' " +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miSchoolId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miAcademicYearId +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miInsertedById +
                 " , N'" + moSchoolwiseStudentFeeMasterStruct.mdtInsertDate + "' " +
                 " , " + moSchoolwiseStudentFeeMasterStruct.miUpdatedById +
                 " , N'" + Constants.C_NO+ "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msChequeNumber,true) + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msBankName, true) + "' " +
                 " , N'" + moSchoolwiseStudentFeeMasterStruct.mdtChequeDate + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msRemarks, true) + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msGUID, true) + "' " +
            " ) ";
           sArrInsert.Add(str);
           str = GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY);
           sArrInsert.Add(str);
           str = "INSERT INTO Schoolwise_Fee_Subtype_Backup_details  (" +
                            " Student_Fee_Id " +
                            " ,[Fee_Subtype_Id]" +
                           " ,[Fee_Amount]" +
                           " ,[Inserted_By_Id]" +
                           " ,[Insert_Date]" +
                           " ,[Updated_By_Id]" +
                           ",[Is_Deleted])" +
                       "SELECT " +
                       "  '" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                       ",[Fee_SubType_Id]" +
                       ",[Fee_Amount] " +
                       " , " + moSchoolwiseStudentFeeMasterStruct.miInsertedById +
                       " , N'" + moSchoolwiseStudentFeeMasterStruct.mdtInsertDate + "' " +
                       " , " + moSchoolwiseStudentFeeMasterStruct.miUpdatedById +
                       " , N'" + Constants.C_NO + "' "+
                       " FROM vw_Standard_FeeSubtypeConfiguration " +
                       " WHERE Schoolwise_Standard_FeeType_Id = "
                       +moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId;
            sArrInsert.Add(str);
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])sArrInsert.ToArray(typeof(string)));
            
        }
        public void UpdateSchoolwiseStudentFeeMaster()
        {

            string sUpdateStatement = " UPDATE Schoolwise_Student_Fee_Master SET " +
                "schoolwise_student_fee_id =  " + moSchoolwiseStudentFeeMasterStruct.miSchoolwiseStudentFeeId +
                " , payment_date =  N'" + moSchoolwiseStudentFeeMasterStruct.mdtPaymentdate + "' " +
                " , yearwise_student_id =  " + moSchoolwiseStudentFeeMasterStruct.miYearwisestudentId +
                " , standard_fee_type_id =  " + moSchoolwiseStudentFeeMasterStruct.miStandardFeeTypeId +
                " , due_amount =  " + moSchoolwiseStudentFeeMasterStruct.miDueAmount +
                " , late_fee_amount =  " + moSchoolwiseStudentFeeMasterStruct.miLateFeeAmount +
                " , total_fee_amount =  " + moSchoolwiseStudentFeeMasterStruct.miTotalFeeAmount +
                " , receipt_number =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msReceiptNumber, false) + "' " +
                " , interval =  " + moSchoolwiseStudentFeeMasterStruct.miInterval +
                " , school_id =  " + moSchoolwiseStudentFeeMasterStruct.miSchoolId +
                " , academic_year_id =  " + moSchoolwiseStudentFeeMasterStruct.miAcademicYearId +
                " , inserted_by_id =  " + moSchoolwiseStudentFeeMasterStruct.miInsertedById +
                " , insert_date =  N'" + moSchoolwiseStudentFeeMasterStruct.mdtInsertDate + "' " +
                " , updated_by_id =  " + moSchoolwiseStudentFeeMasterStruct.miUpdatedById +
                " , update_date =  N'" + moSchoolwiseStudentFeeMasterStruct.mdtUpdateDate + "' " +
                " , is_deleted =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseStudentFeeMasterStruct.msIsdeleted, false) + "' " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_student_fee_id =  " + moSchoolwiseStudentFeeMasterStruct.miSchoolwiseStudentFeeId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }
        public void DeleteSchoolwiseStudentFeeMaster()
        {

            string sUpdateStatement = " UPDATE Schoolwise_Student_Fee_Master SET " +
                 " is_deleted = N'" + Constants.C_YES + "'" +
                " , update_date = " + Constants.S_SERVER_CURRENT_DATE_TIME +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_student_fee_id =  " + moSchoolwiseStudentFeeMasterStruct.miSchoolwiseStudentFeeId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }
        public static DataSet GetFeeDetailsForStudent(int aiYearwiseStudentId)
        {
            string sSelectStatement = " SELECT  " +
                                        " standard_fee_type_id " +
                                        " , fee_type_id " +
                                        " , standard_id " +
                                        " , schoolwise_student_fee_id " +
                                        " , yearwise_student_id " +
                                        " , receipt_number " +
                                        " , fee_type " +
                                        " , payment_date " +
                                        " , intervalname " +
                                        " , due_amount " +
                                        " , late_fee_amount " +
                                        " , Concession_Amount " +
                                        " , total_fee_amount " + 
                                      " FROM  " +
                                        " vw_StudentFee_Details " +
                                      " WHERE  " +
                                        "Yearwise_Student_Id = " + aiYearwiseStudentId +
                                       " AND is_deleted = N'" + Constants.C_NO + "'"+
                                       " ORDER BY Payment_date";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);
 
        }
        public static DataTable GetPaymentDetailsForReciept(int aiStudentFeesPaymentId)
        {
            // Method requires almost all fields except 2 among 24 fields. Thus "*" is not replaced with actual field names.
            string sSelectStatement = " SELECT  " +
                                      " * " +
                                      " FROM  " +
                                        " vw_StudentFee_Details " +
                                      " WHERE  " +
                                      "  schoolwise_student_fee_id = '" + aiStudentFeesPaymentId + "'"+
            " AND is_deleted = '" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())                   
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }
        public static DataSet GetPaymentDetailsForTermReciept(int aiSchoolId,int aiAcademicYearId,int aiStudentFeesPaymentId)
        {  
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Fees_Payment_Id", aiStudentFeesPaymentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_getStudentPaymentDetailsForTermReciept");
            }
        }

        /// <summary>
        /// This method is used to get count of GUID. 
        /// </summary>
        /// <param name="asGUID"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static int GetGUIDCnt(string asGUID, int aiStudentId)
        {
            string sQuery = " SELECT " +
                            " COUNT(*) " +
                            " FROM  " +
                                  " Schoolwise_Student_Fee_Master " +
                            " WHERE " +
                            " Yearwise_student_Id= " + aiStudentId +
                            " AND " +
                            " GUID='" + asGUID + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
        }

        /// <summary>
        /// This method is used to get last fee entry for particular student and for particular fee type.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiStdFeeTypeId"></param>
        /// <returns></returns>
        public static int GetLastFeeEntry(int aiStudentId, int aiStdFeeTypeId)
        {
            string sQuery = " SELECT " +
                            " MAX(Schoolwise_Student_Fee_Id) " +
                            " FROM " +
                                " Schoolwise_Student_Fee_Master " +
                            " WHERE " +
                            " Standard_Fee_Type_Id = " + aiStdFeeTypeId +
                            " AND " +
                            " Yearwise_student_Id = " + aiStudentId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sQuery);
        }

        /// <summary>
        /// This method is used to update remarks of a particular transaction.
        /// </summary>
        /// <param name="aiStudentFeeId"></param>
        /// <param name="asRemarks"></param>
        public static void UpdateStudentReamrks(int aiStudentFeeId, string asRemarks)
        {
            string sQuery = " UPDATE " +
                            " Schoolwise_Student_Fee_Master " +
                            " SET " +
                            " Remarks = N'" + StringUtility.ReplaceSingleQuoteInString(asRemarks, true) + "'" +
                            " WHERE " +
                            " Schoolwise_Student_Fee_Id= " + aiStudentFeeId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery);
        }

        /// <summary>
        /// This method is used to delete student's fee details.
        /// </summary>
        /// <param name="aiStudentFeesPaymentId"></param>
        /// <param name="aiYrwise_Student_Id"></param>
        /// <param name="aiReceipt_No"></param>
        public static void DeleteStudentDetails(int aiStudentFeeId,int aiYrwise_Student_Id,
                                                                                         string asReceipt_No)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Student_Fee_Id", aiStudentFeeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Yrwise_Student_Id", aiYrwise_Student_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Receipt_Number", asReceipt_No, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_Dlete_StudentFee_Details",true);
            }
        }

        /// <summary>
        /// This method is used to delete student fee details if cheque is bounced.
        /// </summary>
        /// <param name="aiYrwiseStudentId"></param>
        /// <param name="aiIntervalCnt"></param>
        /// <param name="aiStdFeeId"></param>
        public static void DeleteStudentFeeDetails(int aiYrwiseStudentId, int aiIntervalCnt, int aiStdFeeId)
        {            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Yrwise_Student_Id", aiYrwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IntervalCnt", aiIntervalCnt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Std_Fee_Type_Id", aiStdFeeId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_Delete_Student_BounceFeeDetails",true);
            }
        }

        public static DataTable GetFeeTypes(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPayableFeeTypes");
            }
        }

        /// <summary>
        /// This method is used to return all active ledger ids of selected financial year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<int> GetActiveLedgersIds(int aiSchoolId, int aiFinancialYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("accounts.usp_GetAllactiveLedgers"))
                {
                    List<int> lstLedgerIds = new List<int>();

                    while (oSqlDataReader.Read())
                    {
                        lstLedgerIds.Add(Convert.ToInt32(oSqlDataReader["LedgerId"]));
                    }

                    return lstLedgerIds;
                }
            }
        }


        #endregion

    }

}
