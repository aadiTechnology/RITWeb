// Class Name       :- NetBankingPaymentTransactionsDC
// Purpose          :- This class is used to manage NetBankingPaymentTransactions details.
// Date Of creation :- 11/11/2009
// Author Name      :- 

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using SchoolEntities.Accounts;
using System.Data.SqlClient;
using Utility;
using SchoolEntities;
using FeeEntities;

namespace DataCommunicator
{


    public class NetBankingPaymentTransactionsDC
    {

        private NetBankingPaymentTransactionsStruct moNetBankingPaymentTransactionsStruct;

        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miStudentId;
        private string msMinMaxCharges;
        private string msServiceTaxes;

        #endregion

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        public NetBankingPaymentTransactionsDC(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miStudentId = aiStudentId;
        } 

        public NetBankingPaymentTransactionsDC()
        {
        }

        public NetBankingPaymentTransactionsDC(int aiSchoolId)
        {
             this.miSchoolId = aiSchoolId;
        }

        public NetBankingPaymentTransactionsDC(string asNetBankingPaymentTransactionID)
        {
            LoadNetBankingPaymentTransactionsDetails(asNetBankingPaymentTransactionID);
        }

        public virtual NetBankingPaymentTransactionsStruct NetBankingPaymentTransactionsStructDetails
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct = value;
            }
        }

        public string MinMaxCharge
        {
            get{return msMinMaxCharges;}
            set { msMinMaxCharges = value; }
        }

        public string ServiceTax
        {
            get{return msServiceTaxes;}
            set{msServiceTaxes = value;}
        }

        /// <summary>
        /// This method is used to pay current year fee online.
        /// </summary>
        /// <param name="aiLateFeeAmount"></param>
        /// <param name="asRemarks"></param>
        /// <param name="asLateFeeRemark"></param>
        /// <param name="asStudentFeeIdXML"></param>
        /// <param name="aoNetBankingTransaction"></param>
        /// <param name="bIsPayFromMobile"></param>
        /// <returns></returns>
        public DataTable PayStudentFeeOnLine(int aiLateFeeAmount, string asRemarks, string asLateFeeRemark, string asStudentFeeIdXML, NetBankingTransaction aoNetBankingTransaction, bool bIsPayFromMobile, bool abIsCautionMoneyPayment, bool abIsInternalFeePayment, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Remarks", asRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SessionId", aoNetBankingTransaction.PaymentITCParameter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TrnsAmount", aoNetBankingTransaction.TransactionAMT, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("BankId", aoNetBankingTransaction.TransactionBankID, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransactionStatus",Convert.ToChar(aoNetBankingTransaction.TransactionStatus), SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("GatewayId", aoNetBankingTransaction.GatewayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPayFromMobile", bIsPayFromMobile, SqlDbType.Bit);
                
                if (abIsCautionMoneyPayment)
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_PayStudentCautionMoneyOnline", true);
                else if (abIsInternalFeePayment)
                {
                    oSQLServerDbUtility.AddParameter("AcdYrId", aiAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StudentFeeIdsXML", asStudentFeeIdXML, SqlDbType.Xml);
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_PayStudentInternalFeeOnline", true);
                }
                else
                {
                    oSQLServerDbUtility.AddParameter("AcdYrId", miAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("LateFeeAmount", aiLateFeeAmount, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("LateFeeRemarks", asLateFeeRemark, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("StudentFeeIdsXML", asStudentFeeIdXML, SqlDbType.Xml);
                    oSQLServerDbUtility.AddParameter("ConcessionAmount", aoNetBankingTransaction.ConcessionAmount, SqlDbType.Int);
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_PayStudentFeeOnline", true);
                }
            }
        }

        /// <summary>
        /// This method is used to pay next year fee online.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="asRemarks"></param>
        /// <param name="asDueDatesFilterXML"></param>
        /// <param name="aiLateFeeAmount"></param>
        /// <param name="aoNetBankingTransaction"></param>
        /// <returns></returns>
        public DataTable PayStudentNextYearFeeOnLine(int aiStandardId, string asRemarks, string asDueDatesFilterXML, int aiLateFeeAmount, NetBankingTransaction aoNetBankingTransaction, bool abIsForIntrnalFee, string asInternalFeeDetailsIds, string asSelectedFeeType)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", miStudentId, SqlDbType.Int);  
                oSQLServerDbUtility.AddParameter("SessionId", aoNetBankingTransaction.PaymentITCParameter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TrnsAmount", aoNetBankingTransaction.TransactionAMT, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("BankId", aoNetBankingTransaction.TransactionBankID, SqlDbType.NVarChar);                
                oSQLServerDbUtility.AddParameter("GatewayId", aoNetBankingTransaction.GatewayId, SqlDbType.Int);                
                if (abIsForIntrnalFee)
                {
                    oSQLServerDbUtility.AddParameter("InternalFeeDetailsIds", asInternalFeeDetailsIds, SqlDbType.NVarChar);
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_PayNextAcademicYearInternalFeeOnline", true);
                }
                else
                {
                    oSQLServerDbUtility.AddParameter("Remarks", asRemarks, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("DuedateXml", asDueDatesFilterXML, SqlDbType.Xml);
                    oSQLServerDbUtility.AddParameter("iStandardId", aiStandardId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("ConcessionAmount", aoNetBankingTransaction.ConcessionAmount, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("LateFeeAmount", aiLateFeeAmount, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SelectedFeeType", asSelectedFeeType, SqlDbType.NVarChar);
                    
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_PayNextAcademicYearFeeOnline", true);
                }
            }
        }

        /// <summary>
        /// This method is used to delete the fee transaction from superadmin screen.
        /// </summary>
        /// <param name="aiTranscationId"></param>
        /// <param name="aiUserId"></param>
        public void DeleteTransactionDetails(int aiTranscationId, int aiUserId, Constants.OnlineFeeTypes aoOnlineFeeType)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TranscationId", aiTranscationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TransactionFor", aoOnlineFeeType.ToInt(), SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteTransactionDetails");
            }
        }

        public TransactionStatusDetails GetTransactionStatus(int aiSchoolId, int aiGatewayId, string asOrderId)
        {
            TransactionStatusDetails oTransactionStatusDetails = new TransactionStatusDetails();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("GatewayId", aiGatewayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OrderId", asOrderId, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTransactionOrderDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oTransactionStatusDetails.NetbankingTransactionId = oSqlDataReader["NetbankingTransactionId"].ToInt();
                        oTransactionStatusDetails.StatusCode = oSqlDataReader["StatusCode"].ToString();
                       // oTransactionStatusDetails.ErrorMessage = oSqlDataReader["ErrorMessage"].ToString();
                        oTransactionStatusDetails.Amount = oSqlDataReader["Amount"].ToInt();
                    }
                }
            }
            return oTransactionStatusDetails;
        }

        /// <summary>
        /// This is the common method to complete all types of incomplete online transactions.
        /// </summary>
        /// <param name="aiTranscationId"></param>
        /// <param name="asTPSLTransactionID"></param>
        /// <param name="aiTransactionFor"></param>
        /// <returns></rseturns>
        public string CompleteTransactionDetails(NetBankingTransaction aoNetBankingTransaction, int aiPaymentCategoryFeeId)
        {
            string sMessge = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TranscationId", aoNetBankingTransaction.NetBankingPaymentTransactionID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TPSLTransactionID", aoNetBankingTransaction.TPSLTransactionID, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransactionFor", aoNetBankingTransaction.TransactionFor, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TransactionStatus", Convert.ToChar(aoNetBankingTransaction.TransactionStatus), SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("Amount", aoNetBankingTransaction.TransactionAMT, SqlDbType.Float);
                oSQLServerDbUtility.AddParameter("BankCode", (aoNetBankingTransaction.TransactionBankID.IsNull() ? string.Empty : aoNetBankingTransaction.TransactionBankID), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PaymentCategoryFeeId", aiPaymentCategoryFeeId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_UpdatePendingTransactionStatus"))
                {
                    if (oSqlDataReader.Read())
                        sMessge = oSqlDataReader["MsgDuplicate"].ToString();
                }
            }
            return sMessge;
        }
        /// <summary>
        ///  This is the common method to complete all types of incomplete online transactions.
        /// </summary>
        /// <param name="aoNetBankingTransaction"></param>
        /// <param name="aiPaymentCategoryFeeId"></param>
        /// <returns></returns>
        public void MarkAsInComplete(int aiTranscationId)
        {
           using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TranscationId", aiTranscationId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateInCompleteAdmissionTransactions");     
            }       
        }
        /// <summary>
        /// This method is used to generate the incomplete transaction.
        /// </summary>
        /// <param name="aiAdmissionId"></param>
        /// <param name="oNetBankingTransaction"></param>
        /// <returns></returns>
        public DataTable CreateNetBankingTransaction(NetBankingTransaction aoNetBankingTransaction,int aiAdmissionId = 0)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("PaymentReferenceNumber", aoNetBankingTransaction.PaymentReferenceNumber, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("PaymentITCParameter", aoNetBankingTransaction.PaymentITCParameter, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("TransactionAMT", aoNetBankingTransaction.TransactionAMT, SqlDbType.Decimal);
                oSqlDbUtility.AddParameter("TransactionBankID", aoNetBankingTransaction.TransactionBankID, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("TransactionStatus",Convert.ToChar(aoNetBankingTransaction.TransactionStatus), SqlDbType.Char);
                oSqlDbUtility.AddParameter("TPSLTransactionID", aoNetBankingTransaction.TPSLTransactionID, SqlDbType.NVarChar);
                oSqlDbUtility.AddParameter("AdmissionId", aiAdmissionId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("GatewayId", aoNetBankingTransaction.GatewayId, SqlDbType.Int);

                //using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_InsertNetBankingTransaction"))
                //{
                //    if (oReader.Read() && oReader["ID"] != DBNull.Value)
                //        return oReader["ID"].ToInt();
                //}

                return oSqlDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_InsertNetBankingTransaction");
            }
           // return -1;
        }

        /// <summary>
        /// This method is called to update the online transaction status as failed for success.
        /// </summary>
        /// <param name="oNetBankingTransaction"></param>
        /// <returns></returns>
        public DataTable UpdateOnlineTransactionStatus(NetBankingTransaction aoNetBankingTransaction, bool abIsCautionMoneyPaymenty, bool abIsInternalFeePayment, bool abIsLastYearPayment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                if (aoNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.StudentFee.ToInt() ||
                    aoNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.CautionMoney.ToInt() ||
                    aoNetBankingTransaction.TransactionFor == Constants.OnlineFeeTypes.InternalFee.ToInt()
                    )
                {
                    oSQLServerDbUtility.AddParameter("StudentID", miStudentId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                }
                oSQLServerDbUtility.AddParameter("PaymentITCParameter", aoNetBankingTransaction.PaymentITCParameter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PaymentReferenceNumber", aoNetBankingTransaction.PaymentReferenceNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransactionAMT", aoNetBankingTransaction.TransactionAMT, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("TransactionBankID", aoNetBankingTransaction.TransactionBankID, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TPSLTransactionID", aoNetBankingTransaction.TPSLTransactionID, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransactionStatus",Convert.ToChar(aoNetBankingTransaction.TransactionStatus), SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("IsNextAcademicYear", aoNetBankingTransaction.IsNextAcademicYear, SqlDbType.Bit);
                
                if(abIsCautionMoneyPaymenty)
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_UpdateCautionMoneyTransactionStatus", true);
                else if (abIsInternalFeePayment)
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_UpdateInternalFeeOnlineTransactionStatus", true);
                else
                {
                    oSQLServerDbUtility.AddParameter("IsLastYearPayment", abIsLastYearPayment, SqlDbType.Bit);                    
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_UpdateOnlineTransactionStatus", true);
                }
            }
        }

        /// <summary>
        /// This method is used to save query string received from gateway after the transaction.
        /// </summary>
        /// <param name="aiNetBankingTxnId"></param>
        /// <param name="asQueryString"></param>
        public static void SaveQueryString(int aiNetBankingTxnId, string asQueryString)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TransactionId", aiNetBankingTxnId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("QueryString", asQueryString, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveGatewayQueryString");
            }
        }

        /// <summary>
        /// This method is used to get list onlne payment types.
        /// </summary>
        /// <returns></returns>
        public List<OnlinePaymentType> GetOnlinePaymentTypes()
        {
            List<OnlinePaymentType> lstOnlinePaymentType = new List<OnlinePaymentType>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetOnlinePaymentTypes"))
                {
                    while (oSqlDataReader.Read())
                    {
                        OnlinePaymentType oOnlinePaymentType = new OnlinePaymentType
                        {
                            Id = oSqlDataReader["Id"].ToInt(),
                            Type = oSqlDataReader["Type"].ToString()
                        };
                        lstOnlinePaymentType.Add(oOnlinePaymentType);
                    }
                }
            }

            return lstOnlinePaymentType;
        }

        /// <summary>
        /// This method is used to get all the payment gateway information from database.
        /// </summary>
        /// <returns></returns>
        public static List<PaymentGateWayDetails> GetPaymentGatewayDetails(string asStudentFeeIds, int iScStudentId, string asSelectedFeeType, int aiAcademicYearId, bool abIsInternalFee)
        {
            List<PaymentGateWayDetails> lstPaymentGateWayDetails = new List<PaymentGateWayDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentFeeIds", asStudentFeeIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", iScStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NextAcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SelectedFeeType", asSelectedFeeType, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPaymentGatewayDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        PaymentGateWayDetails oPaymentGateWayDetails = new PaymentGateWayDetails
                        {
                            GatewayId = oSqlDataReader["GatewayId"].ToInt(),
                            NetBankingUrl = oSqlDataReader["NetBankingUrl"].ToString(),
                            Version = oSqlDataReader["Version"].ToString(),
                            Command = oSqlDataReader["Command"].ToString(),
                            AccessCode = oSqlDataReader["AccessCode"].ToString(),
                            MerchantId = oSqlDataReader["MerchantId"].ToString(),
                            Locale = oSqlDataReader["Locale"].ToString(),
                            SuccessCode = oSqlDataReader["SuccessCode"].ToString(),
                            Hash = oSqlDataReader["Hash"].ToString(),
                            Sequence = oSqlDataReader["Sequence"].ToString(),
                            HasBankSelection = oSqlDataReader["HasBankSelection"].ToBool(),
                            PaymentGateway = oSqlDataReader["Gateway"].ToString(),
                            ProductInfo = oSqlDataReader["ProductInfo"].ToString()
                        };
                        lstPaymentGateWayDetails.Add(oPaymentGateWayDetails);
                    }
                }
            }

            return lstPaymentGateWayDetails;
        }

        /// <summary>
        /// This method is used to get the charges details whenever there is no bank selection given on screen.
        /// </summary>
        /// <param name="aiPaymentMethodId"></param>
        /// <param name="aiGatewayId"></param>
        public void GetMinMaxCharges(int aiPaymentMethodId, int aiGatewayId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {   
                oSQLServerDbUtility.AddParameter("PaymentMethodId", aiPaymentMethodId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GatewayId", aiGatewayId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBankChargesDetails"))
                {                    
                    if (oSqlDataReader.Read())
                    {
                        msMinMaxCharges = oSqlDataReader["MinMaxCharges"].ToString();
                        msServiceTaxes = oSqlDataReader["ServiceTax"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// This class is used to get the student details for net banking. We need to send these details to the gateway.
        /// </summary>
        /// <param name="asFormNumber"></param>
        /// <param name="abIsFee"></param>
        /// <returns></returns>
        public StudentNetBankingDetails GetStudentNetBankingDetails(string asFormNumber, bool abIsFee, int aiStandardId, bool abIsNextYearFeePayment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentNetBankingDetails oStudentNetBankingDetails = new StudentNetBankingDetails();
                oSQLServerDbUtility.AddParameter("FormNumber", asFormNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsFee", abIsFee, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsNextYearFeePayment", abIsNextYearFeePayment, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentNetBankingDetails"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oStudentNetBankingDetails.FirstName = oSqlDataReader["FirstName"].ToString();
                        oStudentNetBankingDetails.Email = oSqlDataReader["Email"].ToString();
                        oStudentNetBankingDetails.Phone = oSqlDataReader["Phone"].ToString();
                        oStudentNetBankingDetails.IsPreprimaryStudent = Convert.ToBoolean(oSqlDataReader["IsPreprimaryStudent"]);
                        oStudentNetBankingDetails.SchoolEmailAddress = oSqlDataReader["SchoolEmailAddress"].ToString();
                        oStudentNetBankingDetails.RegNoOrFormNo = oSqlDataReader["RegNoOrFormNo"].ToString();
                    }
                }

                return oStudentNetBankingDetails;
            }            
        }

        /// <summary>
        /// This method is used to get the banks for selected gateway.
        /// </summary>
        /// <param name="aiGatewayId"></param>
        /// <returns></returns>
        public List<Bank> GetBanksForGateway(int aiGatewayId, bool abIsForFee)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {                
                List<Bank> lstBank=new List<Bank>();
                oSQLServerDbUtility.AddParameter("GatewayId", aiGatewayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsForFee", abIsForFee, SqlDbType.Bit); 
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBanksForGateway"))
                {
                    while (oSqlDataReader.Read())
                    {
                        Bank oBank = new Bank
                        {
                            BankCode =  oSqlDataReader["BankCode"].ToString(),
                            Name = oSqlDataReader["Name"].ToString()
                        };

                        lstBank.Add(oBank);
                    }
                }

                return lstBank;
            }            
        }

        // This function is used to insert the NetBankingPaymentTransactions Details
        public virtual int InsertNetBankingPaymentTransactions(ArrayList oArrStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction((String[])oArrStatements.ToArray(typeof(string)));
        }        

        // This function is used to update the NetBankingPaymentTransactions Details
        public virtual void UpdateNetBankingPaymentTransactions()
        {
            string sUpdateStatement = "UPDATE NetBankingPaymentTransactions SET " +
            "PaymentReferenceNumber= N'" + StringUtility.ReplaceSingleQuoteInString(moNetBankingPaymentTransactionsStruct.msPaymentReferenceNumber, true) + "' " +
            ",PaymentITCParameter= N'" + StringUtility.ReplaceSingleQuoteInString(moNetBankingPaymentTransactionsStruct.msPaymentITCParameter, true) + "' " +
            ",TransactionAMT= " + moNetBankingPaymentTransactionsStruct.mdTransactionAMT +
            ",TransactionBankID= " + moNetBankingPaymentTransactionsStruct.miTransactionBankID +
            ",TransactionStatus= N'" + StringUtility.ReplaceSingleQuoteInString(moNetBankingPaymentTransactionsStruct.msTransactionStatus, true) + "' " +
            ",TPSLTransactionID= N'" + StringUtility.ReplaceSingleQuoteInString(moNetBankingPaymentTransactionsStruct.msTPSLTransactionID, true) + "' " +
            ",IsTransactionResponse= N'" + moNetBankingPaymentTransactionsStruct.mblnIsTransactionResponse + "' " +
            "" +
            " WHERE NetBankingPaymentTransactionID=" + moNetBankingPaymentTransactionsStruct.miNetBankingPaymentTransactionID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to delete the NetBankingPaymentTransactions Details
        public virtual void DeleteNetBankingPaymentTransactions()
        {
            string sDeleteStatement = "DELETE NetBankingPaymentTransactions WHERE NetBankingPaymentTransactionID=N'" + moNetBankingPaymentTransactionsStruct.miNetBankingPaymentTransactionID + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        public void UpdateAdmission(int aiTransactionID, int aiAdmissionId)
        {
            string sUpdateStatement = " UPDATE Student_Admissions SET NetBankingPaymentTransactionID = " + aiTransactionID
                                      + " WHERE Student_Admission_Id = " + aiAdmissionId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to load the NetBankingPaymentTransactions Details
        private void LoadNetBankingPaymentTransactionsDetails(string asNetBankingPaymentTransactionID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchNetBankingPaymentTransactionsDetailsFromDatabase(asNetBankingPaymentTransactionID);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["NetBankingPaymentTransactionID"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.miNetBankingPaymentTransactionID = Convert.ToInt32(oDR["NetBankingPaymentTransactionID"]);
                            if (oDR["PaymentReferenceNumber"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.msPaymentReferenceNumber = Convert.ToString(oDR["PaymentReferenceNumber"]);
                            if (oDR["PaymentITCParameter"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.msPaymentITCParameter = Convert.ToString(oDR["PaymentITCParameter"]);
                            if (oDR["TransactionAMT"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.mdTransactionAMT = Convert.ToDouble(oDR["TransactionAMT"]);
                            if (oDR["TransactionBankID"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.miTransactionBankID = oDR["TransactionBankID"].ToString();
                            if (oDR["TransactionStatus"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.msTransactionStatus = Convert.ToString(oDR["TransactionStatus"]);
                            if (oDR["TPSLTransactionID"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.msTPSLTransactionID = Convert.ToString(oDR["TPSLTransactionID"]);
                            if (oDR["IsTransactionResponse"] != DBNull.Value)
                                moNetBankingPaymentTransactionsStruct.mblnIsTransactionResponse = Convert.ToBoolean(oDR["IsTransactionResponse"]);
                        }
                    }
                }
            }
        }

        // This function is used to fetch the NetBankingPaymentTransactions Details
        private String FetchNetBankingPaymentTransactionsDetailsFromDatabase(string asNetBankingPaymentTransactionID)
        {
            string sSelectStatement = " SELECT  " +
            "NetBankingPaymentTransactionID" +
            ",PaymentReferenceNumber" +
            ",PaymentITCParameter" +
            ",TransactionAMT" +
            ",TransactionBankID" +
            ",TransactionStatus" +
            ",TPSLTransactionID" +
            ",IsTransactionResponse" +
            " FROM NetBankingPaymentTransactions" +
            " WHERE NetBankingPaymentTransactionID=" + asNetBankingPaymentTransactionID;
            return sSelectStatement;
        }

        public struct NetBankingPaymentTransactionsStruct
        {

            public int miNetBankingPaymentTransactionID;

            public string msPaymentReferenceNumber;

            public string msPaymentITCParameter;

            public double mdTransactionAMT;

            public string miTransactionBankID;

            public string msTransactionStatus;

            public string msTPSLTransactionID;

            public bool mblnIsTransactionResponse;
        }

    	/// <summary>
    	///		Retrieves Online transactions details based on the specified criterion.
    	/// </summary>
    	/// <param name="aiSchoolId"></param>
    	/// <param name="aiAcademicYearId"></param>
    	/// <param name="abIncludeClearedPayments"></param>
    	/// <param name="aiTransactionId"></param>
    	/// <param name="asStudentNameRegNoFilter"></param>
    	///	<param name="asDateFilterType"></param>
    	///	<param name="adtStartDate"></param>
    	///	<param name="adtEndDate"></param>
    	///<returns></returns>
        public static DataSet FetchOnlineTransactionDetail(int aiSchoolId, int aiAcademicYearId, bool abIncludeClearedPayments, string asTransactionId, string asStudentNameRegNoFilter, string asDateFilterType, DateTime adtStartDate, DateTime adtEndDate, int aiBankId, bool abIncludeCautionMoney, bool abIsInternalFee, int aiPaymentGatewayId)
        {
		    using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("IncludeClearedPayments", abIncludeClearedPayments, SqlDbType.Bit);
				
				if (!asTransactionId.IsNullOrEmpty())
					oSQLServerDbUtility.AddParameter("NetbankinTransactionId", asTransactionId, SqlDbType.NVarChar);
				if (!asStudentNameRegNoFilter.IsNullOrEmpty())
					oSQLServerDbUtility.AddParameter("StudentNameRegNoFilter", asStudentNameRegNoFilter, SqlDbType.NVarChar);
				
				if (!asDateFilterType.IsNullOrEmpty())
				{
					oSQLServerDbUtility.AddParameter("DateFilterType", asDateFilterType, SqlDbType.NVarChar);
					
					if (adtStartDate != DateTime.MinValue)
						oSQLServerDbUtility.AddParameter("StartDate", adtStartDate, SqlDbType.Date);
						
					if (adtEndDate != DateTime.MinValue)
						oSQLServerDbUtility.AddParameter("EndDate", adtEndDate, SqlDbType.Date);

				}

                oSQLServerDbUtility.AddParameter("DepositBankId ", aiBankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeCautionMoney", abIncludeCautionMoney, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsInternalFee", abIsInternalFee, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("GatewayId", aiPaymentGatewayId, SqlDbType.Int);

				return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetOnlineTransactionDetails");
            }
        }

        /// <summary>
        /// Retrieve the list of all successful online admission fee payments details.
        /// </summary>
        /// <param name="asFilter"></param>
        /// <param name="aiOptCheck"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataSet FetchOnlineAdmissionFeeDetail(string asFilter, int aiOptCheck, int aiSchoolId, int aiAcademicYearId, bool aChkAll)
        {
            string sFilter = string.Empty;
            asFilter = asFilter != null ? asFilter : "";
            switch (aiOptCheck)
            {
                case 1:
                    sFilter = " AND NetBankingPaymentTransactions.TPSLTransactionID like '%" + asFilter + "%'";
                    break;
                case 2:
                    sFilter = " AND ( Student_Admissions.Form_Number like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%'"
                                + " OR Student_Admissions.First_Name like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%'"
                                + " OR Student_Admissions.Middle_Name like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%'"
                                + " OR Student_Admissions.Last_Name like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%' )";
                    break;
                case 3:
                    sFilter = " AND Standard_Master.Standard_Name like '%" + asFilter + "%'";
                    break;
            }
            if (aChkAll)
            {
                sFilter = sFilter + " AND Student_Admissions.Acedemic_Year_Id = " + aiAcademicYearId
                                  + " AND Student_Admissions.School_Id = " + aiSchoolId;                                
            }
            else
            {
                  sFilter = sFilter + " AND Student_Admissions.Acedemic_Year_Id = " + aiAcademicYearId
                                  + " AND Student_Admissions.School_Id = " + aiSchoolId  
                                  + " AND NetBankingPaymentTransactions.ClearanceDate IS NULL ";
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetOnlineAdmissionFeeClearanceDetails");
            }
        }

        public static DataSet FetchOnlineAdmissionFeeDetails(string adStartDate, string adEndDate, string asFilter, int aiOptCheck, int aiSchoolId, int aiAcademicYearId, bool aChkAll)  //new added for date filter
        {
            adStartDate = adStartDate + " 00:00 AM";
            adEndDate = adEndDate + " 11:59 PM";
            string sFilter = string.Empty;
            asFilter = asFilter != null ? asFilter : "";
            switch (aiOptCheck)
            {
                case 1:
                    sFilter = " AND NetBankingPaymentTransactions.TPSLTransactionID like '%" + asFilter + "%'";
                    break;
                case 2:
                    sFilter = " AND ( Student_Admissions.Form_Number like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%'"
                                + " OR Student_Admissions.First_Name like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%'"
                                + " OR Student_Admissions.Middle_Name like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%'"
                                + " OR Student_Admissions.Last_Name like '%" + StringUtility.ReplaceSingleQuoteInString(asFilter, false) + "%' )";
                    break;
                case 3:
                    sFilter = " AND Standard_Master.Standard_Name like '%" + asFilter + "%'";
                    break;

                case 4:
                    if (adStartDate != String.Empty && adEndDate != String.Empty)
                        sFilter = String.Format(" AND TransactionDateTime BETWEEN '{0}' AND '{1}'", adStartDate, adEndDate);
                    else if (adStartDate != String.Empty && adEndDate == String.Empty)
                        sFilter = String.Format(" AND TransactionDateTime >= '{0}'", adStartDate);
                    else if (adStartDate == String.Empty && adEndDate != String.Empty)
                        sFilter = String.Format(" AND TransactionDateTime <= '{0}'", adEndDate);
                    break;
                case 5:
                    if (adStartDate != String.Empty && adEndDate != String.Empty && aChkAll == true)
                        sFilter = String.Format(" AND ClearanceDate BETWEEN '{0}' AND '{1}'", adStartDate, adEndDate);
                    else if (adStartDate != String.Empty && adEndDate == String.Empty && aChkAll == true)
                        sFilter = String.Format(" AND ClearanceDate >= '{0}'", adStartDate);
                    else if (adStartDate == String.Empty && adEndDate != String.Empty)
                        sFilter = String.Format(" AND ClearanceDate <= '{0}'", adEndDate);
                    else if ((adStartDate != String.Empty || adEndDate != String.Empty) && aChkAll == false)
                       // sFilter = " AND ReceiptNo = -9999999";
                        sFilter = String.Format(" AND TransactionDateTime BETWEEN '{0}' AND '{1}'", adStartDate, adEndDate);
                    break;
            }
            if (aChkAll)
            {
                sFilter = sFilter + " AND Student_Admissions.Acedemic_Year_Id = " + aiAcademicYearId
                                  + " AND Student_Admissions.School_Id = " + aiSchoolId;
            }
            else
            {
                sFilter = sFilter + " AND Student_Admissions.Acedemic_Year_Id = " + aiAcademicYearId
                                + " AND Student_Admissions.School_Id = " + aiSchoolId
                                + " AND NetBankingPaymentTransactions.ClearanceDate IS NULL ";
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
              
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetOnlineAdmissionFeeClearanceDetails");
            }
        }

        /// <summary>
        /// Retrieve all Standard Names.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataTable FetchStandardNameDetails(int aiSchoolId, int aiAcademicYearId)
        {
            string sQuery = " SELECT DISTINCT Standard_Id, Standard_Name  "
                        + " FROM Standard_Master INNER JOIN Student_Admissions "
                        + " ON Standard_Master.Standard_Id=Student_Admissions.For_Standard "
                        + " where Standard_Master.School_Id = " + aiSchoolId
                        + " AND Standard_Master.academic_Year_Id = " + aiAcademicYearId
                        + " AND Student_Admissions.IsOnlineAdmission = 1";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
            }
        }

        public void UpdateNetBankingPaymentTransactions(String asXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("OnlineTransactionXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_UpdateOnlineTransactionDetails");
            }
        }


        public static DataTable CheckDuplicateTSPLID(String asXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("OnlineTransactionXML", asXML, SqlDbType.Xml);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("USP_DuplicateTSPLTransactionID");
            }
        }

        public void UpdateOnlineAdmissionFeeData(String asXML,int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("OnlineAdmissionFeeXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateOnlineAdmissionFeeDetails");
            }
        }

        public static string GetTransactionBankCode(int aiGatewayId, string asBankName, string asDiscriminator)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("GatewayId", aiGatewayId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("BankName", asBankName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Discriminator", asDiscriminator, SqlDbType.NVarChar);      
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("TransactionBankId", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 50);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetTransactionBankCode");
                return oSqlParameter.Value.ToString();
            }
        }
       
        /// <summary>
        /// This method is used to return active payment gateway URLs for incomplete transaction details.
        /// </summary>
        /// <param name="iSchoolId"></param>
        /// <returns></returns>
        public DataTable GetPaymentGatewayURL()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPaymentGatewayURL", true);
            }
        }


        
    }
}