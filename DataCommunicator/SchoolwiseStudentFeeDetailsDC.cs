// Class Name       :- SchoolwiseStudentFeeDetailsDC
// Purpose          :- This class is used to manage SchoolwiseStudentFeeDetails details.
// Date Of creation :- 9/19/2008
// Author Name      :- Anu

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FeeEntities;
using SchoolEntities.Accounts;
using SchoolEntities.StudentFee;
using Utility;
using SchoolEntities;
using SchoolEntities.Dashboard;
using StudentEntities;
 
namespace DataCommunicator
{
    public class StudentFeeDetailsDC
    {
        #region Data Member(s)

        private StudentFeeDetailsStruct moStudentFeeDetailsStruct;
        private List<StudentPayFeeDetails> mlstStudentPayFeeDetails;
        private List<ChequeDetails> mlstChequeDetails;
        private EditFeeDetails moEditFeeDetails;
        private SwapCardDetails moSwapCardDetails;
        private ElectronicPaymentDetails moElectronicPaymentDetails;
        private StudentPayFeeDetails moStudentPayFeeDetails;
        private int miDepositedBankId;
        private bool mbCanSendSMS;
        private string msMobileNumber;
        private int miFeeDefaulterUserId;
        private string msDesignation;
        private string msRemarks;
        private int miSchoolId;
        private int miAcademicYearId;
        private int miStudentId;
        private int miUserId;
        private int miTotalAmount;
        private int miLastChequeBank;
        private DateTime mdtPaymentDate;
        
        private List<PayableForDetails> mlstIntervals;
        private List<PayableForDetails> mlstFeeDetails;               

        #endregion

        #region Constructors

        public StudentFeeDetailsDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUserId"></param>
        public StudentFeeDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miStudentId = aiStudentId;
            this.miUserId = aiUserId;
        }

        public StudentFeeDetailsDC(int miSchoolwiseStudentFeeId)
        {
            LoadStudentFeeDetailsDetails(miSchoolwiseStudentFeeId);
        }

        public StudentFeeDetailsDC(int aiStudentFeeId, bool abIsInternalFee)
        {
            if (!abIsInternalFee)
                LoadStudentFeeDetailsDetails(aiStudentFeeId);
            else
                LoadInternalFeeDetails(aiStudentFeeId);
        }

        #endregion

        #region Properties

        public List<StudentPayFeeDetails> StudentPayFeeDetails
        {
            get { return mlstStudentPayFeeDetails; }
            set { mlstStudentPayFeeDetails = value; }
        }

        public List<ChequeDetails> ChequeDetails
        {
            get { return mlstChequeDetails; }
            set { mlstChequeDetails = value; }
        }

        public EditFeeDetails EditFeeDetails
        {
            get { return moEditFeeDetails; }
            set { moEditFeeDetails = value; }
        }

        public SwapCardDetails SwapCardDetails
        {
            get { return moSwapCardDetails; }
            set { moSwapCardDetails = value; }
        }

        public ElectronicPaymentDetails ElectronicPaymentDetails
        {
            get { return moElectronicPaymentDetails; }
            set { moElectronicPaymentDetails = value; }
        }

        public StudentPayFeeDetails StudentPayFeeDetail
        {
            get { return moStudentPayFeeDetails; }
            set { moStudentPayFeeDetails = value; }
        }

        public int DepositedBankId
        {
            get { return miDepositedBankId; }
            set { miDepositedBankId = value; }
        }

        public string sRemarks
        {
            get { return msRemarks; }
            set { msRemarks = value; }
        }

        public StudentFeeDetailsStruct StudentFeeDetailsStructDetails
        {
            get { return moStudentFeeDetailsStruct; }
            set { moStudentFeeDetailsStruct = value; }
        }

        public bool CanSendSMS
        {
            get { return mbCanSendSMS; }
            set { mbCanSendSMS = value; }
        }

        public string MobileNumber
        {
            get { return msMobileNumber; }
            set { msMobileNumber = value; }
        }

        public int FeeDefaulterUserId
        {
            get { return miFeeDefaulterUserId; }
            set { miFeeDefaulterUserId = value; }
        }

        public string Designation
        {
            get { return msDesignation; }
            set { msDesignation = value; }
        }

        public DateTime PaymentDate
        {
            get { return mdtPaymentDate; }
            set { mdtPaymentDate = value; }
        }

        public int TotalAmount
        {
            get { return miTotalAmount; }
            set { miTotalAmount = value; }
        }

        public int LastChequeBank
        {
            get { return miLastChequeBank; }
            set { miLastChequeBank = value; }
        }

        public List<PayableForDetails> Intervals
        {
            get { return mlstIntervals; }
        }

        public List<PayableForDetails> FeeDetails
        {
            get { return mlstFeeDetails; }
        }       

        #endregion

        #region Public Methods


        private void LoadInternalFeeDetails(int aiInternalFeeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {   
                oSQLServerDbUtility.AddParameter("InternalFeeId", aiInternalFeeId, SqlDbType.Int);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetInternalFeeDetailsForStudent"))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["InternalFeeDetailsId"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miSchoolwiseStudentFeeId = Convert.ToInt32(oDR["InternalFeeDetailsId"]);

                            if (oDR["Yearwise_Student_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStudentId = Convert.ToInt32(oDR["Yearwise_Student_Id"]);

                            if (oDR["Payable_For"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msPayableFor = Convert.ToString(oDR["Payable_For"]);

                            if (oDR["Standard_Div_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStandardDivId = Convert.ToInt32(oDR["Standard_Div_Id"]);

                            if (oDR["Std_FeeType_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStdFeeTypeId = Convert.ToInt32(oDR["Std_FeeType_Id"]);

                            if (oDR["Amount"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miAmount = Convert.ToInt32(oDR["Amount"]);

                            if (oDR["Debit/Credit"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msDebitOrCredit = Convert.ToString(oDR["Debit/Credit"]);

                            if (oDR["PaidDate"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mdtPaidDate = Convert.ToDateTime(oDR["PaidDate"]);

                            if (oDR["ReceiptNo"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msReceiptNumber = Convert.ToString(oDR["ReceiptNo"]);

                            if (oDR["Remark"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msRemarks = Convert.ToString(oDR["Remark"]);

                            if (oDR["FeeDetailsID"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStudentFeeId = Convert.ToInt32(oDR["FeeDetailsID"]);

                            if (oDR["SchoolId"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miSchoolId = Convert.ToInt32(oDR["SchoolId"]);

                            if (oDR["AcademicYearId"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miAcademicYearId = Convert.ToInt32(oDR["AcademicYearId"]);

                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);

                            if (oDR["InsertDate"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["InsertDate"]);

                            if (oDR["InsertedByid"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miInsertedByid = Convert.ToInt32(oDR["InsertedByid"]);

                            if (oDR["UpdateDate"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["UpdateDate"]);

                            if (oDR["UpdatedById"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["UpdatedById"]);

                            if (oDR["Fee_Type"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msFeeType = Convert.ToString(oDR["Fee_Type"]);

                            if (oDR["Serial_Number"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miSerialNumber = Convert.ToInt32(oDR["Serial_Number"]);

                            if (oDR["AccountHeaderId"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miAccountHeaderId = Convert.ToInt32(oDR["AccountHeaderId"]);

                            if (oDR["IsConsiderForOnlinePayment"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment = Convert.ToBoolean(oDR["IsConsiderForOnlinePayment"]);
                        }

                    }
                }
            }
        }


        /// <summary>
        /// This function is used to insert the SchoolwiseStudentFeeDetails Details from student payables screen.
        /// </summary>
        public void InsertStudentFeeDetails()
        {
            StringBuilder sQuery = new StringBuilder();

            DataTable oDT = GetAcademicYearDetails(moStudentFeeDetailsStruct.miSchoolId);
            DataRow[] dr = oDT.Select("Academic_Year_ID=" + moStudentFeeDetailsStruct.miAcademicYearId);
            StudentDC oStudentDC = new StudentDC();

            int iChallanNo = 0;
            if (moStudentFeeDetailsStruct.miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                iChallanNo = oStudentDC.GetNextChallanNo(moStudentFeeDetailsStruct.miSchoolId, moStudentFeeDetailsStruct.miStudentId);
            }
            
            if (moStudentFeeDetailsStruct.miStdFeeTypeId == 0)
                sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");
            else
                sQuery.Append("DECLARE @iSerialNo INT ");

            sQuery.Append(" INSERT INTO " + " Schoolwise_Student_Fee_Details(" + "Student_Id" + ",Payable_For" + ",Std_FeeType_Id" + ",Amount" + ",[Debit/Credit]" + ",Paid_Date" + ",Remarks" + ",Student_Fee_Id" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type" + ",Serial_Number " + ",Standard_Div_Id, ChallanNo ,IntervalStartDate, IntervalEndDate, AccountHeaderId" + ")VALUES(" + " " + moStudentFeeDetailsStruct.miStudentId + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miStdFeeTypeId + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miStudentFeeId + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ," + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + moStudentFeeDetailsStruct.miStandardDivId + "," + iChallanNo + ",N'" + Convert.ToDateTime(dr[0]["start_date"]).ToString("MM-dd-yyyy") + "',N'" + Convert.ToDateTime(dr[0]["end_date"]).ToString("MM-dd-yyyy") + "', " + moStudentFeeDetailsStruct.miAccountHeaderId + ")");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This method is sued to return academic year data.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        private DataTable GetAcademicYearDetails(int aiSchoolId)
        {
            SchoolWiseAcademicYearMasterDC oSchoolWiseAcademicYearMasterDC = new SchoolWiseAcademicYearMasterDC();
            return oSchoolWiseAcademicYearMasterDC.GetAllSchoolwiseAcademicYearInfo(aiSchoolId);
        }

        /// <summary>
        /// This function is used to insert the SchoolwiseStudentFeeDetails Details from student payables screen for selected standard.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        public void InsertStudentFeeDetails(ArrayList oarrStdDivIdLst)
        {
            StringBuilder sQuery = new StringBuilder();

            DataTable oDT = GetAcademicYearDetails(moStudentFeeDetailsStruct.miSchoolId);
            DataRow[] dr = oDT.Select("Academic_Year_ID=" + moStudentFeeDetailsStruct.miAcademicYearId);

            sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");
            for (int iRowCnt = 0; iRowCnt < oarrStdDivIdLst.Count; iRowCnt++)
            {
                int iStdDivId = Convert.ToInt32(oarrStdDivIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " Schoolwise_Student_Fee_Details(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Student_Id " + ",Serial_Number " + ", IntervalStartDate, IntervalEndDate, AccountHeaderId) SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " ,  Standard_Div_Id " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , Schoolwise_Student_Fee_Details.Student_Id " + " , @iSerialNo " + ",N'" + Convert.ToDateTime(dr[0]["start_date"]).ToString("MM-dd-yyyy") + "',N'" + Convert.ToDateTime(dr[0]["end_date"]).ToString("MM-dd-yyyy") + "'," + moStudentFeeDetailsStruct.miAccountHeaderId + " FROM Schoolwise_Student_Fee_Details INNER JOIN YearWise_Student_Details YSD ON Schoolwise_Student_Fee_Details.Student_Id=YSD.YearWise_Student_Id INNER JOIN SchoolWise_Student_Master SSM ON YSD.Student_Id=SSM.SchoolWise_Student_Id " + " WHERE Schoolwise_Student_Fee_Details.Standard_Div_Id = " + iStdDivId + " AND SSM.SchoolLeft_Date IS NULL");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append(" AND YSD.Is_RTE_Student=0");
           }
            sQuery.Append("    INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Serial_Number " + ",IsConsiderForRTEStudent" + ",AccountHeaderId" + ",IsDueDateApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + ", " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + ", " + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + ",1)");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This function is used to insert the SchoolwiseStudentFeeDetails Details from student payables screen for selected standard-division.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
        public void InsertStudentFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId)
        {
            StringBuilder sQuery = new StringBuilder();

            DataTable oDT = GetAcademicYearDetails(moStudentFeeDetailsStruct.miSchoolId);
            DataRow[] dr = oDT.Select("Academic_Year_ID=" + moStudentFeeDetailsStruct.miAcademicYearId);

            sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");
            for (int iRowCnt = 0; iRowCnt < oarrStdDivIdLst.Count; iRowCnt++)
            {
                int iStdDivId = Convert.ToInt32(oarrStdDivIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " Schoolwise_Student_Fee_Details(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Student_Id " + ",Serial_Number " + ",Std_FeeType_Id, IntervalStartDate, IntervalEndDate, AccountHeaderId" + ") SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " ,  Standard_Div_Id " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ," + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , Schoolwise_Student_Fee_Details.Student_Id " + " , @iSerialNo " + ", " + moStudentFeeDetailsStruct.miStdFeeTypeId + ",N'" + Convert.ToDateTime(dr[0]["start_date"]).ToString("MM-dd-yyyy") + "',N'" + Convert.ToDateTime(dr[0]["End_date"]).ToString("MM-dd-yyyy") + "', " + moStudentFeeDetailsStruct.miAccountHeaderId + "  FROM Schoolwise_Student_Fee_Details INNER JOIN YearWise_Student_Details YSD on Schoolwise_Student_Fee_Details.Student_Id=YSD.YearWise_Student_Id INNER JOIN SchoolWise_Student_Master SSM on YSD.Student_Id = SSM.SchoolWise_Student_Id " + " WHERE Standard_Div_Id = " + iStdDivId + " AND Schoolwise_Student_Fee_Details.Is_Deleted = 'N'" + " AND SSM.SchoolLeft_Date IS NULL");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append("  AND YSD.Is_RTE_Student = 0 ");
            }

            sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Serial_Number " + ", Standard_Id " + ", IsConsiderForRTEStudent" + ",AccountHeaderId" + ",IsDueDateApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + aiStandardId + "," + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + ", 1)");
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This method is used to insert studentfeedetails records for standard-division.
        /// </summary>
        /// <param name="oarrStdIdLst"></param>
        public void CopyStudentFeeDetails(ArrayList oarrStdIdLst)
        {
            StringBuilder sQuery = new StringBuilder();
            DataTable oDT = GetAcademicYearDetails(moStudentFeeDetailsStruct.miSchoolId);
            DataRow[] dr = oDT.Select("Academic_Year_ID=" + moStudentFeeDetailsStruct.miAcademicYearId);
            sQuery.Append(" DECLARE @iSerialNo INT ");
            for (int iRowCnt = 0; iRowCnt < oarrStdIdLst.Count; iRowCnt++)
            {
                sQuery.Append(" SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");

                int iStdId = Convert.ToInt32(oarrStdIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " Schoolwise_Student_Fee_Details(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Student_Id " + ",Serial_Number " + ",Std_FeeType_Id" + ", IntervalStartDate, IntervalEndDate, AccountHeaderId) SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " ,  Standard_Div_Id " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , Schoolwise_Student_Fee_Details.Student_Id " + " , @iSerialNo " + ", " + moStudentFeeDetailsStruct.miStdFeeTypeId + ",N'" + Convert.ToDateTime(dr[0]["start_date"]).ToString("MM-dd-yyyy") + "',N'" + Convert.ToDateTime(dr[0]["End_date"]).ToString("MM-dd-yyyy") + "','" + moStudentFeeDetailsStruct.miAccountHeaderId + "' FROM         YearWise_Student_Details INNER JOIN " + " Schoolwise_Student_Fee_Details ON YearWise_Student_Details.YearWise_Student_Id = Schoolwise_Student_Fee_Details.Student_Id AND " + " YearWise_Student_Details.School_Id = Schoolwise_Student_Fee_Details.School_Id AND " + " YearWise_Student_Details.Academic_Year_ID = Schoolwise_Student_Fee_Details.Academic_Year_Id " + " INNER JOIN SchoolWise_Student_Master SSM on YearWise_Student_Details.Student_Id = SSM.SchoolWise_Student_Id WHERE     (YearWise_Student_Details.Standard_Id = " + iStdId + ")" + " AND SSM.SchoolLeft_Date IS NULL");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append(" AND YearWise_Student_Details.Is_RTE_Student=0 ");
                sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Serial_Number " + ", Standard_Id " + ", IsConsiderForRTEStudent" + ",AccountHeaderId" + ",IsDueDateApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + iStdId + " , " + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + ",1)");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This method is used to insert studentfeedetails records for standard-division.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        public void InsertStudentFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId, int aiDivisionId)
        {
            StringBuilder sQuery = new StringBuilder();            
            DataTable oDT = GetAcademicYearDetails(moStudentFeeDetailsStruct.miSchoolId);
            DataRow[] dr = oDT.Select("Academic_Year_ID=" + moStudentFeeDetailsStruct.miAcademicYearId);

            sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");
            for (int iRowCnt = 0; iRowCnt < oarrStdDivIdLst.Count; iRowCnt++)
            {
                int iStdDivId = Convert.ToInt32(oarrStdDivIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " Schoolwise_Student_Fee_Details(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Student_Id " + ",Serial_Number " + ",Std_FeeType_Id" + ", IntervalStartDate, IntervalEndDate, AccountHeaderId) SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , Standard_Div_Id  " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ," + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , Schoolwise_Student_Fee_Details.Student_Id " + " , @iSerialNo " + " , " + moStudentFeeDetailsStruct.miStdFeeTypeId + ",N'" + Convert.ToDateTime(dr[0]["Start_Date"]).ToString("MM-dd-yyyy") + "',N'" + Convert.ToDateTime(dr[0]["End_Date"]).ToString("MM-dd-yyyy") + "'," + moStudentFeeDetailsStruct.miAccountHeaderId + " FROM Schoolwise_Student_Fee_Details INNER JOIN YearWise_Student_Details YSD ON Schoolwise_Student_Fee_Details.Student_Id = YSD.YearWise_Student_Id INNER JOIN SchoolWise_Student_Master SSM on YSD.Student_Id=SSM.SchoolWise_Student_Id " + " WHERE Standard_Div_Id = " + iStdDivId + " AND Schoolwise_Student_Fee_Details.Is_Deleted='N' " + " AND SSM.SchoolLeft_Date IS NULL");
                if(!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                sQuery.Append(" AND YSD.Is_RTE_Student=0 ");
            }

            sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Serial_Number " + " , Standard_Id " + " , Division_Id " + " , IsConsiderForRTEStudent" + ", AccountHeaderId" + ",IsDueDateApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ," + "dbo.getlocaldate(default) " + ",  N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + aiStandardId + " , " + aiDivisionId + "," + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + ",1)");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        ///  This method is used to insert internal studentfeedetails records for standard-division from student payables screen
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        public void InsertStudentInternalFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId, int aiDivisionId)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");

            sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Serial_Number " + " , Standard_Id " + " , Division_Id " + " , IsInternalFee " + " , IsConsiderForRTEStudent" + ", AccountHeaderId" + ",IsDueDateApplicable" + ",IsOnlinePaymentApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + aiStandardId + " , " + aiDivisionId + " , 1 " + " , " + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + "," + (moStudentFeeDetailsStruct.mbIsDueDateApplicable ? 1 : 0) + "," + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + ")");

            sQuery.Append("DECLARE @iMasterID INT " + "SELECT  @iMasterID = SCOPE_IDENTITY()");

            for (int iRowCnt = 0; iRowCnt < oarrStdDivIdLst.Count; iRowCnt++)
            {
                int iStdDivId = Convert.ToInt32(oarrStdDivIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " InternalFeeDetails (" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",PaidDate" + ",Remark" + ",SchoolId" + ",AcademicYearId" + ",InsertedByid" + ",InsertDate" + ",Fee_Type " + ",InternalFeeMasterID " + ", ReceiptNo " + ", UpdatedById " + ", Schoolwise_Student_Id, IsChequeBounced, IsConsiderForOnlinePayment " + ") SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id  " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iMasterID " + " , 0 " + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ,  YearWise_Student_Details.Student_Id, 'N', " + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + " FROM         YearWise_Student_Details INNER JOIN " + " SchoolWise_Standard_Division_Master ON YearWise_Student_Details.Division_id = SchoolWise_Standard_Division_Master.Division_Id AND " + " YearWise_Student_Details.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id INNER JOIN " + " SchoolWise_Student_Master ON YearWise_Student_Details.Student_Id = SchoolWise_Student_Master.SchoolWise_Student_Id " + " WHERE     (SchoolWise_Student_Master.Is_Deleted = 'N') " + " AND SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = " + iStdDivId + "AND (YearWise_Student_Details.Is_Deleted='N')" + " AND (SchoolWise_Student_Master.SchoolLeft_Date IS NULL) ");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append(" AND(YearWise_Student_Details.Is_RTE_Student = 0) ");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This method is used to insert internal studentfeedetails records for standard from CopyFeeConfigurationPopup.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        /// <param name="aiStandardId"></param>
        public void InsertStudentInternalFeeDetails(ArrayList oarrStdDivIdLst, int aiStandardId)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");

            sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" + ",Fee_Type " + ",Serial_Number " + ", Standard_Id " + " , IsInternalFee " + ", IsConsiderForRTEStudent" + ", AccountHeaderId" + ",IsDueDateApplicable" + ", IsOnlinePaymentApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + aiStandardId + " , 1 " + ", " + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + "," + (moStudentFeeDetailsStruct.mbIsDueDateApplicable ? 1 : 0) + "," + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + ")");
            
            sQuery.Append("DECLARE @iMasterID INT " + "SELECT  @iMasterID = SCOPE_IDENTITY()");

            for (int iRowCnt = 0; iRowCnt < oarrStdDivIdLst.Count; iRowCnt++)
            {
                int iStdDivId = Convert.ToInt32(oarrStdDivIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " InternalFeeDetails(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",PaidDate" + ",Remark" + ",SchoolId" + ",AcademicYearId" + " ,InsertedByid" + ",InsertDate" + ",Fee_Type " + ",InternalFeeMasterID " + ", ReceiptNo " + ", UpdatedById " + " , Schoolwise_Student_Id, IsChequeBounced,IsConsiderForOnlinePayment " + ") SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " ,  SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iMasterID " + " , 0 " + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ,  YearWise_Student_Details.Student_Id, 'N', " + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + " FROM         YearWise_Student_Details INNER JOIN " + " SchoolWise_Standard_Division_Master ON YearWise_Student_Details.Division_id = SchoolWise_Standard_Division_Master.Division_Id AND " + " YearWise_Student_Details.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id INNER JOIN " + " SchoolWise_Student_Master ON YearWise_Student_Details.Student_Id = SchoolWise_Student_Master.SchoolWise_Student_Id " + " WHERE      (SchoolWise_Student_Master.Is_Deleted = 'N') " + " AND SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = " + iStdDivId + " AND (YearWise_Student_Details.Is_Deleted='N')" + " AND (SchoolWise_Student_Master.SchoolLeft_Date IS NULL)");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append(" AND (YearWise_Student_Details.Is_RTE_Student=0) ");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This method is used to insert internal studentfeedetails records for standard-division from CopyFeeConfigurationPopup.
        /// </summary>
        /// <param name="oarrStdIdLst"></param>
        public void CopyStudentInternalFeeDetails(ArrayList oarrStdIdLst)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(" DECLARE @iSerialNo INT ");
            sQuery.Append(" DECLARE @iMasterID INT ");
            for (int iRowCnt = 0; iRowCnt < oarrStdIdLst.Count; iRowCnt++)
            {
                sQuery.Append(" SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");
                int iStdId = Convert.ToInt32(oarrStdIdLst[iRowCnt].ToString());
                sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Fee_Type " + ",Serial_Number " + ", Standard_Id " + " , IsInternalFee " + ", IsConsiderForRTEStudent" + ", AccountHeaderId" + ",IsDueDateApplicable,IsOnlinePaymentApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , '" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , " + iStdId + " , 1 " + ", " + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + "," + (moStudentFeeDetailsStruct.mbIsDueDateApplicable ? 1 : 0) + "," + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + ")");

                sQuery.Append(" SELECT  @iMasterID = SCOPE_IDENTITY() ");

                sQuery.Append("          INSERT " + " INTO " + " InternalFeeDetails(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",PaidDate" + ",Remark" + ",SchoolId" + ",AcademicYearId" + ",InsertedByid" + ",Fee_Type " + ",InternalFeeMasterID " + ", ReceiptNo " + ", UpdatedById " + " , Schoolwise_Student_Id " + ",IsChequeBounced,IsConsiderForOnlinePayment) SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " ,  SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iMasterID " + " , 0 " + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ,  YearWise_Student_Details.Student_Id,'N',"+(moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment? 1 : 0)+ " FROM         YearWise_Student_Details INNER JOIN " + " SchoolWise_Standard_Division_Master ON YearWise_Student_Details.Division_id = SchoolWise_Standard_Division_Master.Division_Id AND " + " YearWise_Student_Details.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id INNER JOIN " + " SchoolWise_Student_Master ON YearWise_Student_Details.Student_Id = SchoolWise_Student_Master.SchoolWise_Student_Id " + " WHERE  (SchoolWise_Student_Master.Is_Deleted = 'N') " + " AND SchoolWise_Standard_Division_Master.Standard_Id = " + iStdId + "AND (SchoolWise_Student_Master.SchoolLeft_Date IS NULL)");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append(" AND (YearWise_Student_Details.Is_RTE_Student=0)");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

        /// <summary>
        /// This method is used to insert internal studentfeedetails records for standard-division.
        /// </summary>
        /// <param name="oarrStdDivIdLst"></param>
        public void InsertStudentInternalFeeDetails(ArrayList oarrStdDivIdLst)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("DECLARE @iSerialNo INT " + "SELECT  @iSerialNo = " + " dbo.UDF_NextSerialNo(" + moStudentFeeDetailsStruct.miSchoolId + ")");

            sQuery.Append("     INSERT " + " INTO  " + " Schoolwise_Debit_Entry_Log(" + "Payable_For" + ",Amount" + ",Paid_Date" + ",Remarks" + ",School_Id" + ",Academic_Year_Id" + ",Inserted_By_id" + ",Insert_Date" +",Fee_Type " + ",Serial_Number " + " , IsInternalFee " + ", IsConsiderForRTEStudent" + ", AccountHeaderId" + ", IsDueDateApplicable" + ", IsOnlinePaymentApplicable)VALUES(" + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " , " + moStudentFeeDetailsStruct.miAmount + " , '" + moStudentFeeDetailsStruct.mdtPaidDate + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ," + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iSerialNo " + " , 1 " + ", " + moStudentFeeDetailsStruct.mbIncludeRTEStudent.ToInt() + "," + moStudentFeeDetailsStruct.miAccountHeaderId + "," + (moStudentFeeDetailsStruct.mbIsDueDateApplicable ? 1 : 0) + "," + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + ")");

            sQuery.Append("DECLARE @iMasterID INT " + "SELECT  @iMasterID = SCOPE_IDENTITY()");            

            for (int iRowCnt = 0; iRowCnt < oarrStdDivIdLst.Count; iRowCnt++)
            {
                int iStdDivId = Convert.ToInt32(oarrStdDivIdLst[iRowCnt].ToString());
                sQuery.Append("          INSERT " + " INTO " + " InternalFeeDetails(" + "Payable_For" + ",Standard_Div_Id" + ",Amount" + ",[Debit/Credit]" + ",PaidDate" + ",Remark" + ",SchoolId" + ",AcademicYearId" + ",InsertedByid" + ",InsertDate" + ", Fee_Type " + ",InternalFeeMasterID " + ", ReceiptNo " + ", UpdatedById " + ", Schoolwise_Student_Id, IsChequeBounced, IsConsiderForOnlinePayment" + ") SELECT DISTINCT " + " N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + " ,  SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id " + " , " + moStudentFeeDetailsStruct.miAmount + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + " , N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + " , N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "' " + " , " + moStudentFeeDetailsStruct.miSchoolId + " , " + moStudentFeeDetailsStruct.miAcademicYearId + " , " + moStudentFeeDetailsStruct.miInsertedByid + " , " + "dbo.getlocaldate(default) " + ", N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + " , @iMasterID " + " , 0 " + " , " + moStudentFeeDetailsStruct.miInsertedByid + " ,  YearWise_Student_Details.Student_Id, 'N', " + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + " FROM         YearWise_Student_Details INNER JOIN " + " SchoolWise_Standard_Division_Master ON YearWise_Student_Details.Division_id = SchoolWise_Standard_Division_Master.Division_Id AND " + " YearWise_Student_Details.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id INNER JOIN " + " SchoolWise_Student_Master ON YearWise_Student_Details.Student_Id = SchoolWise_Student_Master.SchoolWise_Student_Id " + " WHERE     (SchoolWise_Student_Master.Is_Deleted = 'N') " + " AND SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = " + iStdDivId + " AND (SchoolWise_Student_Master.SchoolLeft_Date IS NULL)");
                if (!moStudentFeeDetailsStruct.mbIncludeRTEStudent)
                    sQuery.Append(" AND (YearWise_Student_Details.Is_RTE_Student=0)");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sQuery.ToString());
        }

   /// <summary>
   /// This method is used to insert internal fees details for specific student.
   /// </summary>
   /// <param name="aiStudentId"></param>
   /// <param name="aiPaymentTypeId"></param>
        public void InsertStudentInternalFeeDetails(int aiStudentId, int aiPaymentTypeId, int aiIsNewEntry, int asPdcId)
    {

        using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        {
            oSQLServerDbUtility.AddParameter("SchoolId", moStudentFeeDetailsStruct.miSchoolId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("AcademicYearId", moStudentFeeDetailsStruct.miAcademicYearId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("PayableFor", moStudentFeeDetailsStruct.msPayableFor, SqlDbType.NVarChar);
            oSQLServerDbUtility.AddParameter("Amount", moStudentFeeDetailsStruct.miAmount, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("PaidDate", moStudentFeeDetailsStruct.mdtPaidDate, SqlDbType.DateTime);
            oSQLServerDbUtility.AddParameter("Remark", moStudentFeeDetailsStruct.msRemarks, SqlDbType.NVarChar);
            oSQLServerDbUtility.AddParameter("FeeType", moStudentFeeDetailsStruct.msFeeType, SqlDbType.NVarChar);
            oSQLServerDbUtility.AddParameter("DebitCredit", moStudentFeeDetailsStruct.msDebitOrCredit, SqlDbType.NVarChar);
            oSQLServerDbUtility.AddParameter("PaymentTypeId", aiPaymentTypeId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("IsNewEntry", aiIsNewEntry, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("AccountHeaderId", moStudentFeeDetailsStruct.miAccountHeaderId, SqlDbType.Int);
            oSQLServerDbUtility.AddParameter("ConsiderForOnlinePayment", moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment, SqlDbType.Bit);
            if (asPdcId != 0)
            oSQLServerDbUtility.AddParameter("PdcId", asPdcId, SqlDbType.Int);

            oSQLServerDbUtility.AddParameter("UserId", moStudentFeeDetailsStruct.miUpdatedById, SqlDbType.Int);

            oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertInternalFeeDetailsForStudent");
        }
    
    }
        /// <summary>
        /// This function is used to update the SchoolwiseStudentFeeDetails Details at the school level from student payable screen.
        /// </summary>
        public void UpdateStudentFeeDetails()
        {
            string sUpdateStatement = " UPDATE " + " Schoolwise_Student_Fee_Details " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",[Debit/Credit]= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + ",Paid_Date= N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remarks= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ",Updated_By_Id= " + moStudentFeeDetailsStruct.miUpdatedById + " , Update_Date = dbo.GetLocalDate(DEFAULT) "+ " , AccountHeaderId = "+ moStudentFeeDetailsStruct.miAccountHeaderId + " WHERE Serial_Number = " + moStudentFeeDetailsStruct.miSerialNumber;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This function is used to update the SchoolwiseStudentFeeDetails Details for the selected debit entry from student payables screen.
        /// </summary>
        /// <param name="aiDebitId"></param>
        public void UpdateStudentFeeDetails(int aiDebitId)
        {
            string sUpdateStatement = " UPDATE " + " Schoolwise_Student_Fee_Details " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",[Debit/Credit]= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + ",Paid_Date= N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remarks= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ",Updated_By_Id= " + moStudentFeeDetailsStruct.miUpdatedById + " , Update_Date = dbo.GetLocalDate(DEFAULT) "+ " , AccountHeaderId = "+ moStudentFeeDetailsStruct.miAccountHeaderId  + " WHERE Schoolwise_Student_Fee_Id = " + aiDebitId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void UpdateStudentInternalFeeDetails(int aiDebitId)
        {
            string sUpdateStatement = " UPDATE " + " InternalFeeDetails " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",[Debit/Credit]= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + ",PaidDate= N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remark= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ",UpdatedById= " + moStudentFeeDetailsStruct.miUpdatedById + ", IsConsiderForOnlinePayment= " + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + " , UpdateDate = dbo.GetLocalDate(DEFAULT) " + " WHERE InternalFeeDetailsId = " + aiDebitId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This function is used update extra added fee details from selected standard division from student payables screen.
        /// </summary>
        /// <param name="oarrStdDivLst"></param>
        /// <param name="asIsUpdate"></param>
        public void UpdateStudentFeeDetails(ArrayList oarrStdDivLst, string asIsUpdate)
        {
            ArrayList oarrInsertQuery = new ArrayList();
            string sUpdateStatement;
            for (int iRowCnt = 0; iRowCnt < oarrStdDivLst.Count; iRowCnt++)
            {
                sUpdateStatement = " UPDATE " + " Schoolwise_Student_Fee_Details " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",[Debit/Credit]= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + ",Paid_Date= '" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remarks= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ",Updated_By_Id= " + moStudentFeeDetailsStruct.miUpdatedById + " , Update_Date = dbo.GetLocalDate(DEFAULT)" + " , AccountHeaderId = " + moStudentFeeDetailsStruct.miAccountHeaderId + " FROM Schoolwise_Student_Fee_Details INNER JOIN YearWise_Student_Details YSD ON Schoolwise_Student_Fee_Details.Student_Id = YSD.YearWise_Student_Id INNER JOIN SchoolWise_Student_Master SSM ON YSD.Student_Id=SSM.SchoolWise_Student_Id WHERE " + " Serial_Number=" + moStudentFeeDetailsStruct.miSerialNumber + " AND Standard_Div_Id = " + Convert.ToInt32(oarrStdDivLst[iRowCnt].ToString());
                //if (moStudentFeeDetailsStruct.msIncludeLeftStudent == false)
                sUpdateStatement = sUpdateStatement + " AND SSM.SchoolLeft_Date IS NULL";
                oarrInsertQuery.Add(sUpdateStatement);
            }

            if (asIsUpdate == "true")
            {
                sUpdateStatement = " UPDATE " + " Schoolwise_Debit_Entry_Log " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",Paid_Date= '" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remarks= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ",Updated_By_Id= " + moStudentFeeDetailsStruct.miUpdatedById + " , Update_Date = dbo.GetLocalDate(DEFAULT) " + " , AccountHeaderId = " + moStudentFeeDetailsStruct.miAccountHeaderId + " WHERE " + " Serial_Number=" + moStudentFeeDetailsStruct.miSerialNumber;
                oarrInsertQuery.Add(sUpdateStatement);
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])oarrInsertQuery.ToArray(typeof(string)));
        }

        /// <summary>
        /// This function is used to update internal fee details from student payables screen.
        /// </summary>
        /// <param name="oarrStdDivLst"></param>
        /// <param name="asIsUpdate"></param>
        public void UpdateStudentInternalFeeDetails(ArrayList oarrStdDivLst, string asIsUpdate)
        {
            ArrayList oarrInsertQuery = new ArrayList();
            string sUpdateStatement;
            for (int iRowCnt = 0; iRowCnt < oarrStdDivLst.Count; iRowCnt++)
            {
                sUpdateStatement = " UPDATE " + " InternalFeeDetails " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",[Debit/Credit]= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msDebitOrCredit, false) + "' " + ",PaidDate= N'" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remark= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ",UpdatedById= " + moStudentFeeDetailsStruct.miUpdatedById + " , UpdateDate = dbo.GetLocalDate(DEFAULT) " + " , IsConsiderForOnlinePayment = " + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + "FROM InternalFeeDetails INNER JOIN SchoolWise_Student_Master SSM ON InternalFeeDetails.Schoolwise_Student_Id=SSM.SchoolWise_Student_Id WHERE " + " InternalFeeMasterID = (SELECT Schoolwise_Debit__Entry_Id FROM Schoolwise_Debit_Entry_Log WHERE Serial_Number = " + moStudentFeeDetailsStruct.miSerialNumber + ") " + " AND Standard_Div_Id = " + Convert.ToInt32(oarrStdDivLst[iRowCnt].ToString());
                sUpdateStatement=sUpdateStatement + "AND SSM.SchoolLeft_Date IS NULL";
                oarrInsertQuery.Add(sUpdateStatement);
            }

            if (asIsUpdate == "true")
            {
                sUpdateStatement = " UPDATE " + " Schoolwise_Debit_Entry_Log " + " SET " + " Payable_For= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msPayableFor, false) + "' " + ",Fee_type= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msFeeType, false) + "' " + ",Amount= " + moStudentFeeDetailsStruct.miAmount + ",Paid_Date= '" + moStudentFeeDetailsStruct.mdtPaidDate.ToString("MM-dd-yyyy") + "' " + ",Remarks= N'" + StringUtility.ReplaceSingleQuoteInString(moStudentFeeDetailsStruct.msRemarks, false) + "'" + ", IsDueDateApplicable=" + (moStudentFeeDetailsStruct.mbIsDueDateApplicable ? Constants.S_ONE : Constants.S_ZERO) + ",Updated_By_Id= " + moStudentFeeDetailsStruct.miUpdatedById + " , Update_Date = dbo.GetLocalDate(DEFAULT) " + " , IsOnlinePaymentApplicable= " + (moStudentFeeDetailsStruct.mbIsConsiderForOnlinePayment ? 1 : 0) + " WHERE " + " Serial_Number=" + moStudentFeeDetailsStruct.miSerialNumber;
                oarrInsertQuery.Add(sUpdateStatement);
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])oarrInsertQuery.ToArray(typeof(string)));
        }

        /// <summary>
        /// This function is used to delete the standard level debit details added as extra fees from student payables screen.
        /// </summary>
        /// <param name="aiSerialNo"></param>
        /// <param name="oarrStdDivIdsList"></param>
        public void DeleteDebitFeeDetails(int aiSerialNo, ArrayList oarrStdDivIdsList ,int aiUpdatedByid, bool abIsInternalFee, int aiSchoolId, int aiAcademicYearId)
        {
            ArrayList oarrUpdateQuery = new ArrayList();
            string sDeleteStatement;

            if (abIsInternalFee)
            {
                string sStdDivId = string.Join(",", oarrStdDivIdsList.ToArray());
                sDeleteStatement = "UPDATE IFD SET Is_Deleted = 1,UpdateDate = DBO.GetLocalDate(DEFAULT),UpdatedById = "+aiUpdatedByid+" FROM InternalFeeDetails IFD INNER JOIN Schoolwise_Debit_Entry_Log SDE ON IFD.InternalFeeMasterID = SDE.Schoolwise_Debit__Entry_Id WHERE IFD.AcademicYearId = "+aiAcademicYearId+" AND IFD.SchoolId = "+ aiSchoolId+" and IFD.Is_Deleted = 0 AND SDE.Is_Deleted = 'N' AND SDE.Serial_Number = "+ aiSerialNo + "AND IFD.Standard_Div_Id IN ("+ sStdDivId + ")";
                oarrUpdateQuery.Add(sDeleteStatement);
            }
            else
            {
                for (int iRowCnt = 0; iRowCnt < oarrStdDivIdsList.Count; iRowCnt++)
                {
                    sDeleteStatement = " UPDATE " + " Schoolwise_Student_Fee_Details " + " SET " + " Is_Deleted =N'" + Constants.C_YES + "'" + " , Updated_By_Id= " + aiUpdatedByid + " ,Update_Date = dbo.GetLocalDate(DEFAULT)  WHERE " + " Serial_Number=" + aiSerialNo + " AND Standard_Div_Id = " + Convert.ToInt32(oarrStdDivIdsList[iRowCnt].ToString());
                    oarrUpdateQuery.Add(sDeleteStatement);
                }
            }

            sDeleteStatement = " UPDATE " + " Schoolwise_Debit_Entry_Log " + " SET " + " Is_Deleted =N'" + Constants.C_YES + "'" + " ,Updated_By_Id= " + aiUpdatedByid + ", Update_Date = dbo.GetLocalDate(DEFAULT)  WHERE " + " Serial_Number=" + aiSerialNo;
            oarrUpdateQuery.Add(sDeleteStatement);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])oarrUpdateQuery.ToArray(typeof(string)));
        }

        /// <summary>
        /// This function is used to delete the SchoolwiseStudentFeeDetails Details for bounced cheques.
        /// </summary>
        /// <param name="aiDebitId"></param>
        public void DeleteStudentBounceChequeFeeDetails(int aiDebitId, bool abIsInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iDebitId", aiDebitId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteBounceChequeDetails");
            }
        }

        /// <summary>
        /// This function is used to delete the SchoolwiseStudentFeeDetails Details from Student Payable screen.
        /// </summary>
        /// <param name="aiDebitId"></param>
        public void DeleteStudentFeeDetails(int aiDebitId, int aiUpdatedByid)
        {
            string sUpdateQuery = " UPDATE " + " Schoolwise_Student_Fee_Details " + " SET " + " Is_Deleted =N'" + Constants.C_YES + "'" + " , Updated_By_Id= " + aiUpdatedByid + ", Update_Date = dbo.GetLocalDate(DEFAULT)  WHERE " + " Schoolwise_Student_Fee_Id=" + aiDebitId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateQuery);
        }

        /// <summary>
        /// This function is used to delete the SchoolwiseStudentFeeDetails Details from Student Payable screen.
        /// </summary>
        /// <param name="aiDebitId"></param>
        public void DeleteStudentInternalFeeDetails(int aiDebitId,int aiUpdatedByid)
        {
            string sUpdateQuery = " UPDATE " + " InternalFeeDetails " + " SET " + " Is_Deleted =1 ,UpdatedById= " + aiUpdatedByid + ", UpdateDate = dbo.GetLocalDate(DEFAULT)  WHERE " + " InternalFeeDetailsId=" + aiDebitId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateQuery);
        }

        /// <summary>
        /// This function is used to permanent delete student's debit details from student payable screen.
        /// </summary>
        public void DeleteStudentDebitDetails()
        {
            string sDeleteStatement = "DELETE Schoolwise_Student_Fee_Details WHERE Schoolwise_Student_Fee_Id=N'" + moStudentFeeDetailsStruct.miSchoolwiseStudentFeeId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }


        public struct StudentFeeDetailsStruct
        {
            public int miSchoolwiseStudentFeeId;
            public int miStudentId;
            public string msPayableFor;
            public int miAccountHeaderId;
            public string msAccountHeaderName;
            public int miStandardDivId;
            public int miStdFeeTypeId;
            public int miAmount;
            public string msDebitOrCredit;
            public DateTime mdtPaidDate;
            public string msReceiptNumber;
            public string msRemarks;
            public int miStudentFeeId;
            public int miSchoolId;
            public int miAcademicYearId;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
            public int miSerialNumber;
            public string msFeeType;
            public bool mbIncludeRTEStudent;
            public Collection<StudentFeeDetailsDC> moStudentFeeDetailsDC;
            public bool mbIsDueDateApplicable;
            public bool mbIsConsiderForOnlinePayment;
        }

        /// <summary>
        /// This method is used to get debit details of a particular student by selecting a student from search grid of student payable screen.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataSet GetStudentDebitDetails(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Student_ID", aiStudentId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStudentDebitDetails");
            }
        }

        public DataSet GetInternalFeesChequeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetInternalFeesChequeDetails");
            }
        }



        /// <summary>
        /// This method is used to get debit details of a particular student from Student Payable screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="abIncludeInternalFee"></param>
        /// <returns></returns>
        public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, bool abIncludeInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeInternalFee", abIncludeInternalFee, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetDebitDetails");
            }
        }

        /// <summary>
        ///  This method is used to get debit details of a particular standard and is used on Student Payable screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="abIncludeInternalFee"></param>
        /// <returns></returns>
        public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, int aiStandardId, bool abIncludeInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardID", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeInternalFee", abIncludeInternalFee, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetDebitDetails");
            }
        }

        /// <summary>
        /// This method is used to get debit details of a particular standard and is used on Student Payable screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="abIncludeInternalFee"></param>
        /// <returns></returns>
        public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, int aiStandardId, int aiDivisionId, bool abIncludeInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardID", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionID", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeInternalFee", abIncludeInternalFee, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetDebitDetails");
            }
        }

        /// <summary>
        /// This procedure is used to get the correct payment mode for selected receipt number.
        /// </summary>
        /// <param name="asReceiptNumber"></param>
        /// <returns></returns>
        public Constants.FeePaymentType GetPaymentModeForReceipt(string asReceiptNumber, int aiAccountHeaderId)
        {
            string sPaymentMode = Constants.FeePaymentType.Cash.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ReceiptNumber", asReceiptNumber, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccountHeaderId", aiAccountHeaderId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeePaymentMode"))
                {
                    if (oSqlDataReader.Read())
                    {
                        PaymentDate = oSqlDataReader["PaymentDate"].ToDateTime();
                        sPaymentMode = oSqlDataReader["PaymentMode"].ToString();
                    }
                }
            }
            return (Constants.FeePaymentType)Enum.Parse(typeof(Constants.FeePaymentType), sPaymentMode);
        }

        /// <summary>
        /// This method is used to get payable amount.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public FeeSMS GetPayableAmount(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);          
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPayableFeeAmount"))
                {
                    FeeSMS oFeeSMS = new FeeSMS();
                    if(oSqlDataReader.Read())
                    {
                        oFeeSMS.PayableAmount = oSqlDataReader["PayableAmount"].ToInt();
                        oFeeSMS.Term = oSqlDataReader["TermName"].ToString();
                    }
                    return oFeeSMS;
                }
            }
        }
        
         /// <summary>
        /// This method is used to get yearwise student Id by user id.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public int GetYearwiseStudentId(int aiUserId, int aiSchoolId, int aiAcademicYearId)
        {
            string sFetchStatement = string.Format("SELECT dbo.YearWise_Student_Details.YearWise_Student_Id FROM  dbo.YearWise_Student_Details INNER JOIN  dbo.SchoolWise_Student_Master ON dbo.YearWise_Student_Details.Student_Id = dbo.SchoolWise_Student_Master.SchoolWise_Student_Id WHERE (dbo.SchoolWise_Student_Master.User_Id = {0}) AND (dbo.SchoolWise_Student_Master.Is_Deleted = 'N') AND (dbo.YearWise_Student_Details.School_Id = {1}) AND (dbo.YearWise_Student_Details.Academic_Year_ID = {2})",aiUserId,aiSchoolId,aiAcademicYearId);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sFetchStatement).ToInt();
        }

        public void ResetReceiptNumber(int aiSchoolId, int aiAcademicYearId, int aiAccountHeaderId, DateTime Date, int aiOrderById, int aiIsInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AccountHeaderId", aiAccountHeaderId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("OrderById", aiOrderById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", aiIsInternalFee, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ResetReceiptForm");
            }
        }

        public DataTable GetAllAccountHeaderCombo(int aiSchoolId, int aiAcademicYearId, int aiOriginalFeeTypeId, int aiIsSchoolFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OriginalFeeTypeId", aiOriginalFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSchoolFee", aiIsSchoolFee, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAccountHeaderName");
            }
        }

		public DataTable GetReceiptDetailsForStudent(int aiSchoolId, int aiAcademicYearId, string sReceiptNo, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiptNo", sReceiptNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetReceiptDetailsForStudent");
            }
        }
		
        /// <summary>
        /// This method is used to get student fee details for adding concession for staff kid and RTE students.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aodtCurrentDate"></param>
        /// <param name="abShowOnlyDebits"></param>
        /// <returns></returns>
        public DataSet GetStudentFeeDetails(int aiStudentId, DateTime adtCurrentDate, int aiLoginUserRoleId, bool abShowOnlyDebits = false)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CurrentDate", adtCurrentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ShowOnlyDebits", abShowOnlyDebits, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("LoginUserRoleId", aiLoginUserRoleId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetStudentFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to get all student id's.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<int> GetAllStudentId(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolwiseStudentFeeId"))
                    return this.ReadAllStudentId(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to add concession for internal fee of RTE Student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public void InternalFeeConcessionForRTEStudent(int aiStudentId, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AddConcessionForInternalFee");
            }
        }
        
        /// <summary>
        /// This method is used to read all student id's.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        public List<int> ReadAllStudentId(SqlDataReader aoSqlDataReader)
        {
            List<int> lstStudentId = new List<int>();
            if (aoSqlDataReader != null)
            {
                while (aoSqlDataReader.Read())
                {
                    int iStudentId=0;
                    if (aoSqlDataReader["Schoolwise_Student_Fee_Id"] != DBNull.Value)
                        iStudentId = Convert.ToInt32(aoSqlDataReader["Schoolwise_Student_Fee_Id"]);                    
                    lstStudentId.Add(iStudentId);
                }
                aoSqlDataReader.Close();
            }
            return lstStudentId;
        }

        /// <summary>
        /// This method is used to get student fee details.
        /// </summary>
        /// <param name="aiPaymentType"></param>
        /// <param name="abShowOnlyDebits"></param>
        /// <param name="aiReceiptNumber"></param>
        /// <returns></returns>
        public List<StudentPaidFeeDetails> GetStudentFeeDetails(DateTime adtCurrentDate, int aiPaymentType, bool abShowOnlyDebits = false, int aiReceiptNumber = 0)
        {
            List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = new List<StudentPaidFeeDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CurrentDate", adtCurrentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ShowOnlyDebits", abShowOnlyDebits, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ReceiptNumber", aiReceiptNumber, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentType", aiPaymentType, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeDetailsOfStudent"))
                {
                    lstStudentPaidFeeDetails = FillStudentPayFeeDetails(oSqlDataReader);
                    FillEditedFeeDetails(oSqlDataReader, aiReceiptNumber);

                    if (aiReceiptNumber != Constants.I_ZERO && (aiPaymentType == Constants.FeePaymentType.PDC.ToInt() || aiPaymentType == Constants.FeePaymentType.Cheque.ToInt()))
                        FillEditedChequeDetails(oSqlDataReader);

                    if (aiReceiptNumber != Constants.I_ZERO && aiPaymentType == Constants.FeePaymentType.SwapCard.ToInt())
                        FillEditedCardDetails(oSqlDataReader);

                    if (aiReceiptNumber != Constants.I_ZERO && aiPaymentType == Constants.FeePaymentType.Electronic.ToInt())
                        FillElectronicPaymentDetails(oSqlDataReader);

                    if (abShowOnlyDebits)
                    {
                        oSqlDataReader.NextResult();
                        oSqlDataReader.NextResult();
                        if (oSqlDataReader.Read())
                            miLastChequeBank = oSqlDataReader["LastChequeBank"].ToInt();
                    }
                }
            }
            return lstStudentPaidFeeDetails;
        }

        public List<StudentPaidFeeDetails> GetStudentFeeDetailsForOnlinePartialPayment(string asDueDatesFilterXML, DateTime dtCurrentDate, int aiStudentId, int aiAcademicYearId)
        { 
             List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = new List<StudentPaidFeeDetails>();
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                 oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("CurrentDate", dtCurrentDate, SqlDbType.DateTime);
                 oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("DueDateDetails", asDueDatesFilterXML, SqlDbType.NVarChar);

                 using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeDetailsOfStudentForOnline"))                 
                     return lstStudentPaidFeeDetails = FillStudentPayFeeDetails(oSqlDataReader);                 
             }
        }


        /// <summary>
        /// This method is used to add concession for the student from student UI .
        /// </summary>
        /// <param name="aiAmtToBePaid"></param>
        /// <param name="aiActualAmt"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asStudentFeeIdList"></param>
        /// <param name="asRemarks"></param>
        /// <param name="odtPaymentDate"></param>
        /// <param name="aiConcessionAmt"></param>
        /// <param name="asLateFeeDetails"></param>
        /// <param name="aiLateFeeAmt"></param>
        /// <param name="acIsDirectlyDeposited"></param>
        /// <param name="aiBankId"></param>
        /// <param name="aiLedgerId"></param>
        /// <param name="ReceiptNumber"></param>
        /// <param name="asChallanNo"></param>
        public void PayStudentFee(int aiAmtToBePaid, int aiActualAmt, int aiStudentId, string asStudentFeeIdList, string asRemarks, DateTime odtPaymentDate, int aiConcessionAmt, string asLateFeeDetails, int aiLateFeeAmt, char acIsDirectlyDeposited, int aiBankId, int aiLedgerId, int ReceiptNumber, string asChallanNo)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prmStudentFeeList", asStudentFeeIdList, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("prm_AmtToBePaid", aiAmtToBePaid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_ActualAmt", aiActualAmt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ActualRemarks", asRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PaymentDate", odtPaymentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("LateFeeDetails", asLateFeeDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ConcessionAmt", aiConcessionAmt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ActualLateFeeAmt", aiLateFeeAmt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsDirectlyDeposited", acIsDirectlyDeposited, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", aiBankId, SqlDbType.Int);
                if (aiLedgerId != 0)
                    oSQLServerDbUtility.AddParameter("DepositBankId", aiLedgerId, SqlDbType.Int);
                SqlParameter oSqlReceptNumParam = oSQLServerDbUtility.AddParameter("ReceiptNumberOutput", Constants.I_ZERO, SqlDbType.Int, ParameterDirection.Output);
                if (ReceiptNumber > 0)
                    oSQLServerDbUtility.AddParameter("ReceiptNumber", ReceiptNumber, SqlDbType.Int);
                if (!asChallanNo.IsNullOrEmpty())
                    oSQLServerDbUtility.AddParameter("ChallanNo", asChallanNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", moStudentFeeDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", moStudentFeeDetailsStruct.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_PayFee_WithoutPDC");

                if (oSqlReceptNumParam.Value != DBNull.Value)
                    moStudentFeeDetailsStruct.msReceiptNumber = oSqlReceptNumParam.Value.ToString();
            }
        }

        /// <summary>
        /// This function is used to pay fee with cash.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        /// <returns></returns>
        public string PayStudentFeeWithCash(string asStudentPayFeeXML, string asCreditDetailsXML)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentPayFeeXML", asStudentPayFeeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CreditDetailsXML", asCreditDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("ReceiptNumber", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayStudentFeeWithCash");
                return oSqlParameter.Value.ToString();
            }
        }

        /// <summary>
        /// This function is used to pay fee with cash.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        /// <returns></returns>
        public string PayStudentFeeWithJournalVoucher(string asStudentPayFeeXML, string asCreditDetailsXML, int aiLedgerId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentPayFeeXML", asStudentPayFeeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CreditDetailsXML", asCreditDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LedgerId", aiLedgerId, SqlDbType.Int);                
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("ReceiptNumber", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayStudentFeeWithJournalVoucher");
                return oSqlParameter.Value.ToString();
            }
        }

        /// <summary>
        /// This method is used to pay fee using cheque.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asChequeDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayStudentFeeWithCheque(string asStudentPayFeeXML, string asChequeDetailsXML, string asCreditDetailsXML)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentPayFeeXML", asStudentPayFeeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ChequeDetailsXML", asChequeDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CreditDetailsXML", asCreditDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayFeeWithCheque");
            }
        }

        /// <summary>
        /// This method is used to pay fee using PDC cheque.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asChequeDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayStudentFeeWithPDC(string asStudentPayFeeXML, string asChequeDetailsXML, string asCreditDetailsXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentPayFeeXML", asStudentPayFeeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ChequeDetailsXML", asChequeDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CreditDetailsXML", asCreditDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayFeeWithPDC");
            }
        }

        /// <summary>
        /// This procedure is used to pay fee using swap card.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asCardDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayStudentFeeWithCard(string asStudentPayFeeXML, string asCardDetailsXML, string asCreditDetailsXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentPayFeeXML", asStudentPayFeeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CardDetailsXML", asCardDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CreditDetailsXML", asCreditDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayFeeWithCard");
            }
        }

        /// <summary>
        /// This procedure is used to pay fee using electronic mode.
        /// </summary>
        /// <param name="asStudentPayFeeXML"></param>
        /// <param name="asCardDetailsXML"></param>
        /// <param name="asCreditDetailsXML"></param>
        public void PayFeeWithElectronicMode(string asStudentPayFeeXML, string asElectronicPaymentXML, string asCreditDetailsXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentPayFeeXML", asStudentPayFeeXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ElectronicPaymentXML", asElectronicPaymentXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("CreditDetailsXML", asCreditDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_PayFeeWithElectronicMode");
            }
        }

        /// <summary>
        /// This method is used to pay fee using cheques from import cheques screen of super admin.
        /// </summary>
        /// <param name="aiAmtToBePaid"></param>
        /// <param name="aiActualAmt"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asStudentFeeIdList"></param>
        /// <param name="asRemarks"></param>
        /// <param name="asChequeDetails"></param>
        /// <param name="odtPaymentDate"></param>
        /// <param name="aiConcessionAmt"></param>
        /// <param name="asLateFeeDetails"></param>
        /// <param name="aiLateFeeAmt"></param>
        /// <param name="acIsDirectlyDeposited"></param>
        /// <param name="aiBankId"></param>
        /// <param name="asCreditDetailsList"></param>
        /// <param name="aodtClearanceDate"></param>
        public void PayStudentFeeWithCheque(int aiAmtToBePaid, int aiActualAmt, int aiStudentId, string asStudentFeeIdList, string asRemarks, string asChequeDetails, DateTime odtPaymentDate, int aiConcessionAmt, string asLateFeeDetails, int aiLateFeeAmt, char acIsDirectlyDeposited, int aiBankId, string asCreditDetailsList, DateTime aodtClearanceDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prmStudentFeeList", asStudentFeeIdList, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("prm_AmtToBePaid", aiAmtToBePaid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_ActualAmt", aiActualAmt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ActualRemarks", asRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CreditDetails", asCreditDetailsList, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ChequeDetails", asChequeDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("PaymentDate", odtPaymentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("LateFeeDetails", asLateFeeDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ConcessionAmt", aiConcessionAmt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ActualLateFeeAmt", aiLateFeeAmt, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsDirectlyDeposited", acIsDirectlyDeposited, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BankId", aiBankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiptNumberOutput", Constants.I_ZERO, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.AddParameter("ReceiptNumber", 0, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsImport", true, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("ClearanceDate", aodtClearanceDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("InsertedById", moStudentFeeDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", moStudentFeeDetailsStruct.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_PayFee_WithoutPDC");
            }
        }

        /// <summary>
        /// This method is used to get particular receipt details.
        /// </summary>
        /// <param name="aiReceiptNo"></param>
        /// <returns></returns>
        public static DataSet GetReceiptDetails(int aiReceiptNo, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ReceiptNumber", aiReceiptNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetReceiptDetails");
            }
        }

         /// <summary>
        /// This method is used to get particular receipt details.
        /// </summary>
        /// <param name="aiReceiptNo"></param>
        /// <returns></returns>
        public static DataSet GetReceiptDetailsForSNS(string asReceiptNo, int aiAcademicYearId, int aiAccountHeaderId, int aiStudentId, bool abIsRefundFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ReceiptNumber", asReceiptNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HeaderId", aiAccountHeaderId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsRefundFee", abIsRefundFee, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetReceiptDetailsForSNS");
            }
        }

        public DataTable CheckStudentsStandardDetails(int aiReceiptNo, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ReceiptNo", aiReceiptNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId",aiAcademicYearId , SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CheckStudentsStandardDetails");
            }
        }


        public DataTable GetPaymentClearanceNotification(int aischoolid,int aiacademicyearid)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aischoolid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiacademicyearid, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("Usp_GetNonClearancePaymentList");
            }
        }

        /// <summary>
        /// This method is used to get particular receipt details for nex year using serial number.
        /// </summary>
        /// <param name="aiSerialNo"></param>
        /// <returns></returns>
        public static DataSet GetReceiptDetails(int aiSerialNo)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SerialNumber", aiSerialNo, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetReceiptDetailsForNextYear");
            }
        }

        /// <summary>
        /// This method is used to delete last credit entry of a particular student.
        /// </summary>
        /// <param name="iStudentId"></param>
        /// <param name="sReceiptNo"></param>
        public void DeleteLastCreditEntry(int iStudentId, string sReceiptNo, int aiAccountHeaderId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", iStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiptNo", sReceiptNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AccountHeaderId", aiAccountHeaderId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_DeleteStudentFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to get late fee details.
        /// </summary>
        /// <param name="asStudentFeeIdsList"></param>
        /// <returns>DataTable</returns>      
        public DataTable GetLateFeeDetails(string asStudentFeeIdsList, DateTime oPaymentDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentFeeIdsList", asStudentFeeIdsList, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("PaymentDate", oPaymentDate, SqlDbType.DateTime);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("USP_GetLateFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to get standardwise fee types to pay extra fee payment from pay fee popup.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public DataTable GetStandardFeeType(int aiSchoolId, int aiAcademicYrId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Get_FeeType_Intervals");
            }
        }

        /// <summary>
        /// This method is used to get intervals according to standardwise fee type.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStdFeeTypeId"></param>
        /// <returns></returns>
        public DataTable GetIntervals(int aiSchoolId, int aiAcademicYrId, int aiStdFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Fee_Id", aiStdFeeTypeId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Get_FeeType_Intervals");
            }
        }

        /// <summary>
        /// This method is used to get payable for according to fee type.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiOriginalFeeTypeId"></param>
        /// <returns></returns>
        public DataTable GetFeeTypewisePayableFor(int aiSchoolId, int aiAcademicYearId, int aiOriginalFeeTypeId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Original_Fee_Type_Id", aiOriginalFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetFeeTypeWisePayableForPendingFee");
            }
        }

        /// <summary>
        /// This method is used to get intervals according to standardwise fee type from student pay fee popup.
        /// </summary>
        /// <returns></returns>
        public List<StudentPaidFeeDetails> GetIntervals(int aiStdFeeTypeId, string asStudentDeeTypeIds, bool abIsExcess)
        {
            List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = new List<StudentPaidFeeDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Fee_Id", aiStdFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentFeeIds", asStudentDeeTypeIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsExcess", abIsExcess, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeTypeIntervals"))
                {
                    while (oSqlDataReader.Read())
                    {
                        StudentPaidFeeDetails oStudentPaidFeeDetails = new StudentPaidFeeDetails
                        {
                            StandardwiseFeeTypeId = oSqlDataReader["Std_FeeType_Id"].ToInt(),
                            PayableFor = oSqlDataReader["PayableFor"].ToString(),
                            Amount = oSqlDataReader["Amount"].ToInt()
                        };
                        lstStudentPaidFeeDetails.Add(oStudentPaidFeeDetails);
                    }
                }
            }
            return lstStudentPaidFeeDetails;
        }

        /// <summary>
        /// This method is used to get intervals according to standardwise fee type for student payables screen.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStdFeeTypeId"></param>
        /// <returns></returns>
        public DataTable GetIntervalsWithAmount(int aiSchoolId, int aiAcademicYrId, int aiStdFeeTypeId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Fee_Id", aiStdFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetFeeTypeIntervalWithAmount");
            }
        }

        /// <summary>
        /// This method is used to get intervals according to standardwise fee type.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStdFeeTypeId"></param>
        /// <param name="aiStudentID"></param>
        /// <param name="asStudentDeeTypeIds"></param>
        /// <param name="abIsExcess"></param>
        /// <returns></returns>
        public DataTable GetIntervals(int aiSchoolId, int aiAcademicYrId, int aiStdFeeTypeId, int aiStudentID, string asStudentDeeTypeIds, bool abIsExcess)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Fee_Id", aiStdFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", aiStudentID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentFeeIds", asStudentDeeTypeIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsExcess", abIsExcess, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Get_FeeType_Intervals");
            }
        }

        /// <summary>
        /// Returns the receipt number for the given PDC payment from student payable screen.
        /// </summary>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiPDCId"></param>
        /// <returns></returns>
        public int GetReceiptNoForPDCPayment(int aiYearwiseStudentId, int aiPDCId, int aiMode)
        {
            string sSqlStatement = String.Format("SELECT dbo.udf_GetReceiptNoForPDCPayment({0}, {1},{2})",
                                                    aiYearwiseStudentId,
                                                    aiPDCId,
                                                    aiMode);

            using (var oSqlServerDbUtility = new SQLServerDbUtility())
                return oSqlServerDbUtility.PerformIntQueryOnSqlServer(sSqlStatement);
        }

        /// <summary>
        /// This method is used to rollback all transations if cheque deposited is bounced from student payable screen.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiPDCId"></param>
        /// <param name="asBouncedChequeDetails"></param>
        public void RollBackIfChequeIsBounce(int aiStudentId, int aiPDCId, string asBouncedChequeDetails, int aiMode)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("PDC_ID", aiPDCId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BounceChequeDetails", asBouncedChequeDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Mode", aiMode, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_RollBack_IfCheque_Is_Bounce");
            }
        }

        /// <summary>
        /// This method is used to get total amount paid by given student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aistudentId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public string GetTotalAmtForITConciliationRpt(int aiSchoolId, int aiAcademicYrId, int aistudentId, int aiYear, int aiSelectedAcademicYearId)
        {
            DataTable dtTotAmt = null;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aistudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Value_Member", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Select_AcademicYearId", aiSelectedAcademicYearId, SqlDbType.Int);                
                dtTotAmt = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("USP_ITReconciliation_Statement_Report");
            }
            return dtTotAmt.Rows[Constants.I_ZERO]["TotalAmtPaid"].ToString();
        }

        /// <summary>
        /// This method is used to get student caution money details for show note message.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetStudentCautionMoneyDetails(int aiStudentId, int aiSchoolId)
        {
            string sSelectStatement = "SELECT * " + " FROM vw_StudentCautionMoneyDetails " + " WHERE " + "Schoolwise_Student_Id = (SELECT Student_Id FROM  YearWise_Student_Details " + " WHERE YearWise_Student_Id = " + aiStudentId + ")" + " AND School_Id = " + aiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get fee refund details of student on fee refund UI.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <returns></returns>
        public DataSet GetStudentRefundDetails(int aiStudentId, int aiSchoolId, int aiAcademicYearID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStudentRefundFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to pay fee for next year using cheque,card & electronic payments.
        /// </summary>
        /// <param name="asFeeDetailsXML"></param>
        /// <param name="asPaymentDetailsXML"></param>
        /// <param name="aiLateAmount"></param>
        /// <param name="aiPaymentMode"></param>
        /// <param name="iSerialNo"></param>
        public void InsertStudentFeeDetailsForNextYear(string asFeeDetailsXML, string asPaymentDetailsXML, int aiLateAmount,int aiPaymentMode, out int iSerialNo, int aiConcessionAmount)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Insert_By_ID", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Paid_Date", moStudentPayFeeDetails.PaymentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Remarks", moStudentPayFeeDetails.Remarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FeeDetailsXML", asFeeDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("PaymentDetailsXML", asPaymentDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("LateAmount", aiLateAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentMode", aiPaymentMode, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConcessionAmount", aiConcessionAmount, SqlDbType.Int);                
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("SrNo", 0, SqlDbType.Int, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStudentFeeDetailsForNextYear");
                iSerialNo = Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to pay fee for next year using cash payment
        /// </summary>
        /// <param name="asFeeDetailsXML"></param>
        /// <param name="aiLateAmount"></param>
        /// <param name="iSerialNo"></param>
        public void InsertStudentFeeDetailsForNextYear(string asFeeDetailsXML, int aiLateAmount, out int iSerialNo, int aiConcessionAmount = 0)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Insert_By_ID", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Paid_Date", moStudentPayFeeDetails.PaymentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Remarks", moStudentPayFeeDetails.Remarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FeeDetailsXML", asFeeDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("LateAmount", aiLateAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConcessionAmount", aiConcessionAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentMode", Constants.PaymentMode.Cash.ToInt(), SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("SrNo", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStudentFeeDetailsForNextYear");
                iSerialNo = Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to delete the next year fee payment details.
        /// </summary>
        /// <param name="aiSerialNo"></param>
        /// <param name="aiUserID"></param>
        public void DeleteFeeDetailsForNextYear(int aiSerialNo, int aiUserID)
        {
            string sDeleteStatement = " UPDATE PaidFeeDetailsOfNextYear SET " + " Is_Deleted = 1, Updated_By_ID = " + aiUserID + ", Updated_Date = dbo.GetLocalDate(DEFAULT) " + " WHERE SerialNo = " + aiSerialNo;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to get the fee details for next year fee payment.
        /// </summary>
        /// <param name="abIsNewSudent"></param>
        /// <param name="aiStandardID"></param>
        /// <returns></returns>
        public DataSet getStudentFeeDetailsForNextYear(bool abIsNewSudent, int aiStandardID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsNewStudent", abIsNewSudent, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("iStandardId", aiStandardID, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_FeeDetailsOfNextAcademicYear");
            }
        }

        /// <summary>
        /// This method is used to insert refund fee details from fee refund screen.
        /// </summary>
        /// <param name="aiTotalAmount"></param>
        /// <param name="asRefundFeeDetails"></param>
        public DataTable InsertRefundFeeDetails(int aiTotalAmount, string asRefundFeeDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", moStudentFeeDetailsStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", moStudentFeeDetailsStruct.miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStudentFeeDetailsStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Div_Id", moStudentFeeDetailsStruct.miStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moStudentFeeDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Paid_Date", moStudentFeeDetailsStruct.mdtPaidDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Remarks", moStudentFeeDetailsStruct.msRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Amount", aiTotalAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RefundFeeDetails", asRefundFeeDetails, SqlDbType.Xml);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_InsertStudentRefundFeeDetails");
               
            }
        }

        /// <summary>
        /// This method is used to insert refund fee details from fee refund screen.
        /// </summary>
        /// <param name="adtChequeDate"></param>
        /// <param name="aiChequeNumber"></param>
        /// <param name="aiBankId"></param>
        /// <param name="aiTotalAmount"></param>
        /// <param name="asRefundFeeDetails"></param>
        public void InsertRefundFeeDetails(DateTime adtChequeDate, int aiChequeNumber, int aiBankId, int aiTotalAmount, string asRefundFeeDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", moStudentFeeDetailsStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", moStudentFeeDetailsStruct.miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStudentFeeDetailsStruct.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Div_Id", moStudentFeeDetailsStruct.miStandardDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_id", moStudentFeeDetailsStruct.miInsertedByid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Paid_Date", moStudentFeeDetailsStruct.mdtPaidDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Remarks", moStudentFeeDetailsStruct.msRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Amount", aiTotalAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RefundFeeDetails", asRefundFeeDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ChequeDate", adtChequeDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ChequeNumber", aiChequeNumber, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BankId", aiBankId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStudentRefundFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to delete the refunded entries from fee base screen.
        /// </summary>
        /// <param name="aiRefundFeeDetailsID"></param>
        public void DeleteRefundFeeDetails(int aiRefundFeeDetailsID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("RefundFeeDetailsID", aiRefundFeeDetailsID, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteStudentRefundFeeDetails");
            }
        }

        /// <summary>
        /// This method is used to return the online fee payment details on student login to pay fee online.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcdYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asDueDatesFilterXML"></param>
        /// <returns></returns>
        public DataSet GetFeeDetailsForOnlineFee(int aiSchoolId, int aiAcdYrId, int aiStudentId, string asDueDatesFilterXML, int aiSchoolwiseStudentFeeId, bool abIsCautionMoneyOnlinePayment, bool abIsInternalFeeOnlinePayment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcdYrId", aiAcdYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DueDates", asDueDatesFilterXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentFeeId", aiSchoolwiseStudentFeeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsCautionMoneyOnlinePayment", abIsCautionMoneyOnlinePayment, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsInternalFeeOnlinePayment", abIsInternalFeeOnlinePayment, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetFeeDetailsForOnlineFee");
            }
        }

        /// <summary>
        /// This method is call from Incomplete TransactionUI to get the incomplete fee transaction for selected criteria.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRegNo"></param>
        /// <param name="asTransactionDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <returns></returns>
        public static DataTable GetInCompleteTransaction(int aiSchoolId, int aiAcademicYearId, string asRegNo, DateTime asTransactionDate, string asPaymentCategoryFeeId, String sortExpression, bool IsIncomplete, int aiEndIndex, int aiStartRowIndex)
        {
            string sFilter = CreateFilter(asRegNo, asTransactionDate, asPaymentCategoryFeeId);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PaymentCategoryFeeId", asPaymentCategoryFeeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsIncomplete", IsIncomplete, SqlDbType.Bit);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedInCompleteTransaction");
            }
        }

        /// <summary>
        /// This method is used to get all the incompleted admission transaction which is accessed from Incomplete transactionUI.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asMobileNumber"></param>
        /// <param name="asTransactionDate"></param>
        /// <param name="sortExpression"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="aiStartRowIndex"></param>
        /// <returns></returns>
        public static DataTable GetInCompleteAdmissionTransaction(int aiSchoolId, int aiAcademicYearId, string asMobileNumber, DateTime asTransactionDate, String sortExpression, bool IsIncomplete, int aiEndIndex, int aiStartRowIndex)
        {
            string sDate = asTransactionDate != null && asTransactionDate != DateTime.MinValue ? asTransactionDate.ToString() : null;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MobileNumber", asMobileNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransactionDate", sDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsIncomplete", IsIncomplete, SqlDbType.Bit);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedInCompleteAdmissionTransaction");
            }
        }

        /// <summary>
        /// This method is used to get all the incompleted admission transaction which is accessed from Incomplete transactionUI.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asMobileNumber"></param>
        /// <param name="asTransactionDate"></param>
        /// <returns></returns>
        public static int CountRowsOfInCompleteAdmissionTransaction(int aiSchoolId, int aiAcademicYearId, string asMobileNumber, DateTime asTransactionDate, bool IsIncomplete)
        {
            string sDate = asTransactionDate != null && asTransactionDate != DateTime.MinValue ? asTransactionDate.ToString() : null;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MobileNumber", asMobileNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransactionDate", sDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("IsIncomplete", IsIncomplete, SqlDbType.Bit);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountInCompleteAdmissionTransaction");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get all extra fee details for a selected standard to copy fee configuration from one standard to another.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSerialNumber"></param>
        /// <returns></returns>
        public static DataTable GetStandardFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiSerialNumber)
        {
            string sFetchStatement = " SELECT     Schoolwise_Debit__Entry_Id as DebitID, Payable_For, Amount, Fee_Type, Paid_Date, Remarks, IsInternalFee, Standard_Id,IsDueDateApplicable,IsOnlinePaymentApplicable" + " FROM       Schoolwise_Debit_Entry_Log " + " WHERE	   School_Id =  " + aiSchoolId + " AND Academic_Year_Id = " + aiAcademicYearId + " AND Serial_Number = " + aiSerialNumber + " AND Is_Deleted = 'N' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchStatement);
        }

        public static int GetAccountHeaderIdBySerialNo(int aiSchoolId, int aiAcademicYearId, int aiSerialNumber, int aiIsForInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId",aiSchoolId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SerialNumber", aiSerialNumber, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsForInternalFee", aiIsForInternalFee, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("AccountHeaderId", 0, SqlDbType.Int,ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetAccountHeaderIdBySerialNo");
                int iAccountHeaderId = 0;
                if (oSqlParameter.Value != DBNull.Value)
                    iAccountHeaderId = oSqlParameter.Value.ToInt();
                return iAccountHeaderId;
            }
        }

        /// <summary>
        /// This method is used to get the fee details for a selected standard to copy fee configuration from one standard to another.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardID"></param>
        /// <param name="asFeeType"></param>
        /// <param name="asPayableFor"></param>
        /// <returns></returns>
        public static DataTable GetStandardListForFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStandardID, string asFeeType, string asPayableFor)
        {
            string sFetchStatement = " SELECT     Schoolwise_Debit_Entry_Log.Standard_Id, Standard_Master.Standard_Name " + " FROM         Schoolwise_Debit_Entry_Log INNER JOIN " + " Standard_Master ON Schoolwise_Debit_Entry_Log.Standard_Id = Standard_Master.Standard_Id AND " + " Schoolwise_Debit_Entry_Log.School_Id = Standard_Master.School_Id AND  " + " Schoolwise_Debit_Entry_Log.Academic_Year_Id = Standard_Master.academic_Year_Id " + " WHERE	   Schoolwise_Debit_Entry_Log.School_Id =  " + aiSchoolId + " AND Schoolwise_Debit_Entry_Log.Academic_Year_Id = " + aiAcademicYearId + " AND Schoolwise_Debit_Entry_Log.Fee_Type = N'" + StringUtility.ReplaceSingleQuoteInString(asFeeType, false) + "'" + " AND Schoolwise_Debit_Entry_Log.Payable_For =N'" + StringUtility.ReplaceSingleQuoteInString(asPayableFor, false) + "'" + " AND Schoolwise_Debit_Entry_Log.Standard_Id <> " + aiStandardID + " AND Schoolwise_Debit_Entry_Log.Is_Deleted = 'N' AND Standard_Master.Is_Deleted = 'N'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchStatement);
        }

        /// <summary>
        /// This returns all the data required to show the fees mini receipt.
        /// </summary>
        /// <param name="aiSubmissionID"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataSet GetAdmissionReceiptDetails(int aiSubmissionID, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SubmissionID", aiSubmissionID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAdmissionReceiptDetails");
            }
        }

        /// <summary>
        /// This method is used to fill all the Bank related combos from multiple screens.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet GetBankDetailsForNetBanking(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetBankDetailsForNetBanking");
            }
        }

        /// <summary>
        /// This method is used to check whether the given cheque number is alreday exists or not.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiBankId"></param>
        /// <param name="aiChequeNumber"></param>
        /// <returns></returns>
        public bool IsDuplicateChequeNumber(int aiBankId, int aiChequeNumber)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Bank_Id", aiBankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Cheque_No", aiChequeNumber, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", null, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DuplicateChequeNoForNextYear");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

       

        /// <summary>
        /// This method is used to check whether the given card number is alreday exists or not.
        /// </summary>
        /// <param name="asCardNumber"></param>
        /// <returns></returns>
        public bool IsDuplicateCardNumber(string asCardNumber)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id",miSchoolId , SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Card_No", asCardNumber, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", null, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DuplicateCardNoForNextYear");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to check if the Txn number is duplicated for the current student or not.
        /// </summary>
        /// <param name="asTxnNumber"></param>
        /// <returns></returns>
        public bool IsDuplicateTxnNumberForNextYear(string asTxnNumber)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId",miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TxnNumber", asTxnNumber, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", null, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsDuplicateTxnNumberForNextYear");
                return Convert.ToBoolean(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to display fee details on the Pay fee for next year popup on pagelaod.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aodtCurrentDate"></param>
        /// <param name="AcademicYearId"></param>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        public static DataTable GetFeeDetailsForDisplay(int aiStudentId, DateTime aodtCurrentDate, out int AcademicYearId, out int StudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CurrentDate", aodtCurrentDate, SqlDbType.DateTime);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("NextAcademicYearID", null, SqlDbType.Int, ParameterDirection.Output);
                SqlParameter oSqlParameterStudentId = oSQLServerDbUtility.AddParameter("NextYearStudentID", null, SqlDbType.Int, ParameterDirection.Output);

                DataTable oDT = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DispalyNextYearFeeDetails");
                AcademicYearId = Convert.ToInt32(oSqlParameter.Value);
                StudentId = Convert.ToInt32(oSqlParameterStudentId.Value);
                return oDT;
            }
        }

        /// <summary>
        /// This method is used to get the recipt numbers to show the previous receipts generated for the student.
        /// </summary>
        /// <param name="aiPaymentMode"></param>
        /// <returns></returns>
        public DataTable GetReceiptNoToUpdate(int aiPaymentMode)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentMode", aiPaymentMode, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetRecieptNoToUpdate");
            }
        }

        /// <summary>
        /// This method is used to get the receipt number to print the receipt from Pay fee popup screen.
        /// </summary>
        /// <param name="iStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <returns></returns>
        public static int GetReceiptNo(int iStudentId, int aiSchoolId, int aiAcademicYrId, int aiFeeTypeId)
        {
            string sFetchStatement = "SELECT Receipt_Number" + " FROM Schoolwise_Student_Fee_Details" + " WHERE Student_Id=" + iStudentId + " and Student_Fee_Id=" + aiFeeTypeId + " and Is_Deleted='N'" + " AND Receipt_Number IS NOT Null";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sFetchStatement);
        }

        /// <summary>
        /// This method is used to show previous year pending fee messege on the base screen of fee.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static string PreviousFeesPending(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CurrentDate", DateTime.Today, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", "", SqlDbType.NVarChar, ParameterDirection.Output,100);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetLastYearPendingFees");
                return Convert.ToString(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to check student student is presnet or absent as per configureds days.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static string IsStudentAbsent(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {                
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("ReturnMessage", "", SqlDbType.NVarChar, ParameterDirection.Output, 300);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_IsStudentIsAbsent");
                return Convert.ToString(oSqlParameter.Value);
            }
        }


        /// <summary>
        /// This method is used to show previous year pending fee messege on the base screen of fee.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static bool PreviousInternalFeesPending(int aiSchoolId, int aiAcademicYrId, int aiStudentId, out string aiAcademicYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);

                SqlParameter oSqlParameters = oSQLServerDbUtility.AddParameter("LastYearPendingFeeStatus", 0, SqlDbType.Bit, ParameterDirection.Output);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("AcademicYear", "", SqlDbType.NVarChar, ParameterDirection.Output, 100);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_LastYearPendingInternalFeeStatus");

                aiAcademicYear = Convert.ToString(oSqlParameter.Value);

                return Convert.ToBoolean(oSqlParameters.Value);
            }
        }

        public  List<AccountHeaderDetails> GetAccountHeaderDetails(int aiSchoolId, bool abIsForInternal)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsForInternal", abIsForInternal, SqlDbType.Bit);
                List<AccountHeaderDetails> lstAccountHeaderDetails = new List<AccountHeaderDetails>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAccountHeaderDetails"))
                { 
                    if(oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {

                           AccountHeaderDetails oAccountHeaderDetails = new AccountHeaderDetails()
                            {
                                AccountHeaderId = oSqlDataReader["AccountHeaderId"].ToInt(),
                                AccountHeaderName = oSqlDataReader["AccountHeaderName"].ToString()
                            };
                           lstAccountHeaderDetails.Add(oAccountHeaderDetails);
                        };
                }
                    
                }
                return lstAccountHeaderDetails;
            }
        }


        /// <summary>
        /// This method is used to get remark for selected receipt.
        /// </summary>
        /// <param name="aiReceiptNo"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcademicYearID"></param>
        /// <returns></returns>
        public static string GetRemark(int ReceiptNo, int iSchoolID, int iAcademicYearID)
        {
            string sFetchStatement = "SELECT DISTINCT Remarks" + " FROM Schoolwise_Student_Fee_Details" + " WHERE Receipt_Number=" + ReceiptNo + " and School_ID=" + iSchoolID + " and Academic_Year_Id=" + iAcademicYearID + " and Is_Deleted='N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sFetchStatement);
        }

        /// <summary>
        /// This method is used to check that selected student from search grid is on leave or not.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static string IsOnLeave(int aiStudentID, int aiSchoolID, int aiAcademicYrID)
        {
            string sSelectStatement = "SELECT [dbo].[udf_CheckIfStudentIsOnLeave](" + aiStudentID + "," + aiAcademicYrID + "," + aiSchoolID + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        ///		Gets a list of fee defaulters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<FeeDefaulter> GetFeeDefaulters(int aiSchoolId)
        {
            var lstFeeDefaulters = new List<FeeDefaulter>();

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeDefaulters"))
                    if (oReader.HasRows)
                        lstFeeDefaulters = new GenericClass<FeeDefaulter>().GetFilledObjectList(oReader);
            }

            return lstFeeDefaulters;
        }

        /// <summary>
        ///		Deactivates the a/c of defaulters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asUsersXML"></param>
        public static void DeactivateDefaulters(int aiSchoolId, string asUsersXML)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UsersXML", asUsersXML, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeactivateFeeDefaulters");
            }
        }

        /// <summary>
        ///		DAL : Activates the student if there are no more fees pending for which deactivation settings do not apply to the user.
        /// </summary>
        /// <param name="aiStudentId">YearwiseStudentId of the student to be activated.</param>
        public void ActivateFeeDefaulter(int aiStudentId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ActivateFeeDefaulter"))
                {
                    while (oSqlDataReader.Read())
                    {
                        CanSendSMS = oSqlDataReader["SendSMS"].ToBool();
                        msMobileNumber = oSqlDataReader["MobileNumber"].ToString();
                        if (oSqlDataReader["UserId"] != DBNull.Value)
                            miFeeDefaulterUserId = oSqlDataReader["UserId"].ToInt();
                        msDesignation = oSqlDataReader["Designation"].ToString();
                    }
                }
            }
        }

       /// <summary>
       /// This method is used to get the mobile numbers of student after fee payment.
       /// </summary>
        public void GetStudentMobileNumber()
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentMobileNumbers"))
                {
                    if(oSqlDataReader.Read())
                    {
                        msMobileNumber = oSqlDataReader["MobileNumber"].ToString();                        
                        miFeeDefaulterUserId = oSqlDataReader["UserId"].ToInt();
                        msDesignation = oSqlDataReader["Designation"].ToString();
                    }
                }
            }
        }

        /// <summary>
        ///		Determines if the given Challan no already exists in the system.
        ///		Does not consider the challan no for the given ReceipNo, it is not empty.
        /// </summary>
        ///<param name="aiSchoolId"></param>
        ///<param name="aiAcademicYearId"></param>
        ///<param name="asChallanNo"></param>
        ///<param name="asReceiptNo"></param>
        /// <returns></returns>
        public static bool IsDuplicateChallanNo(int aiSchoolId, int aiAcademicYearId, string asChallanNo, string asReceiptNo)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChallanNo", asChallanNo, SqlDbType.NVarChar);
                if (!asReceiptNo.IsNullOrEmpty())
                    oSQLServerDbUtility.AddParameter("ReceiptNo", asReceiptNo, SqlDbType.NVarChar);

                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_IsChallanNoDuplicate"))
                {
                    if (oReader.HasRows && oReader.Read())
                        return oReader["IsDuplicate"].ToBool();
                }
            }

            return true;
        }

        /// <summary>
        ///		Returns Fee collection details for a given academic year. Option to include internal fees and caution money.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abIncludeInternalFees"></param>
        /// <param name="abIncludeCautionMoney"></param>
        /// <returns></returns>
        public FeeCollection GetFeeCollectionDetails(int aiSchoolId, int aiAcademicYearId, bool abIncludeInternalFees, bool abIncludeCautionMoney)
        {
            var oFeeCollection = new FeeCollection();

            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("IncludeInternalFees", abIncludeInternalFees, SqlDbType.Bit);
                oSqlDbUtility.AddParameter("IncludeCautionMoney", abIncludeCautionMoney, SqlDbType.Bit);

                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeCollectionDetails"))
                    if (oReader.Read())
                        oFeeCollection = new FeeCollection
                            {
                                Fees = oReader["Fees"].ToInt(),
                                InternalFees = oReader["InternalFees"].ToInt(),
                                CautionMoney = oReader["CautionMoney"].ToInt(),
                            };
            }

            return oFeeCollection;
        }

        /// <summary>
        /// This method is used to check if there exists a pending fee for a student. And is used to block progress report from progress report screen.
        /// </summary>
        /// <returns></returns>
        public bool PendingFeesAvailableForStudent()
        {
            int iStudentPedingFeeCount = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", StudentFeeDetailsStructDetails.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcadmicYearId", StudentFeeDetailsStructDetails.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", StudentFeeDetailsStructDetails.miStudentId, SqlDbType.Int);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("USP_StudentPendingFeesCount"))
                {
                    if (oReader.Read())
                        iStudentPedingFeeCount = oReader["Count"].ToInt();
                }
            }

            return iStudentPedingFeeCount > 0;
        }

        /// <summary>
        /// This method is used to load other fee types from student payables screen.
        /// </summary>
        /// <param name="aiSchoold"></param>
        /// <returns></returns>
        public List<string> GetOtherFeeTypes(int aiSchoolId, bool abIsInternalFee)
        {
            var lstOtherFeeTypes = new List<string>();

            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                
                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOtherFeeTypes"))
                    if (oReader.HasRows)
                        while (oReader.Read())
                            lstOtherFeeTypes.Add(oReader["Fee_Type"].ToString());
            }

            return lstOtherFeeTypes;
        }

        /// <summary>
        /// This method is used to get electronic payment types.
        /// </summary>
        /// <returns></returns>
        public List<ElectronicPaymentType> GetElectronicPaymentTypes()
        {
            List<ElectronicPaymentType> lstElectronicTypes = new List<ElectronicPaymentType>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetElectronicPaymentTypes"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ElectronicPaymentType oElectronicPaymentType = new ElectronicPaymentType
                        {
                            TypeId = oSqlDataReader["TypeId"].ToInt(),
                            Type = oSqlDataReader["Type"].ToString()
                        };
                        lstElectronicTypes.Add(oElectronicPaymentType);
                    }
                }
            }

            return lstElectronicTypes;
        }

        /// <summary>
        /// This method will be used to check whether the transaction number is duplicated for NEFT RTGS transaction.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateElectronicTxn(string asTxnNumber, Constants.PaymentMode aoMode)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Mode", aoMode.ToInt(), SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TxnNo", asTxnNumber, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_DuplicateElectronicTxn"))
                {
                    if (oSqlDataReader.Read())
                        return oSqlDataReader["Status"].ToBool();

                }
                return false;
            }
        }

        /// <summary>
        /// This method is used to get the electronic payment details on clearance details screen.
        /// </summary>
        /// <param name="aoFeeClearanceFilters"></param>
        /// <returns></returns>
        public List<StudentFeeClearanceDetails> GetElectronicPayments(FeeClearanceFilters aoFeeClearanceFilters, bool abIsInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {     
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeAll", aoFeeClearanceFilters.IncludeAll, SqlDbType.Bit);
                if (!aoFeeClearanceFilters.TransactionNumber.IsNullOrEmpty())
                    oSQLServerDbUtility.AddParameter("TransactionNumber", aoFeeClearanceFilters.TransactionNumber, SqlDbType.NVarChar);
                if (!aoFeeClearanceFilters.RegNo.IsNullOrEmpty())
                    oSQLServerDbUtility.AddParameter("RegNo", aoFeeClearanceFilters.RegNo, SqlDbType.NVarChar);
                if (aoFeeClearanceFilters.PaymentStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("PaymentStartDate", aoFeeClearanceFilters.PaymentStartDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.PaymentEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("PaymentEndDate", aoFeeClearanceFilters.PaymentEndDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.ClearanceStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("ClearanceStartDate", aoFeeClearanceFilters.ClearanceStartDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.ClearanceEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("ClearanceEndDate", aoFeeClearanceFilters.ClearanceEndDate, SqlDbType.DateTime);

                oSQLServerDbUtility.AddParameter("TypeId", aoFeeClearanceFilters.TypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DepositBankId", aoFeeClearanceFilters.DepositedBankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeCautionMoney", aoFeeClearanceFilters.IncludeCautionMoney, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetElectronicPayments"))
                    return LoadElectronicFeeClearanceList(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get the electronic payment details on clearance details screen.
        /// </summary>
        /// <param name="aoFeeClearanceFilters"></param>
        /// <returns></returns>
        public int GetElectronicPaymentsCount(FeeClearanceFilters aoFeeClearanceFilters)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeAll", aoFeeClearanceFilters.IncludeAll, SqlDbType.Bit);
                if (!aoFeeClearanceFilters.RegNo.IsNullOrEmpty())
                    oSQLServerDbUtility.AddParameter("RegNo", aoFeeClearanceFilters.RegNo, SqlDbType.NVarChar);
                if (aoFeeClearanceFilters.PaymentStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("PaymentStartDate", aoFeeClearanceFilters.PaymentStartDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.PaymentEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("PaymentEndDate", aoFeeClearanceFilters.PaymentEndDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.ClearanceStartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("ClearanceStartDate", aoFeeClearanceFilters.ClearanceStartDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.ClearanceEndDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("ClearanceEndDate", aoFeeClearanceFilters.ClearanceEndDate, SqlDbType.DateTime);
                if (aoFeeClearanceFilters.TypeId > 0)
                    oSQLServerDbUtility.AddParameter("TypeId", aoFeeClearanceFilters.TypeId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Count", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CountElectronicPayments");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to update electronic payment clearance details.
        /// </summary>
        /// <param name="asElectronicPaymentXML"></param>
        public void UpdateElectronicPaymentClearance(string asElectronicPaymentXML, bool abIsInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ElectronicPaymentXML", asElectronicPaymentXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateElectronicPaymentClearance");
            }
        }

        /// <summary>
        /// This method is used to update electronic payment clearance details.
        /// </summary>
        /// <param name="asElectronicPaymentXML"></param>
        public void UpdateElectronicPaymentCautionMoneyClearance(string asElectronicPaymentXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ElectronicPaymentXML", asElectronicPaymentXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentCautionMoneyElectronicPayments");
            }
        }

        /// <summary>
        /// This method is used to GetAll fee details to Export.
        /// </summary>
        /// <param name="aiFeetypeId"></param>
        /// <param name="asStandardId"></param>
        public List<FeeStandards> GetAllFeeDetailsForExport(string asStandardId, int aiFeetypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", asStandardId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FeeTypeId", aiFeetypeId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllFeeForExport"))
                {
                    List<FeeStandards> lstFeeStandard = FillStandardFeeDetails(oSqlDataReader);                  

                    return lstFeeStandard;
                }
            }
        }

        public bool ShowInauguralCertificateOption(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId, int aiStandardId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiStandardDivisionId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDetsilsForInauguralCertificate"))
                {
                    if (oSqlDataReader.HasRows)
                        return true;
                    else
                        return false;
                }
            }
        }
     
        #endregion

        #region Private Methods

        /// <summary>
        /// This method is a private method used to load standards for Fee Export.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private List<FeeStandards> FillStandardFeeDetails(SqlDataReader aoSqlDataReader)
        {
            List<FeeStandards> lstFeeStandards = new List<FeeStandards>();
            while (aoSqlDataReader.Read())
            { 
                lstFeeStandards.Add
                    (
                        new FeeStandards
                        {
                            StandardId = Convert.ToInt32(aoSqlDataReader["StandardId"]),                            
                            OriginalStandardId = Convert.ToInt32(aoSqlDataReader["OriginalStandardId"]),
                            StandardName = Convert.ToString(aoSqlDataReader["StandardName"]),
                            FeeType = Convert.ToString(aoSqlDataReader["FeeType"]),
                            PayableFor = Convert.ToString(aoSqlDataReader["PayableFor"]),
                            Count = Convert.ToInt32(aoSqlDataReader["Count"]),
                            PayableAmount = Convert.ToInt32(aoSqlDataReader["Amount"]),
                            OriginalFeeTypeId = Convert.ToInt32(aoSqlDataReader["OriginalFeeTypeId"]),
                            Type = Convert.ToInt32(aoSqlDataReader["Type"]),
                            HeaderId = Convert.ToInt32(aoSqlDataReader["HeaderId"]),
                            HeaderName = Convert.ToString(aoSqlDataReader["HeaderName"]),
                            IsPrePrimary = Convert.ToChar(aoSqlDataReader["IsPrePrimary"])
                        }
                    );
            }
            return lstFeeStandards;
        }       

        /// <summary>
        /// This method is a private method used to load electronic fee clearance details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentFeeClearanceDetails> LoadElectronicFeeClearanceList(SqlDataReader aoSqlDataReader)
        {
            List<StudentFeeClearanceDetails> lstFeeClearanceDetails = new List<StudentFeeClearanceDetails>();
            while (aoSqlDataReader.Read())
            {
                lstFeeClearanceDetails.Add(new StudentFeeClearanceDetails
                {
                    StudentName = aoSqlDataReader["StudentName"].ToString(),
                    Class = aoSqlDataReader["ClassName"].ToString(),
                    RegNo = aoSqlDataReader["RegNo"].ToString(), 
                    Receipt_Number = aoSqlDataReader["Receipt_Number"].ToInt(),
                    StudentElectronicPaymentId = aoSqlDataReader["StudentElectronicPaymentId"].ToInt(),
                    TransactionNumber = aoSqlDataReader["TransactionNumber"].ToString(),
                    IsCautionMoneyPayment = aoSqlDataReader["IsCautionMoneyPayment"].ToInt(),
                    oFeeClearanceFilters = new FeeClearanceFilters
                    {
                        TypeId = aoSqlDataReader["TypeId"].ToInt(),
                        ClearanceStartDate = (!aoSqlDataReader["ClearanceDate"].ToString().IsNullOrEmpty() ? aoSqlDataReader["ClearanceDate"].ToDateTime() : DateTime.MinValue.Date)
                    },
                    oStudentPayFeeDetails = new StudentPayFeeDetails
                    {
                        StudentId = aoSqlDataReader["StudentId"].ToInt(),
                        DepositeBankId = aoSqlDataReader["DepositedBankId"].ToInt(),
                        ActualAmount = aoSqlDataReader["Amount"].ToInt(),
                        PaymentDate = aoSqlDataReader["PaidDate"].ToDateTime(),
                        Remarks = aoSqlDataReader["PayableFor"].ToString()                        
                    }
                });
            }

            if (aoSqlDataReader.NextResult() && aoSqlDataReader.Read())
                TotalAmount = aoSqlDataReader["TotalAmount"].ToInt();

            return lstFeeClearanceDetails;
        }

        public int GetAccountHeaderIdByFeeType(int aiSchoolId, int aiStandardId, int aiFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeeTypeId", aiFeeTypeId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("AccountHeaderId", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetAccountHeaderId");
                return (oSqlParameter.Value).ToInt();
            }
        
        }


        /// <summary>
        ///  This function is used to load the SchoolwiseStudentFeeDetails Details from constructor.
        /// </summary>
        /// <param name="miStudentFeeId"></param>
        private void LoadStudentFeeDetailsDetails(int miStudentFeeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchStudentFeeDetailsDetails(miStudentFeeId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Schoolwise_Student_Fee_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miSchoolwiseStudentFeeId = Convert.ToInt32(oDR["Schoolwise_Student_Fee_Id"]);

                            if (oDR["Student_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStudentId = Convert.ToInt32(oDR["Student_Id"]);

                            if (oDR["Payable_For"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msPayableFor = Convert.ToString(oDR["Payable_For"]);

                            if (oDR["Standard_Div_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStandardDivId = Convert.ToInt32(oDR["Standard_Div_Id"]);

                            if (oDR["Std_FeeType_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStdFeeTypeId = Convert.ToInt32(oDR["Std_FeeType_Id"]);

                            if (oDR["Amount"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miAmount = Convert.ToInt32(oDR["Amount"]);

                            if (oDR["Debit/Credit"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msDebitOrCredit = Convert.ToString(oDR["Debit/Credit"]);

                            if (oDR["Paid_Date"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mdtPaidDate = Convert.ToDateTime(oDR["Paid_Date"]);

                            if (oDR["Receipt_Number"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msReceiptNumber = Convert.ToString(oDR["Receipt_Number"]);

                            if (oDR["Remarks"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msRemarks = Convert.ToString(oDR["Remarks"]);

                            if (oDR["Student_Fee_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miStudentFeeId = Convert.ToInt32(oDR["Student_Fee_Id"]);

                            if (oDR["School_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);

                            if (oDR["Academic_Year_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miAcademicYearId = Convert.ToInt32(oDR["Academic_Year_Id"]);

                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);

                            if (oDR["Insert_Date"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);

                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"]);

                            if (oDR["Update_Date"] != DBNull.Value)
                                moStudentFeeDetailsStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);

                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);

                            if (oDR["Fee_Type"] != DBNull.Value)
                                moStudentFeeDetailsStruct.msFeeType = Convert.ToString(oDR["Fee_Type"]);

                            if (oDR["Serial_Number"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miSerialNumber = Convert.ToInt32(oDR["Serial_Number"]);

                            if (oDR["AccountHeaderId"] != DBNull.Value)
                                moStudentFeeDetailsStruct.miAccountHeaderId = Convert.ToInt32(oDR["AccountHeaderId"]);

                        }

                    }
                }
            }
        }

        /// <summary>
        /// This method is used to generate the sql statement and used for the local purpose.
        /// </summary>
        /// <param name="miStudentFeeId"></param>
        /// <returns></returns>
        private string FetchStudentFeeDetailsDetails(int miStudentFeeId)
        {
            string sSelectStatement = " SELECT  " + "Schoolwise_Student_Fee_Id" + ",Student_Id" + ",Payable_For" + ",Standard_Div_Id" + ",Std_FeeType_Id" + ",Amount" + ",[Debit/Credit]" + ",Paid_Date" + ",Receipt_Number" + ",Remarks" + ",Student_Fee_Id" + ",School_Id" + ",Academic_Year_Id" + ",Is_Deleted" + ",Insert_Date" + ",Inserted_By_id" + ",Update_Date" + ",Updated_By_Id" + ",Fee_Type" + ",Serial_Number" + ", AccountHeaderId" + " FROM Schoolwise_Student_Fee_Details" + " WHERE Schoolwise_Student_Fee_Id=" + miStudentFeeId;
            return sSelectStatement;
        }

        /// <summary>
        /// This method is used for local purpose to fill up the edited fee swapcard details required to show edited data on the screen.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillEditedCardDetails(SqlDataReader aoSqlDataReader)
        {
            aoSqlDataReader.NextResult();
            while (aoSqlDataReader.Read())
            {
                SwapCardDetails oSwapCardDetails = new SwapCardDetails
                {
                    SwapNo = aoSqlDataReader["Swap_Number"].ToString(),
                    CardTypeId = aoSqlDataReader["CardTypeId"].ToInt()
                };
                moSwapCardDetails = oSwapCardDetails;
                msRemarks = (!aoSqlDataReader["Remarks"].IsNull() ? aoSqlDataReader["Remarks"].ToString() : string.Empty);
                miDepositedBankId = (!aoSqlDataReader["DepositBankId"].IsNull() ? aoSqlDataReader["DepositBankId"].ToInt() : Constants.I_ZERO);
            }
        }

        /// <summary>
        /// This method is used for local purpose to fill up the edited fee electronic payment details required to show edited data on the screen.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillElectronicPaymentDetails(SqlDataReader aoSqlDataReader)
        {
            aoSqlDataReader.NextResult();
            while (aoSqlDataReader.Read())
            {
                ElectronicPaymentDetails oElectronicPaymentDetails = new ElectronicPaymentDetails
                {
                    TxnNo = aoSqlDataReader["TxnNo"].ToString(),
                    oElectronicPaymentType = new ElectronicPaymentType
                    {
                        TypeId = aoSqlDataReader["TypeId"].ToInt()
                    }
                };
                moElectronicPaymentDetails = oElectronicPaymentDetails;
                msRemarks = (!aoSqlDataReader["Remarks"].IsNull() ? aoSqlDataReader["Remarks"].ToString() : string.Empty);
                miDepositedBankId = (!aoSqlDataReader["DepositBankId"].IsNull() ? aoSqlDataReader["DepositBankId"].ToInt() : Constants.I_ZERO);
            }
        }

        /// <summary>
        /// This method is used for local purpose to fill up the edited fee cheque details required to show edited data on the screen.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillEditedChequeDetails(SqlDataReader aoSqlDataReader)
        {
            List<ChequeDetails> lstChequeDetails = new List<ChequeDetails>();
            aoSqlDataReader.NextResult();
            while (aoSqlDataReader.Read())
            {
                ChequeDetails oChequeDetails = new ChequeDetails
                {
                    ChequeNumber = aoSqlDataReader["Cheque_Number"].ToString(),
                    ChequeDate = aoSqlDataReader["Cheque_Date"].ToDateTime(),
                    BankId = aoSqlDataReader["Bank_Id"].ToInt(),
                    IsPDC = (aoSqlDataReader["Is_PDC"].ToString() == Constants.S_YES ? true : false),
                    Remarks = (!aoSqlDataReader["Remarks"].IsNull() ? aoSqlDataReader["Remarks"].ToString() : string.Empty)
                };
                lstChequeDetails.Add(oChequeDetails);
                miDepositedBankId = aoSqlDataReader["DepositBankId"].ToInt();
            }
            mlstChequeDetails = lstChequeDetails;
        }

        /// <summary>
        /// This method is used for local purpose to fill up the edited fee details required to show edited data on the screen.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <param name="aiReceiptNumber"></param>
        private void FillEditedFeeDetails(SqlDataReader aoSqlDataReader, int aiReceiptNumber)
        {
            if (aiReceiptNumber != Constants.I_ZERO)
            {
                aoSqlDataReader.NextResult();
                while (aoSqlDataReader.Read())
                {
                    StudentPayFeeDetails oStudentPayFeeDetail = new StudentPayFeeDetails
                    {
                        IsDirectlyDeposited = aoSqlDataReader["IsDirectlyDeposited"].ToBool(),
                        BankId = aoSqlDataReader["Bank_Id"].ToInt(),
                        DepositeBankId = aoSqlDataReader["DepositBankId"].ToInt(),
                        ChallanNumber = aoSqlDataReader["ChallanNo"].ToString(),
                        Remarks = aoSqlDataReader["Remarks"].ToString(),
                        AdditionalRemark = aoSqlDataReader["AdditionalRemark"].ToString(),
                        PaymentDate = aoSqlDataReader["Paid_Date"].ToDateTime(),
                        JournalVoucherLedgerId = aoSqlDataReader["JournalVoucherLedgerId"].ToInt()
                    };

                    EditFeeDetails oEditFeeDetails = new EditFeeDetails
                    {
                        AmountPaid = aoSqlDataReader["Paid"].ToInt(),
                        PaidLateFee = aoSqlDataReader["PaidLateFee"].ToInt(),
                        Payble = aoSqlDataReader["Payble"].ToInt(),
                        ApplicableLateFee = aoSqlDataReader["ApplicableLateFee"].ToInt(),
                        Concession = aoSqlDataReader["Concession"].ToInt(),
                        oStudentPayFeeDetails = oStudentPayFeeDetail,
                        IsCautionMoneyAdjusted = aoSqlDataReader["IsCautionMoneyAdjusted"].ToBool(),
                        FileName = aoSqlDataReader["FileName"].ToString(),
                    };

                    moEditFeeDetails = oEditFeeDetails;
                    break;
                }
            }
        }

        /// <summary>
        /// This method is used to initialize the student pay fee details. It is used for the local purpose.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentPaidFeeDetails> FillStudentPayFeeDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentPayFeeDetails> lstStudentPayFeeDetails = new List<StudentPayFeeDetails>();
            List<StudentPaidFeeDetails> lstStudentPaidFeeDetails = new List<StudentPaidFeeDetails>();
            while (aoSqlDataReader.Read())
            {
                StudentPaidFeeDetails oStudentPaidFeeDetails = new StudentPaidFeeDetails
                {
                    SchoolwiseStudentFeeId = aoSqlDataReader["Schoolwise_Student_Fee_Id"].ToInt(),
                    PayableFor = aoSqlDataReader["Payable_For"].ToString(),
                    Amount = aoSqlDataReader["Amount"].ToInt(),
                    AmountPayable = aoSqlDataReader["Amount_Payable"].ToInt(),
                    FeeType = aoSqlDataReader["Fee_Type"].ToString(),
                    DebitOrCredit = aoSqlDataReader["D/C"].ToString(),
                    LateFeeAmount = aoSqlDataReader["Late_Fee_Amt"].ToInt(),
                    SerialNumber = (!aoSqlDataReader["Serial_Number"].IsNull() ? aoSqlDataReader["Serial_Number"].ToString() : string.Empty),
                    StandardwiseFeeTypeId = (!aoSqlDataReader["Std_Fee_Type_Id"].IsNull() ? aoSqlDataReader["Std_Fee_Type_Id"].ToInt() : Constants.I_ONE),
                    ConcessionAmount = aoSqlDataReader["ConcessionAmount"].ToInt(),
                    AccountHeaderId = aoSqlDataReader["AccountHeaderId"].ToInt()
                };
                lstStudentPaidFeeDetails.Add(oStudentPaidFeeDetails);

                StudentPayFeeDetails oStudentPayFeeDetails = new StudentPayFeeDetails
                {
                    SchoolwiseStudentFeeId = aoSqlDataReader["Schoolwise_Student_Fee_Id"].ToInt(),
                    PaymentDate = (!aoSqlDataReader["DueDate"].IsNull() ? aoSqlDataReader["DueDate"].ToDateTime() : DateTime.Now),
                    ReceiptNumberOutput = (!aoSqlDataReader["Receipt_Number"].IsNull() ? aoSqlDataReader["Receipt_Number"].ToInt() : Constants.I_ZERO),                    
                    RemainingCautionMoney = aoSqlDataReader["CautionMoneyRemainingAmount"].ToInt()
                };
                lstStudentPayFeeDetails.Add(oStudentPayFeeDetails);
            }

            mlstStudentPayFeeDetails = lstStudentPayFeeDetails;
            return lstStudentPaidFeeDetails;
        }

        /// <summary>
        /// This method is used to get all the incomplete fee transaction from accessed from Incomplete transaction ui.
        /// </summary>
        /// <param name="asRegNumber"></param>
        /// <param name="asTransactionDate"></param>
        /// <returns></returns>
        private static string CreateFilter(string asRegNumber, DateTime asTransactionDate, string asPaymentCategoryFeeId)
        {
            StringBuilder sFilter = new StringBuilder();

            if (asRegNumber != string.Empty)
            {
                if (asPaymentCategoryFeeId == "1")
                    sFilter.Append(" AND (SchoolWise_Student_Master.Enrolment_Number LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%' OR SchoolWise_Student_Master.First_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + " OR SchoolWise_Student_Master.Middle_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + "OR  Schoolwise_Student_Fee_Details.NetBankingPaymentTransactionID LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + " OR SchoolWise_Student_Master.Last_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%')");
                else if(asPaymentCategoryFeeId == "2")
                    sFilter.Append(" AND (SchoolWise_Student_Master.Enrolment_Number LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%' OR SchoolWise_Student_Master.First_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + " OR SchoolWise_Student_Master.Middle_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + "OR  Student_Caution_Money_Details.NetBankingPaymentTransactionID LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + " OR SchoolWise_Student_Master.Last_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%')");
                else if(asPaymentCategoryFeeId == "3")
                    sFilter.Append(" AND (SchoolWise_Student_Master.Enrolment_Number LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%' OR SchoolWise_Student_Master.First_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + " OR SchoolWise_Student_Master.Middle_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + "OR  InternalFeeDetails.NetBankingPaymentTransactionID LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%'" + " OR SchoolWise_Student_Master.Last_Name LIKE '%" + StringUtility.ReplaceSingleQuoteInString(asRegNumber, true) + "%')");
            }

            if (asTransactionDate != null && asTransactionDate != DateTime.MinValue)
                sFilter.Append(" AND CONVERT(DATE, NetBankingPaymentTransactions.TransactionDateTime) = N'" + asTransactionDate + "'");

            return sFilter.ToString();
        }

        #endregion

     

        public DataTable GetFinalAcademicYearDetails(int aiSchoolId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetFinalAcademicYearDetails");
            }
        }

        /// <summary>
        /// This method is used to return incomplete transaction details.
        /// </summary>
        /// <returns></returns>
        public static List<IncompleteTransaction> GetAllIncomplteTransactions()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetIncompleteTransactionForService"))
                {
                    List<IncompleteTransaction> lstTransactions = new List<IncompleteTransaction>();
                    while (oSqlDataReader.Read())
                    {
                        lstTransactions.Add
                            (
                                new IncompleteTransaction
                                {
                                    EnrolmentNumber = Convert.ToString(oSqlDataReader["EnrolmentNumber"]),
                                    NetBankingPaymentTransactionId = Convert.ToInt32(oSqlDataReader["NetBankingPaymentTransactionId"]),
                                    SchoolId = Convert.ToInt32(oSqlDataReader["SchoolId"]),
                                    SchoolName = Convert.ToString(oSqlDataReader["SchoolName"]),
                                    StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                                    TransactionAMT = Convert.ToDecimal(oSqlDataReader["TransactionAMT"]),
                                    TransactionDateTime = Convert.ToDateTime(oSqlDataReader["TransactionDateTime"]),
                                    TransactionType = Convert.ToString(oSqlDataReader["TransactionType"]),
                                    TransactionTypeId = Convert.ToInt32(oSqlDataReader["TransactionTypeId"]),
                                    GatewayId = Convert.ToInt32(oSqlDataReader["GatewayId"]),
                                    StudentId = Convert.ToInt32(oSqlDataReader["StudentId"]),
                                    AcademicYearId = Convert.ToInt32(oSqlDataReader["AcademicYearId"])
                                }
                            );
                    }

                    return lstTransactions;
                }
            }
        }
        /// <summary>
        /// This method is used to update service execution details.
        /// </summary>
        public static void UpdateServiceExecutionDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateServiceExecutionDetails");
            }
        }

        public decimal GetFullPaymentConcessionAmount(int aiTotalAmount, bool abIsNewStudent, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsNewStudent", abIsNewStudent, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TotalAmount", aiTotalAmount, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("ConcessionAmount", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetFullPaymentConcessionAmount");
                return oSqlParameter.Value.ToDecimal();
            }
        }

        public string GetConcessionMessage(int aiStandardId, bool abIsForStudentLogin)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsForStudentLogin", abIsForStudentLogin, SqlDbType.Bit);
                SqlParameter  oSqlParameter = oSQLServerDbUtility.AddParameter("ConcessionMessage", string.Empty, SqlDbType.NVarChar,ParameterDirection.Output,300);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetConcessionAmountMessage");
                return oSqlParameter.Value.ToString();
            }
        }

        public List<FeeDetailsToExport> GetFeeDetailsToExport(int aiStandardId, int aiDivisionId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeedetailsToExport"))
                {
                    List<FeeDetailsToExport> lstFeeDetails = new List<FeeDetailsToExport>();
                    while (oSqlDataReader.Read())
                    {
                        FeeDetailsToExport oFeeDetailsToExport = new FeeDetailsToExport
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            StudentId = Convert.ToInt32(oSqlDataReader["StudentId"]),
                            Field = Convert.ToString(oSqlDataReader["Field"]),
                            Value = Convert.ToString(oSqlDataReader["Value"]),
                            IsCredit = Convert.ToBoolean(oSqlDataReader["IsCredit"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            ParentId = Convert.ToInt32(oSqlDataReader["ParentId"]),
                            SerialNo = Convert.ToInt32(oSqlDataReader["SerialNo"]),
                            RowNo = Convert.ToInt32(oSqlDataReader["RowNo"])
                            //TransactionNumber = oSqlDataReader["TransactionNumber"].ToString()
                        };
                        lstFeeDetails.Add(oFeeDetailsToExport);
                    }
                    return lstFeeDetails;
                }
            }
        }

        public List<FeeLedger> GetAllFeeLedgers(int aiStudentId,int aiStandardId,int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLedgersForfeeExport"))
                {
                    List<FeeLedger> lstLedgers = new List<FeeLedger>();
                    while (oSqlDataReader.Read())
                    {
                        lstLedgers.Add(new FeeLedger { Name = Convert.ToString(oSqlDataReader["Name"]), SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"]) });
                    }
                    return lstLedgers;
                }
            }
        }

        public int GetStudentDetails(int aiSchoolId, int aiAcademicYearId, string asReceiptNo)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiptNo", asReceiptNo, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("YearwiseStudentId", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetStudentDetailsFromReceipt");
                return oSqlParameter.Value.ToInt();
            }
        }

        public DataSet GetDebitDetails(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeInternalFee", true, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StudentID", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetDebitDetails");
            }
        }

        public DataTable GetFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiMode, bool abIsInternalFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Mode", aiMode, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetModeFeeDetails");
            }
        }
        /// <summary>
        /// This method is used to return fee details.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<Student> GetStudentAllFeesDetails(int aiStandardId, int aiDivisionId, string aiStartDate, string aiEndDate)  ////
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartDate", aiStartDate, SqlDbType.DateTime);  ////
                oSQLServerDbUtility.AddParameter("EndDate", aiEndDate, SqlDbType.DateTime);  ////
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllFeeDtailsForReport");
                List<Student> lstStudents = GetStudentList(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillIntervals(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillFeePaymentDetails(oSqlDataReader);

                return lstStudents;
            }
        }

        /// <summary>
        /// This method is used to fill fee payment details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillFeePaymentDetails(SqlDataReader aoSqlDataReader)
        {
            mlstFeeDetails = new List<PayableForDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstFeeDetails.Add
                    (
                        new PayableForDetails
                        {
                            StudentId = Convert.ToInt32(aoSqlDataReader["Student_Id"]),
                            PayableFor = Convert.ToString(aoSqlDataReader["Payable_For"]),
                            Amount = Convert.ToInt32(aoSqlDataReader["Amount"]),
                            FeeType = Convert.ToString(aoSqlDataReader["FeeType"]),
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill interval details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillIntervals(SqlDataReader aoSqlDataReader)
        {
            mlstIntervals = new List<PayableForDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstIntervals.Add
                    (
                        new PayableForDetails
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            PayableFor = Convert.ToString(aoSqlDataReader["IntervalName"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                            FeeType = Convert.ToString(aoSqlDataReader["FeeType"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<Student> GetStudentList(SqlDataReader aoSqlDataReader)
        {
            List<Student> lstStudents = new List<Student>();
            while (aoSqlDataReader.Read())
            {
                lstStudents.Add
                    (
                        new Student
                        {
                            YearWiseStudentId = Convert.ToInt32(aoSqlDataReader["Yearwise_Student_Id"]),
                            ClassName = Convert.ToString(aoSqlDataReader["ClassName"]),
                            RollNo = Convert.ToInt32(aoSqlDataReader["Roll_No"]),
                            Name = Convert.ToString(aoSqlDataReader["StudentName"]),
                            OriginalStandardId = Convert.ToInt32(aoSqlDataReader["Original_Standard_Id"]),
                            OriginalDivisionId = Convert.ToInt32(aoSqlDataReader["Original_Division_Id"]),
                            MobileNumber = aoSqlDataReader["Mobile_Number"].ToString(),
                            RegistraionNo = aoSqlDataReader["Enrolment_Number"].ToString(),
                            TotalPayable = aoSqlDataReader["TotalPayable"].ToInt()
                        }
                    );
            }
            return lstStudents;
        }

        public DataSet GetLastYearFeeDetails(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId",aiSchoolId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
               return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetLastYearFeeDetails");
            }
        }

        public bool IsLastYearPendingFeeExist(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                DataTable DT = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetLastYearFeePendingStatus");
                if (DT.Rows.Count > 0 && DT.Rows[0][0] != DBNull.Value && DT.Rows[0][0].ToString() == Constants.S_ONE)
                    return true;
                else
                    return false;
            }
        }

        public void DisableOrDeleteUnpaidFee(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, string asSerialNumber, Boolean abIsDisable, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SerialNumber", asSerialNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsDisable", abIsDisable, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);                
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_DisableOrDeleteUnpaidFee");
            }
        }

        public PaidFeeDetails GetAllFeeDetailsForVP(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asStandardDivisionId, int aiStudentId, DateTime adFromDate, DateTime adToDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                PaidFeeDetails oPaidFeeDetails = new PaidFeeDetails();
                
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", asStandardId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Division_Id", asStandardDivisionId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Student_Id", aiStudentId, SqlDbType.Int);
                if (adFromDate != DateTime.MinValue)
                oSQLServerDbUtility.AddParameter("FromDate", adFromDate, SqlDbType.DateTime);
                if (adToDate != DateTime.MinValue)
                oSQLServerDbUtility.AddParameter("ToDate", adToDate, SqlDbType.DateTime);

                List<StudentFeeDetailsList> lstStudentFeeDetailsList = new List<StudentFeeDetailsList>();
                List<StudentCautionMoneyDetailsList> lstStudentCautionMoneyDetailsList = new List<StudentCautionMoneyDetailsList>();
                List<StudentFeeTypeConfigurationDetailsList> lstStudentFeeTypeConfigurationDetailsList = new List<StudentFeeTypeConfigurationDetailsList>();

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeDetailsForVP"))
                {
                    List<StudentDetailsList> lstStudentDetails = new List<StudentDetailsList>();
                    StudentDetailsList oStudentDetailsList;
                    while (oSqlDataReader.Read())
                    {
                        oStudentDetailsList = new StudentDetailsList();
                        oStudentDetailsList.StudentId = oSqlDataReader["YearWise_Student_Id"].ToInt();
                        oStudentDetailsList.SchoolwiseStudentId = oSqlDataReader["Student_Id"].ToInt();
                        oStudentDetailsList.OriginalStandardId = oSqlDataReader["Original_Standard_Id"].ToInt();
                        oStudentDetailsList.OriginalDivisionId = oSqlDataReader["Original_Division_Id"].ToInt();
                        oStudentDetailsList.RollNo = oSqlDataReader["Roll_No"].ToInt();
                        oStudentDetailsList.Class = oSqlDataReader["className"].ToString();
                        oStudentDetailsList.StudentName = oSqlDataReader["StudentName"].ToString();

                        lstStudentDetails.Add(oStudentDetailsList);
                    }

                    oSqlDataReader.NextResult();
                    StudentFeeDetailsList oStudentFeeDetailsList;
                    while (oSqlDataReader.Read())
                    {
                        oStudentFeeDetailsList = new StudentFeeDetailsList();
                        oStudentFeeDetailsList.StudentId = oSqlDataReader["StudentId"].ToInt();
                        oStudentFeeDetailsList.PaidDate = oSqlDataReader["PaidDate"].ToDateTime();
                        oStudentFeeDetailsList.ReceiptNumber = oSqlDataReader["ReceiptNo"].ToString();
                        oStudentFeeDetailsList.FeeType = oSqlDataReader["FeeType"].ToString();
                        oStudentFeeDetailsList.Amount = oSqlDataReader["Amount"].ToInt();
                        oStudentFeeDetailsList.TransactionNumber = oSqlDataReader["TransactionNo"].ToString();
                        oStudentFeeDetailsList.PaymentMode = oSqlDataReader["PaymentMode"].ToString();

                        lstStudentFeeDetailsList.Add(oStudentFeeDetailsList);
                    }

                    oSqlDataReader.NextResult();
                    StudentCautionMoneyDetailsList oStudentCautionMoneyDetailsList;
                    while (oSqlDataReader.Read())
                    {
                        oStudentCautionMoneyDetailsList = new StudentCautionMoneyDetailsList();
                        oStudentCautionMoneyDetailsList.SchoolwiseStudentId = oSqlDataReader["Schoolwise_Student_Id"].ToInt();
                        oStudentCautionMoneyDetailsList.CautionMoneyAmount = oSqlDataReader["Amount"].ToInt();

                        lstStudentCautionMoneyDetailsList.Add(oStudentCautionMoneyDetailsList);
                    }

                    oSqlDataReader.NextResult();
                    StudentFeeTypeConfigurationDetailsList oStudentFeeTypeConfigurationDetailsList;
                    while (oSqlDataReader.Read())
                    {
                        oStudentFeeTypeConfigurationDetailsList = new StudentFeeTypeConfigurationDetailsList();
                        oStudentFeeTypeConfigurationDetailsList.FeeTypeId = oSqlDataReader["Original_Fee_Type_Id"].ToInt();
                        oStudentFeeTypeConfigurationDetailsList.FeeType = oSqlDataReader["Fee_Type"].ToString();

                        lstStudentFeeTypeConfigurationDetailsList.Add(oStudentFeeTypeConfigurationDetailsList);
                    }

                    oPaidFeeDetails.StudentDetailsList = lstStudentDetails;
                    oPaidFeeDetails.StudentFeeDetailsList = lstStudentFeeDetailsList;
                    oPaidFeeDetails.StudentCautionMoneyDetailsList = lstStudentCautionMoneyDetailsList;
                    oPaidFeeDetails.StudentFeeTypeConfigurationDetailsList = lstStudentFeeTypeConfigurationDetailsList;
                }
                return oPaidFeeDetails;
            }
        }

        /// <summary>
        /// This method is used for get internal paid exam fee details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public InternalPaidFeeExamDetails GetCompetitiveExamwiseDetails(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asDivisionId)
         {
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                 InternalPaidFeeExamDetails oInternalPaidFeeExamDetails = new InternalPaidFeeExamDetails();
                 oInternalPaidFeeExamDetails.StudentList = new List<StudentItem>();
                 oInternalPaidFeeExamDetails.DebitPayables = new List<PayableItem>();
                 oInternalPaidFeeExamDetails.CreditEntries = new List<CreditItem>();

                 oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("Standard_Id", asStandardId, SqlDbType.NVarChar);
                 oSQLServerDbUtility.AddParameter("Division_Id", asDivisionId, SqlDbType.NVarChar);

                 using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ExportCompetitiveExamwiseDetails"))
                 {
                     // Resultset 1: Student List
                     while (oSqlDataReader.Read())
                     {
                         oInternalPaidFeeExamDetails.StudentList.Add(new StudentItem
                         {
                             SchoolwiseStudentId = oSqlDataReader["SchoolwiseStudentId"].ToInt(),
                             RollNo = oSqlDataReader["RollNo"].ToInt(),
                             FirstName = oSqlDataReader["FirstName"].ToString(),
                             MiddleName = oSqlDataReader["MiddleName"].ToString(),
                             LastName = oSqlDataReader["LastName"].ToString(),
                             MobileNumber = oSqlDataReader["MobileNumber"].ToString(),
                             ClassName = oSqlDataReader["ClassName"].ToString()
                         });
                     }

                     // Resultset 2: Debit Payable_For
                     if (oSqlDataReader.NextResult())
                     {
                         while (oSqlDataReader.Read())
                         {
                             oInternalPaidFeeExamDetails.DebitPayables.Add(new PayableItem
                             {
                                 PayableFor = oSqlDataReader["Payable_For"].ToString()
                             });
                         }
                     }

                     // Resultset 3: Credit Entries
                     if (oSqlDataReader.NextResult())
                     {
                         while (oSqlDataReader.Read())
                         {
                             oInternalPaidFeeExamDetails.CreditEntries.Add(new CreditItem
                             {
                                 SchoolwiseStudentId = oSqlDataReader["Schoolwise_Student_Id"].ToInt(),
                                 PayableFor = oSqlDataReader["Payable_For"].ToString()
                             });
                         }
                     }
                 }

                 return oInternalPaidFeeExamDetails;
             }
         }
        /// <summary>
        /// these method is used for get pending student count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asStandardId"></param>
        /// <param name="asDivisionId"></param>
        /// <returns></returns>
        public StudentsAcademicYearwisePendingFeeCountDetails GetYearwisePendingFeeStudent(int aiSchoolId, int aiAcademicYearId, string asStandardId, string asDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentsAcademicYearwisePendingFeeCountDetails oStudentsYearwisePendingfeecount = new StudentsAcademicYearwisePendingFeeCountDetails();
                oStudentsYearwisePendingfeecount.AcademicYears = new List<AcademicYearDetails>();
                oStudentsYearwisePendingfeecount.StudentCounts = new List<StudentCount>();
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", asStandardId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Division_Id", asDivisionId, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentsYearwisePendingFeeCount"))
                {                    
                    while (oSqlDataReader.Read())
                    {
                        oStudentsYearwisePendingfeecount.AcademicYears.Add(new AcademicYearDetails
                        {
                            AcademicYearId = oSqlDataReader["Academic_Year_ID"].ToInt(),
                            AcademicYearName = oSqlDataReader["AcademicYear"].ToString()
                        });
                    }

                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            oStudentsYearwisePendingfeecount.StudentCounts.Add(new StudentCount
                            {
                                CategoryId = oSqlDataReader["CategoryId"].ToInt(),
                                AcademicYearId = oSqlDataReader["AcademicYearId"].ToInt(),
                                Count = oSqlDataReader["Count"].ToInt(),
                            });
                        }
                    }

                    return oStudentsYearwisePendingfeecount;
                }
            }
        }
               
        /// <summary>
        /// Gets NetBankingPaymentTransactionID and TPSLTransactionID for ids in TxnIds XML (usp_GetTransactionDetails).
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asTxnIds">XML: TransactionIdList/Transaction/@NetBankingPaymentTransactionID</param>
        /// <returns></returns>
        public DataTable GetTransactionDetails(int aiSchoolId,int aiFinancialYearId, string asTxnIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TxnIds", asTxnIds, SqlDbType.Xml);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTransactionDetails");
            }
        }
    }

    public class StudentFeeDetailsCollectionDC
    {

        #region Public Methods

        /// <summary>
        /// This method is used to update all debit entries if any fee type get changed
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <param name="aiAmount"></param>
        /// <param name="abIsStudentPayFee"></param>
        public static void UpdateDebitEntries(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiFeeTypeId, int aiAmount, bool abIsStudentPayFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iFeeTypeId", aiFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAmount", aiAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsStudentPayFee", abIsStudentPayFee, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateFeeDetailsForStandardFeeTypeChange");
            }
        }

        /// <summary>
        /// This method is used to update fees if any fees against selected fee type gets increased or decreased.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <param name="aiAmount"></param>
        /// <param name="abIsStudentPayFee"></param>
        /// <param name="adDueDate"></param>
        /// <param name="aiAmountForNewStudent"></param>
        /// <param name="aiAmountForOldStudent"></param>
        public static void UpdateDebitEntries(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiFeeTypeId, int aiAmount, bool abIsStudentPayFee, DateTime adDueDate, int aiAmountForNewStudent, int aiAmountForOldStudent)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iFeeTypeId", aiFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAmount", aiAmount, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsStudentPayFee", abIsStudentPayFee, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("DueDate", adDueDate.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AmountForNewStudent", aiAmountForNewStudent, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AmountForOldStudent", aiAmountForOldStudent, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateFeeDetailsForStandardFeeTypeChange");
            }
        }

        /// <summary>
        /// Get fee summary details to show in fee status widget.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static FeeSummary GetFeeSummary(int aiSchoolId, int aiAcademicYearId, bool abIsServiceCall = false)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId , Constants.I_ZERO, abIsServiceCall))
            {
                FeeSummary oFeeSummary = new FeeSummary();
                string strAmountFormat = "{0:#,###,###.##}";// format amount with comma.

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeSummary"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oFeeSummary = new FeeSummary()
                        {
                            AmountExpectedToReceive = oSqlDataReader["AmountExpectedToReceive"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["AmountExpectedToReceive"])) : Constants.S_ZERO,
                            Concession = oSqlDataReader["Concession"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["Concession"])) : Constants.S_ZERO,
                            DuesTillDate = oSqlDataReader["DuesTillDate"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["DuesTillDate"])) : Constants.S_ZERO,
                            TodaysCollection = oSqlDataReader["TodaysCollection"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["TodaysCollection"])) : Constants.S_ZERO,
                            TotalPaidFees = oSqlDataReader["TotalPaidFees"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["TotalPaidFees"])) : Constants.S_ZERO,
                        };
                    }
                }

                return oFeeSummary;
            }
        }

        #endregion
    }
}