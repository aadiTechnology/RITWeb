// Class Name       :- NetBankingPaymentTransactionsBL
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
using System.Data.SqlClient;
using SchoolEntities.Accounts;
using DataCommunicator;
using Utility;
using SchoolEntities;
using FeeEntities;
namespace BusinessLogic
{


    public class NetBankingPaymentTransactionsBL : BusinessLogicBaseBL
    {
        #region "Members"

        private NetBankingPaymentTransactionsDC.NetBankingPaymentTransactionsStruct moNetBankingPaymentTransactionsStruct;

        private NetBankingPaymentTransactionsDC moNetBankingPaymentTransactionsDC;

        #endregion

        #region "Constructors"

        public NetBankingPaymentTransactionsBL()
        {
            moNetBankingPaymentTransactionsDC = new NetBankingPaymentTransactionsDC();
        }

        public NetBankingPaymentTransactionsBL(int aiSchoolId)
        {
            this.moNetBankingPaymentTransactionsDC = new NetBankingPaymentTransactionsDC(aiSchoolId);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        public NetBankingPaymentTransactionsBL(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            this.moNetBankingPaymentTransactionsDC = new NetBankingPaymentTransactionsDC(aiSchoolId, aiAcademicYearId, aiStudentId);
        } 

        public NetBankingPaymentTransactionsBL(string asNetBankingPaymentTransactionID)
        {
            moNetBankingPaymentTransactionsDC = new NetBankingPaymentTransactionsDC(asNetBankingPaymentTransactionID);
            moNetBankingPaymentTransactionsStruct = moNetBankingPaymentTransactionsDC.NetBankingPaymentTransactionsStructDetails;
        }

        #endregion

        #region "Properties"

        public string MinMaxCharge
        {
            get { return moNetBankingPaymentTransactionsDC.MinMaxCharge; }
            set { moNetBankingPaymentTransactionsDC.MinMaxCharge = value; }
        }

        public string ServiceTax
        {
            get { return moNetBankingPaymentTransactionsDC.ServiceTax; }
            set { moNetBankingPaymentTransactionsDC.ServiceTax = value; }
        }

        public virtual int NetBankingPaymentTransactionID
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.miNetBankingPaymentTransactionID;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.miNetBankingPaymentTransactionID = value;
            }
        }

        public virtual string PaymentReferenceNumber
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.msPaymentReferenceNumber;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.msPaymentReferenceNumber = value;
            }
        }

        public virtual string PaymentITCParameter
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.msPaymentITCParameter;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.msPaymentITCParameter = value;
            }
        }

        public virtual double TransactionAMT
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.mdTransactionAMT;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.mdTransactionAMT = value;
            }
        }

        public virtual string TransactionBankID
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.miTransactionBankID;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.miTransactionBankID = value;
            }
        }

        public virtual string TransactionStatus
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.msTransactionStatus;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.msTransactionStatus = value;
            }
        }

        public virtual string TPSLTransactionID
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.msTPSLTransactionID;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.msTPSLTransactionID = value;
            }
        }

        public virtual bool IsTransactionResponse
        {
            get
            {
                return moNetBankingPaymentTransactionsStruct.mblnIsTransactionResponse;
            }
            set
            {
                moNetBankingPaymentTransactionsStruct.mblnIsTransactionResponse = value;
            }
        }

        #endregion

        #region "Public Methods"

        public TransactionStatusDetails GetTransactionStatus(int aiSchoolId, int aiGatewayId, string asOrderId)
        {
            return moNetBankingPaymentTransactionsDC.GetTransactionStatus(aiSchoolId, aiGatewayId, asOrderId);
        }

        /// <summary>
        /// Update Clearance date and TPSLTransactionID of Online Transaction Details
        /// </summary>
        /// <param name="asXML"></param>
        public void SetOnlineTransactionDetails(String asXML)
        {
            NetBankingPaymentTransactionsDC oNetBankingPaymentTransactionsDC = new NetBankingPaymentTransactionsDC();
            oNetBankingPaymentTransactionsDC.UpdateNetBankingPaymentTransactions(asXML);
        }

        /// <summary>
        /// Update Clearance date and TPSLTransactionID of Online Admission Fee. 
        /// </summary>
        /// <param name="asXML"></param>
        public void SetOnlineAdmissionFeeDetails(String asXML,int aiSchoolId, int aiAcademicYearId)
        {
            NetBankingPaymentTransactionsDC oNetBankingPaymentTransactionsDC = new NetBankingPaymentTransactionsDC();
            oNetBankingPaymentTransactionsDC.UpdateOnlineAdmissionFeeData(asXML, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// Get student details of Duplicate TPSLTransactionID
        /// </summary>
        /// <param name="asXML"></param>
        /// <returns></returns>
        public static DataTable IsTSPLIDuplicate(String asXML)
        {
           return NetBankingPaymentTransactionsDC.CheckDuplicateTSPLID(asXML);
        }

        /// <summary>
        /// This method is used to pay student fee online for current year.
        /// </summary>
        /// <param name="aiLateFeeAmount"></param>
        /// <param name="asRemarks"></param>
        /// <param name="asLateFeeRemark"></param>
        /// <param name="asStudentFeeIdXML"></param>
        /// <param name="aoNetBankingTransaction"></param>
        /// <param name="bIsPayFromMobile"></param>
        /// <returns></returns>
        public DataTable PayStudentFeeOnLine(int aiLateFeeAmount, string asRemarks, string asLateFeeRemark, string asStudentFeeIdXML, NetBankingTransaction aoNetBankingTransaction, bool bIsPayFromMobile = false, bool bIsCautionMoneyPayment = false, bool abIsInternalFeePayment = false, int aiAcademicYearId=0)
        {
            return moNetBankingPaymentTransactionsDC.PayStudentFeeOnLine(aiLateFeeAmount, asRemarks, asLateFeeRemark, asStudentFeeIdXML, aoNetBankingTransaction, bIsPayFromMobile, bIsCautionMoneyPayment, abIsInternalFeePayment, aiAcademicYearId);
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
        public DataTable PayStudentNextYearFeeOnLine(int aiStandardId, string asRemarks, string asDueDatesFilterXML, int aiLateFeeAmount, NetBankingTransaction aoNetBankingTransaction, bool abIsForIntrnalFee, string asInternalFeeDetailsIds, string asSelectedFeeType = "")
        {
            return moNetBankingPaymentTransactionsDC.PayStudentNextYearFeeOnLine(aiStandardId, asRemarks, asDueDatesFilterXML, aiLateFeeAmount, aoNetBankingTransaction, abIsForIntrnalFee, asInternalFeeDetailsIds, asSelectedFeeType);
        }

        /// <summary>
        /// This method is used to delete the fee transaction from superadmin screen.
        /// </summary>
        /// <param name="aiTranscationId"></param>
        /// <param name="aiUserId"></param>
        public void DeleteTransactionDetails(int aiTranscationId, int aiUserId, Constants.OnlineFeeTypes aoOnlineFeeType)
        {
            moNetBankingPaymentTransactionsDC.DeleteTransactionDetails(aiTranscationId, aiUserId,aoOnlineFeeType);
        }
        
        /// <summary>
        /// This is the common method to complete all types of incomplete online transactions.
        /// </summary>       
        public string CompleteTransactionDetails(NetBankingTransaction aoNetBankingTransaction, int aiPaymentCategoryFeeId)
        {
            return moNetBankingPaymentTransactionsDC.CompleteTransactionDetails(aoNetBankingTransaction, aiPaymentCategoryFeeId);
        }

        /// <summary>
        /// This is the common method to complete all types of incomplete online transactions.
        /// </summary>
        /// <param name="aoNetBankingTransaction"></param>
        /// <param name="aiPaymentCategoryFeeId"></param>
        /// <returns></returns>
        public void MarkAsInComplete(int aiTranscationId)
        {
            moNetBankingPaymentTransactionsDC.MarkAsInComplete(aiTranscationId);
        }
        /// <summary>
        /// This method is used to generate the incomplete transaction.
        /// </summary>
        /// <param name="aiAdmissionId"></param>
        /// <param name="oNetBankingTransaction"></param>
        /// <returns></returns>
        public DataTable CreateNetBankingTransaction(NetBankingTransaction aoNetBankingTransaction, int aiAdmissionId = 0)
        {
            return moNetBankingPaymentTransactionsDC.CreateNetBankingTransaction(aoNetBankingTransaction,aiAdmissionId);
        }

        /// <summary>
        /// This method is called to update the online transaction status as failed for success.
        /// </summary>
        /// <param name="oNetBankingTransaction"></param>
        /// <returns></returns>
        public DataTable UpdateOnlineTransactionStatus(NetBankingTransaction aoNetBankingTransaction, bool abIsCautionMoneyPaymenty, bool abIsInternalFeePayment, bool abIsLastYearPayment = false)
        {
            return moNetBankingPaymentTransactionsDC.UpdateOnlineTransactionStatus(aoNetBankingTransaction, abIsCautionMoneyPaymenty, abIsInternalFeePayment, abIsLastYearPayment);
        }

        /// <summary>
        /// This method is used to save query string received from gateway after the transaction.
        /// </summary>
        /// <param name="aiNetBankingTxnId"></param>
        /// <param name="asQueryString"></param>
        public static void SaveQueryString(int aiNetBankingTxnId, string asQueryString)
        {
            NetBankingPaymentTransactionsDC.SaveQueryString(aiNetBankingTxnId, asQueryString);
        }

        /// <summary>
        /// This method is used to get list onlne payment types.
        /// </summary>
        /// <returns></returns>
        public List<OnlinePaymentType> GetOnlinePaymentTypes()
        {
            return moNetBankingPaymentTransactionsDC.GetOnlinePaymentTypes();
        }

        /// <summary>
        /// This method is used to get all the payment gateway information from database.
        /// </summary>
        /// <returns></returns>
        public static List<PaymentGateWayDetails> GetPaymentGatewayDetails(string asStudentFeeIds, int iScStudentId = 0, string asSelectedFeeType = "", int aiAcademicYearId = 0, bool abIsInternalFee=false)
        {
            return NetBankingPaymentTransactionsDC.GetPaymentGatewayDetails(asStudentFeeIds, iScStudentId, asSelectedFeeType, aiAcademicYearId, abIsInternalFee);
        }

        /// <summary>
        /// This class is used to get the student details for net banking. We need to send these details to the gateway.
        /// </summary>
        /// <param name="asFormNumber"></param>
        /// <param name="abIsFee"></param>
        /// <returns></returns>
        public StudentNetBankingDetails GetStudentNetBankingDetails(string asFormNumber, bool abIsFee, int aiStandardId = 0, bool abIsNextYearFeePayment = false)
        {
            return moNetBankingPaymentTransactionsDC.GetStudentNetBankingDetails(asFormNumber, abIsFee, aiStandardId, abIsNextYearFeePayment);
        }

        /// <summary>
        /// This method is used to get the banks for selected gateway.
        /// </summary>
        /// <param name="aiGatewayId"></param>
        /// <returns></returns>
        public List<Bank> GetBanksForGateway(int aiGatewayId,bool abIsForFee)
        {
            return moNetBankingPaymentTransactionsDC.GetBanksForGateway(aiGatewayId,abIsForFee);
        }

        /// <summary>
        /// This method is used to get the charges details whenever there is no bank selection given on screen.
        /// </summary>
        /// <param name="aiPaymentMethodId"></param>
        /// <param name="aiGatewayId"></param>
        public void GetMinMaxCharges(int aiPaymentMethodId, int aiGatewayId)
        {
            moNetBankingPaymentTransactionsDC.GetMinMaxCharges(aiPaymentMethodId, aiGatewayId);
        }

        /// <summary>
        /// Update  Net Banking Transactions details 
        /// </summary>
        public virtual void UpdateNetBankingPaymentTransactions()
        {
            moNetBankingPaymentTransactionsDC.NetBankingPaymentTransactionsStructDetails = moNetBankingPaymentTransactionsStruct;
            moNetBankingPaymentTransactionsDC.UpdateNetBankingPaymentTransactions();
        }

        /// <summary>
        /// Delete the  Net Banking Transactions details. 
        /// </summary>
        public virtual void DeleteNetBankingPaymentTransactions()
        {
            moNetBankingPaymentTransactionsDC.NetBankingPaymentTransactionsStructDetails = moNetBankingPaymentTransactionsStruct;
            moNetBankingPaymentTransactionsDC.DeleteNetBankingPaymentTransactions();
        }

        public void UpdateAdmission(int aiTransactionID, int aiAdmissionId)
        {
            moNetBankingPaymentTransactionsDC.UpdateAdmission(aiTransactionID, aiAdmissionId);
        }

        /// <summary>
        ///		Retrieves Online transactions details based on the specified criterion.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abIncludeClearedPayments"></param>
        /// <param name="aiTransactionId"></param>
        /// <param name="asStudentNameRegNoFilter"></param>
        /// <param name="adtClearanceStartDate"></param>
        /// <param name="adtClearanceEndDate"></param>
        /// <param name="adtPaymentStartDate"></param>
        /// <param name="adtPaymentEndDate"></param>
        /// <returns></returns>
        public static DataSet FetchOnlineTransactionDetail(int aiSchoolId, int aiAcademicYearId, bool abIncludeClearedPayments, string asTransactionId, string asStudentNameRegNoFilter, DateTime adtClearanceStartDate, DateTime adtClearanceEndDate, DateTime adtPaymentStartDate, DateTime adtPaymentEndDate, bool abIncludeCautionMoney, bool abIsInternalFee, int aiPaymentGatewayId, int aiBankId = 0)
        {
            string sDateFilterType = String.Empty;
			DateTime dtStartDate = DateTime.MinValue;
			DateTime dtEndDate = DateTime.MinValue;
			
			if (adtClearanceStartDate != DateTime.MinValue || adtClearanceEndDate != DateTime.MinValue)
			{
				sDateFilterType = "Clearance";
				dtStartDate = adtClearanceStartDate;
				dtEndDate = adtClearanceEndDate;
			}
			else if (adtPaymentStartDate != DateTime.MinValue || adtPaymentEndDate != DateTime.MinValue)
			{
				sDateFilterType = "Payment";
				dtStartDate = adtPaymentStartDate;
				dtEndDate = adtPaymentEndDate;
			}

            return NetBankingPaymentTransactionsDC.FetchOnlineTransactionDetail(aiSchoolId, aiAcademicYearId, abIncludeClearedPayments, asTransactionId, asStudentNameRegNoFilter, sDateFilterType, dtStartDate, dtEndDate, aiBankId, abIncludeCautionMoney, abIsInternalFee, aiPaymentGatewayId);
        }
        /// <summary>
        /// Retrieve the list of all successful online admission fee payments details.
        /// </summary>
        /// <param name="asFilter"></param>
        /// <param name="aiOptCheck"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataSet FetchOnlineAdmissionFeeDetail(string asFilter, int aiOptCheck, int aiSchoolId, int aiAcademicYearId,bool aChkAll)
        {
            return NetBankingPaymentTransactionsDC.FetchOnlineAdmissionFeeDetail(asFilter, aiOptCheck, aiSchoolId, aiAcademicYearId, aChkAll);
        }
        public static DataSet FetchOnlineAdmissionFeeDetails(string adStartDate, string adEndDate, string asFilter, int aiOptCheck, int aiSchoolId, int aiAcademicYearId, bool aChkAll)   // new for date filter
        {
            return NetBankingPaymentTransactionsDC.FetchOnlineAdmissionFeeDetails(adStartDate, adEndDate,asFilter, aiOptCheck, aiSchoolId, aiAcademicYearId, aChkAll);
        }

        /// <summary>
        /// Retrieve all Standard Names.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataTable FetchStandardNameDetails(int aiSchoolId, int aiAcademicYearId)
        {
            return NetBankingPaymentTransactionsDC.FetchStandardNameDetails(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This metohd is used to return transaction code.
        /// </summary>
        /// <param name="aiGatewayId"></param>
        /// <param name="asBankName"></param>
        /// <returns></returns>
        public static string GetTransactionBankCode(int aiGatewayId, string asBankName, string asDiscriminator)
        {
            return NetBankingPaymentTransactionsDC.GetTransactionBankCode(aiGatewayId, asBankName, asDiscriminator);
        }

        /// <summary>
        /// This method is used to return active payment gateway URLs for incomplete transaction details.
        /// </summary>
        /// <param name="iSchoolID"></param>
        /// <returns></returns>
        public DataTable GetPaymentGatewayURL()
        {
            return moNetBankingPaymentTransactionsDC.GetPaymentGatewayURL(); ;
        }

        #endregion
    }
}
