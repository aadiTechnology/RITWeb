using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.StudentFee.FeeReport;
using Utility;

namespace DataCommunicator
{
    public class FeeReportDC
    {
        #region Data Member(s)
        
        private int miSchoolId;
        private int miAcademicYearId;
        private FeeReport moFeeReport; 

        #endregion

        #region Constructor(s)


        public FeeReportDC()
        {
        }

        public FeeReportDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        } 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return fee details to export it.
        /// </summary>
        /// <param name="aiStdId"></param>
        /// <param name="aiDivId"></param>
        /// <returns></returns>
        public FeeReport GetFeeDetailsForReport(int aiStdId, int aiDivId)
        {
            moFeeReport = new FeeReport();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivId", aiDivId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeDetailsForReportAaryan"))
                {
                    LoadFeeTypes(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        FillFeeDetails(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        FillPayableSummmary(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        FillPaidFeeDetails(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        FillTransportDetails(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        FillStudentInfo(oSqlDataReader);

                    return moFeeReport;
                }
            }
        }

        private void FillPayableSummmary(SqlDataReader aoSqlDataReader)
        {
            moFeeReport.PayableSummaryDetails = new List<PayableSummary>();

            while (aoSqlDataReader.Read())
            {
                moFeeReport.PayableSummaryDetails.Add(
                    new PayableSummary
                    {
                        PayableFor = aoSqlDataReader["Payable_For"].ToString(),
                        YearwiseStudentId = aoSqlDataReader["Student_Id"].ToInt(),
                        TotalAmount = aoSqlDataReader["TotalAmount"].ToInt()
                    }
                    );
            }
        }

        #endregion

        #region Private Method(s)
        
        private void FillStudentInfo(SqlDataReader aoSqlDataReader)
        {
            moFeeReport.StudentInfo = new List<StudentInfo>();
            while (aoSqlDataReader.Read())
            {
                moFeeReport.StudentInfo.Add(
                    new StudentInfo
                    {
                        Class = aoSqlDataReader["ClassName"].ToString(),
                        EnrolmentNo = aoSqlDataReader["Enrolment_Number"].ToString(),
                        StudentName = aoSqlDataReader["StudentName"].ToString(),
                        FeeCategory = aoSqlDataReader["FeeCategory"].ToString(),
                        Status = aoSqlDataReader["Status"].ToString(),
                        UserId = aoSqlDataReader["UserId"].ToInt(),
                        YearwiseStudentId = aoSqlDataReader["Yearwise_Student_Id"].ToInt(),
                        OrgStdId = aoSqlDataReader["Original_Standard_Id"].ToInt(),
                        OrdDivId = aoSqlDataReader["Original_Division_Id"].ToInt(),
                        RollNo = aoSqlDataReader["Roll_No"].ToInt()
                    }
                    );
            }
        }

        private void FillTransportDetails(SqlDataReader aoSqlDataReader)
        {
            moFeeReport.TransportDetails = new List<TransportDetails>();
            while (aoSqlDataReader.Read())
            {
                moFeeReport.TransportDetails.Add(
                    new TransportDetails
                    {
                        PickupRoute = aoSqlDataReader["PickupRoute"].ToString(),
                        PickupStop = aoSqlDataReader["PickupStop"].ToString(),
                        DropRoute = aoSqlDataReader["DropRoute"].ToString(),
                        DropStop = aoSqlDataReader["DropStop"].ToString(),
                        UserId = aoSqlDataReader["UserId"].ToInt()
                    }
                    );
            }
        }

        private void FillPaidFeeDetails(SqlDataReader aoSqlDataReader)
        {
            moFeeReport.PaidFeeDetails = new List<PaidFeeDetails>();
            while (aoSqlDataReader.Read())
            {
                moFeeReport.PaidFeeDetails.Add(
                    new PaidFeeDetails
                    {
                        ReceiptNumber = aoSqlDataReader["Receipt_Number"].ToString(),
                        AdditionalRemark = aoSqlDataReader["AdditionalRemark"].ToString(),
                        YearwiseStudentId = aoSqlDataReader["YearwiseStudentId"].ToInt(),
                        Amount = aoSqlDataReader["Amount"].ToInt(),
                        StudentFeeId = aoSqlDataReader["Student_Fee_Id"].ToInt(),
                        TransactionId = aoSqlDataReader["TransactionId"].ToString(),
                        PaidDate = aoSqlDataReader["Paid_Date"].ToDateTime(),
                        ChequeDate = aoSqlDataReader["Cheque_Date"].ToDateTime(),
                        PaymentMode = aoSqlDataReader["PaymentMode"].ToString(),
                        BankName = aoSqlDataReader["BankName"].ToString(),
                        CreatedBy = aoSqlDataReader["CreatedBy"].ToString(),
                        FeeType = aoSqlDataReader["FeeType"].ToString(),
                        PayableFor = aoSqlDataReader["PayableFor"].ToString(),
                        ConcessionAmount = aoSqlDataReader["ConcessionAmount"].ToInt()
                    }
                    );
            }
        }

        private void FillFeeDetails(SqlDataReader aoSqlDataReader)
        {
            moFeeReport.SchooolwiseStudentFeeDetailss = new List<SchooolwiseStudentFeeDetailss>();
            while (aoSqlDataReader.Read())
            {
                moFeeReport.SchooolwiseStudentFeeDetailss.Add(
                    new SchooolwiseStudentFeeDetailss
                    {
                        FeeType = aoSqlDataReader["Fee_Type"].ToString(),
                        PayableFor = aoSqlDataReader["Payable_For"].ToString(),
                        YearwiseStudentId = aoSqlDataReader["Yearwise_Student_Id"].ToInt(),
                        Amount = aoSqlDataReader["Amount"].ToInt(),
                        SchoolwiseStudentFeeId = aoSqlDataReader["Schoolwise_Student_Fee_Id"].ToInt()
                    }
                    );
            }
        }

        private void LoadFeeTypes(SqlDataReader aoSqlDataReader)
        {
            moFeeReport.FeeTypes = new List<FeeType>();
            while (aoSqlDataReader.Read())
            {
                moFeeReport.FeeTypes.Add(
                    new FeeType
                    {
                        Name = aoSqlDataReader["Fee_Type"].ToString(),
                        OrgFeeTypeId = aoSqlDataReader["Original_Fee_Type_Id"].ToInt()
                    }
                    );
            }
        } 

        #endregion
    }
}
