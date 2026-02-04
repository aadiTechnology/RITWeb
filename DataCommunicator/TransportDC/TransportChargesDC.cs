/*File Name - TransportChargesDC.cs
 * Created By - Pravin Shinde
 * Created Date - 26 Dec 2013
 * Description - This class is used to search/pay/refund transport charges of user.
 */
namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Data.SqlClient;
    using Utility;
    using SchoolEntities;
    using SchoolEntities.Transport;
    using SchoolEntities.StudentFee;

    /// <summary>
    /// This class is used to search/pay/refund transport charges of user.
    /// </summary>
    public class TransportChargesDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;        
        private int miInsertedById;
        
        #endregion

        #region Construstor(s)

        public TransportChargesDC()
        {
        }

        public TransportChargesDC(int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miInsertedById = aiInsertedById;
        }
        
        #endregion

        #region Properties(s)

       
        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get the transport user details for selected role & criteria.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asName"></param>
        /// <param name="asRole"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public List<TransportFeeDetails> GetUserDetails(int aiSchoolId, int aiAcademicYearId, string asName, string asRole, String sortExpression, int iEndIndex, int iStartIndex)
        {
            List<TransportFeeDetails> lstTransportFeeDetail = new List<TransportFeeDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", StringUtility.ReplaceSingleQuoteInString(asName, true), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RoleId", asRole.ToInt(), SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllTransportFeeDetails"))
                {
                    while(oSqlDataReader.Read())
                    {
                        TransportFeeDetails oTransportFeeDetails = new TransportFeeDetails
                        {
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            Name = oSqlDataReader["Name"].ToString(),
                            PendingAmount = oSqlDataReader["PendingAmount"].ToInt(),
                            TotalAmount = oSqlDataReader["TotalAmount"].ToInt(),
                            HasRefund = oSqlDataReader["HasRefund"].ToBool(),
                            IsDeactivated = oSqlDataReader["IsDeactivated"].ToBool()
                        };

                        lstTransportFeeDetail.Add(oTransportFeeDetails);
                    }                    
                }
            }

            return lstTransportFeeDetail;
        }

        /// <summary>
        /// This method is used to get count of the transport user details for selected role & criteria.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asName"></param>
        /// <param name="asRole"></param>
        /// <returns></returns>
        public int CountUsers(int aiSchoolId, int aiAcademicYearId, string asName, string asRole)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Name", StringUtility.ReplaceSingleQuoteInString(asName, true), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RoleId", asRole.ToInt(), SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountAllTransportFeeDetails");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This function is used to get the transport charges details of selected role and mode.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aodtCurrentDate"></param>
        /// <param name="abIsForRefund"></param>
        /// <returns></returns>
        public List<PayTransportCharges> GetAll(int aiUserId,DateTime aodtCurrentDate, bool abIsForRefund)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CurrentDate", aodtCurrentDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("IsForRefund", abIsForRefund, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTransportFeeDetails"))                   
                    return FillTrasportCharges(oSqlDataReader);                    
            }
        }

        /// <summary>
        /// This method is used to pay transport charges.
        /// </summary>
        /// <param name="asTransportDetailsXML"></param>
        public void Insert(string asTransportDetailsXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TransportDetailsXML", asTransportDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Transport.usp_PayTransportFee");                
            }
        }

        /// <summary>
        /// This method is used to refund transport charges for selected role.
        /// </summary>
        /// <param name="asTransportFeeId"></param>
        /// <param name="aodtRefundDate"></param>
        public void RefundCharges(string asTransportFeeId, DateTime aodtRefundDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TransportFeeId", asTransportFeeId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RefundDate", aodtRefundDate, SqlDbType.DateTime);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Transport.usp_RefundTransportFee");
            }
        }

        /// <summary>
        /// This function is used to delete paid transport charges.
        /// </summary>
        /// <param name="asReceiptNumber"></param>
        /// <param name="abIsRefund"></param>
        public void Delete(string asReceiptNumber,bool abIsRefund)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiptNumber", asReceiptNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsRefund", abIsRefund, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("Transport.usp_DeleteTransportFees");
            }
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This function is used to fill transport charges in the list. It is used for the local purpose.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<PayTransportCharges> FillTrasportCharges(SqlDataReader aoSqlDataReader)
        {
            List<PayTransportCharges> lstPayTransportCharge = new List<PayTransportCharges>();

            while (aoSqlDataReader.Read())
            {
                PayTransportCharges oPayTransportCharges = new PayTransportCharges
                {
                    TransportFeeDetailsId = aoSqlDataReader["TransportFeeId"].ToInt(),
                    MonthName = aoSqlDataReader["MonthName"].ToString(),
                    IsArrears = aoSqlDataReader["IsArrear"].ToBool(),
                    IsRefund = aoSqlDataReader["IsRefund"].ToBool(),
                    IsAutoRefund = aoSqlDataReader["IsAutoRefund"].ToBool(),
                    IsConcession = aoSqlDataReader["IsConcession"].ToBool(),
                    IsLastCredit = aoSqlDataReader["IsLastCredit"].ToBool(),
                    oStudentPaidFeeDetails = new StudentPaidFeeDetails
                    {
                        PayableFor = aoSqlDataReader["PayableFor"].ToString(),
                        Amount = aoSqlDataReader["Amount"].ToInt(),
                        DebitOrCredit = aoSqlDataReader["DebitCredit"].ToString(),
                        LateFeeAmount = aoSqlDataReader["LateFeeAmt"].ToInt(),
                        SerialNumber = aoSqlDataReader["SerialNumber"].ToString()
                    },
                    oStudentPayFeeDetails = new StudentPayFeeDetails
                    {
                        PaymentDate = aoSqlDataReader["PaidDate"].ToDateTime(),
                        ReceiptNumberOutput = aoSqlDataReader["ReceiptNumber"].ToInt()
                    }
                };

                lstPayTransportCharge.Add(oPayTransportCharges);
            }

            return lstPayTransportCharge;
        }

        #endregion
    }
}
